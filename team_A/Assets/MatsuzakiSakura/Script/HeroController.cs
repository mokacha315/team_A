using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public float baseSpeed = 3.0f;    //移動スピード
    private float extraSpeed = 0f;    //アイテムで増える速度
   public float Speed => baseSpeed + extraSpeed; //現在の速度


    private float SpeedTime = 0f;
    /// <summary>
    /// スピードアップの効果時間
    /// </summary>
    /// <param name="amount">スピード</param>
    /// <param name="duration">効果時間</param>
    public void AddSpeed(float amount, float duration = 10f)
    {
        Debug.Log($"AddSpeed 呼ばれた amount={amount}, duration={duration}");
        extraSpeed = amount;
        SpeedTime = duration;
    }


    public int direction = 0;     //移動方向
    float axisH;                  //横軸
    float axisV;                  //縦軸
    public float angleZ = -90.0f; //回転速度
    Rigidbody2D rbody;            //Rigidbody2D
    Animator animator;            //Animator
    AudioSource audioSource;      //オーディオソース

    SpriteRenderer spriteRenderer;
    public GameObject sword;

    bool isInvincible = false;    //無敵時間中フラグ

    //ダメージ対応
    public static int hp = 10;       //プレイヤーのHP
    public static string gameState;  //ゲームの状態
    bool inDamage = false;           //ダメージ中のフラグ
    //ダメージ時ヒットバック　
    public float hitBackForce = 4.0f;

    //攻撃力
    public SwordHit weapon;

    //SE
    public AudioClip DamageSE;
    public AudioClip DeadSE;

    //敵に当たり続けたら継続でダメージ
    bool touchingEnemy = false;
    GameObject currentEnemy = null;

    Vector2 moveInput;

    /// <summary>
    /// 主人公の初期化
    /// </summary>
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = 10;
        gameState = "playing";

        Application.targetFrameRate = 60;

        rbody = GetComponent<Rigidbody2D>();            //Rigidbody2Dを得る
        rbody.bodyType = RigidbodyType2D.Dynamic;
        rbody.gravityScale = 0;

        animator = GetComponent<Animator>();            //Animatorを得る
        audioSource = GetComponent<AudioSource>();      //AudioSourceを得る
        spriteRenderer = GetComponent<SpriteRenderer>();//SpriteRendererを得る

        //ゲームの状態をプレイ中にする
        gameState = "playing";

        //剣を戻す
        if (sword != null)
        {
            sword.SetActive(true);
        }
    }


    /// <summary>
    /// 主人公の移動処理・向き・アニメーション更新
    /// ゲーム中以外の時主人公の動きを止める・点滅処理
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //スピードアップタイム
        if (SpeedTime > 0f)
        {
            SpeedTime -= Time.deltaTime;

            if (SpeedTime <= 0f)
            {
                Debug.Log("Speed リセットされた");
                extraSpeed = 0f;  //効果終了
                SpeedTime = 0f;
            }
        }

        axisH = Input.GetAxisRaw("Horizontal");   //左右キー入力
        axisV = Input.GetAxisRaw("Vertical");     //上下キー入力

        moveInput = new Vector2(axisH, axisV);
        if (moveInput.magnitude > 1f)
        {
            moveInput = moveInput.normalized;
        }

        if (axisH != 0 || axisV != 0)
        {
            //移動角度から向いている方向とアニメーション更新
            float angle = Mathf.Atan2(axisV, axisH) * Mathf.Rad2Deg;

            int dir;

            if (angle >= -45f && angle <= 45f)
            {
                dir = 3;          // 右
            }
            else if (angle > 45f && angle < 135f)
            {
                dir = 2;          // 上
            }
            else if (angle >= 135f || angle <= -135f)
            {
                dir = 1;          // 左
            }
            else
            {
                dir = 0;          // 下
            }

            if (dir != direction)
            {
                direction = dir;
                animator.SetInteger("Direction", direction);
            }
        }


        if (gameState != "playing")
        {
            rbody.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }

        //角度更新
        if (axisH != 0 || axisV != 0)
        {
            angleZ = Mathf.Atan2(axisV, axisH) * Mathf.Rad2Deg;
        }

        //SpriteRendererがなければ終了
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        if (spriteRenderer == null)
        {
            return;
        }

        //ダメージ時点滅表示
        if (inDamage)
        {
            float val = Mathf.Sin(Time.time * 50);
            spriteRenderer.enabled = val > 0;
        }
        else
        {
            spriteRenderer.enabled = true;
        }

        //赤点滅
        if (isInvincible)
        {
            float val = Mathf.Sin(Time.time * 30);     //点滅速度

            if (val > 0)
            {
                sr.color = new Color(1f, 0.4f, 0.4f);   //赤
            }
            else
            {
                sr.color = Color.white;   //戻す
            }
        }
        else
        {
            sr.color = Color.white;
        }
    }

    /// <summary>
    /// 敵への接触したときの処理
    /// </summary>
    /// <param name="collision">当たった敵の情報(タグ)</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            touchingEnemy = true;
            currentEnemy = collision.gameObject;

            GetDamage(collision.gameObject); //初めのダメージ
        }
    }

    /// <summary>
    /// 敵への接触が終わった時に呼ばれて、接触フラグを解除する
    /// </summary>
    /// <param name="collision">当たった敵の情報(タグ)</param>
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            touchingEnemy = false;
            currentEnemy = null;
        }
    }

    /// <summary>
    /// 敵に接触したときのダメージの処理
    /// HPを減らし、その後に無敵時間・ヒットバック
    /// </summary>
    /// <param name="enemy">敵</param>
    void GetDamage(GameObject enemy)
    {
        if (gameState == "playing")
        {
            if (isInvincible) return;

            hp--;  //HPを減らす

            audioSource.PlayOneShot(DamageSE);

            if (hp > 0)
            {
                 isInvincible = true;    //無敵ON
                 Invoke("InvincibleEnd", 1.0f);  

                 //敵キャラの反対方向にヒットバックさせる
                 Vector3 v = (transform.position - enemy.transform.position).normalized;
                 rbody.AddForce(new Vector2(v.x, v.y) * hitBackForce, ForceMode2D.Impulse);
                 //ダメージフラグ ON
                 inDamage = true;
                 Invoke("DamageEnd", 0.25f);
            }
            else
            {
                 //ゲームオーバー
                 GameOver();
            }
        }
    }

    /// <summary>
    /// ダメージ演出終了後にダメージグラフOFF
    /// HPが0になった時に主人公の動きを止めてGameOverの文字やBGM・アニメーションを出す
    /// </summary>
    void DamageEnd()
    {
        if (gameObject == null) return;

        inDamage = false;                  //ダメージフラグ OFF

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }
    //ゲームオーバー 
    void GameOver()
    {
        gameState = "gameover";

        audioSource.PlayOneShot(DeadSE);

        CancelInvoke();

        //ゲームオーバー演出
        GetComponent<CircleCollider2D>().enabled = false;             //プレイヤーの当たりを消す
        rbody.linearVelocity = new Vector2(0, 0);                     //移動停止
        rbody.gravityScale = 1;                                       //重力を戻す
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);       //プレイヤーを上に少し跳ね上げる
        animator.SetBool("IsDead", true);                             //アニメーションを切り替える

        this.enabled = false;
        Destroy(gameObject, 1.0f);                                    //１秒後にプレイヤーを消す
    }

    /// <summary>
    /// 無敵時間の終了の処理
    /// 点滅解除・無敵時間終了後も敵に当たっていた場合継続ダメージ
    /// </summary>
    void InvincibleEnd()
    {
        if (gameObject == null) return;

        isInvincible = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        //無敵が終わった後に、まだ敵に当たっていたら継続ダメ
        if (touchingEnemy && currentEnemy != null && gameState == "playing")
        {
            GetDamage(currentEnemy);
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    void FixedUpdate()
    {
        if (gameState == "playing" && !inDamage)
        {
            rbody.velocity = moveInput * Speed;
        }
        else
        {
            rbody.velocity = Vector2.zero;
        }
    }
}
