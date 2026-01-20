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
    //ダメージ時少し後ろに下がる
    public float hitBackForce = 4.0f;

    //攻撃力
    public SwordHit weapon;

    //SE
    public AudioClip DamageSE;
    public AudioClip DeadSE;

    //敵に当たり続けたら継続でダメージ
    bool touchingEnemy = false;
    GameObject currentEnemy = null;

    /// <summary>
    /// もう一度ゲームを始める時にHPを満タンに戻す
    /// </summary>
    public static void ResetStaticVariables()
    {
        hp = 10;
        gameState = "playing";
    }

    /// <summary>
    /// 主人公の初期化
    /// </summary>
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetStaticVariables();

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

    // Update is called once per frame
    void Update()
    {

        axisH = Input.GetAxisRaw("Horizontal");   //左右キー入力
        axisV = Input.GetAxisRaw("Vertical");     //上下キー入力

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

        if (gameState == "playing" && !inDamage)
        {
            Vector2 move = new Vector2(axisH, axisV);

            if (move.magnitude > 1f)
            {
                move = move.normalized;
            }

            rbody.velocity = move * Speed;
        }


        if (gameState != "playing")
        {
            rbody.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0);
            return;
        }

        if (axisH != 0 || axisV != 0)
        {
            angleZ = Mathf.Atan2(axisV, axisH) * Mathf.Rad2Deg;
        }


        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        if (spriteRenderer == null)
        {
            return;
        }

        
        if (inDamage)
        {
            float val = Mathf.Sin(Time.time * 50);
            spriteRenderer.enabled = val > 0;
            return;
        }
        else
        {
            spriteRenderer.enabled = true;
        }

        //ダメージ時赤点滅
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

        //スピードアップタイム
        if (SpeedTime > 0f)
        {
            SpeedTime -= Time.deltaTime;

            if (SpeedTime <= 0f)
            {
                extraSpeed = 0f;  //効果終了
                SpeedTime = 0f;
            }
        }
    }

    //接触
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            touchingEnemy = true;
            currentEnemy = collision.gameObject;

            GetDamage(collision.gameObject); //初めの一発
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            touchingEnemy = false;
            currentEnemy = null;
        }
    }

    //ダメージ
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
    //ダメージ終了
    void DamageEnd()
    {
        if (gameObject == null) return;

        inDamage = false;                                             //ダメージフラグ OFF

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
}
