using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public float baseSpeed = 3.0f;    //移動スピード
    private float extraSpeed = 0f; //アイテムで増える速度
   public float Speed => baseSpeed + extraSpeed; //現在の速度

    //効果時間
    private float speedTimer = 0f;
    private Coroutine speedCoroutine;

    public void AddSpeed(float amount, float duration = 10f)
    {
        extraSpeed += amount;

        speedTimer = duration;
        
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
        }

        StartCoroutine(RemoveSpeedAfterTime(amount, duration));
    }

    public int direction = 0;     //移動方向
    float axisH;                  //横軸
    float axisV;                  //縦軸
    public float angleZ = -90.0f; //回転速度
    Rigidbody2D rbody;            //Rigidbody2D
    Animator animator;            //Animator

    SpriteRenderer spriteRenderer;
    public GameObject sword;

    bool isMoving = false;        //移動中フラグ
    bool isInvincible = false;    // 無敵時間中フラグ

    //ダメージ対応
    public static int hp = 10;       //プレイヤーのHP
    public static string gameState;  //ゲームの状態
    bool inDamage = false;           //ダメージ中のフラグ

    public float hitBackForce = 4.0f;
    public float hitBackDuration = 0.2f;

    //攻撃力
    public SwordHit weapon;


    //p1からp2の角度を返す
    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            //移動中であれば角度を更新する
            //p1からp2への差分（原点を０にするため）
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            //アークタンジェント２関数で関数（ラジアン）を求める
            float rad = Mathf.Atan2(dy, dx);
            //ラジアンを度に変換して返す
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            //停止中であれば以前の角度を維持
            angle = angleZ;
        }
        return angle;
    }

    public static void ResetStaticVariables()
    {
        hp = 10;
        gameState = "playing";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetStaticVariables();

        Application.targetFrameRate = 60;

        rbody = GetComponent<Rigidbody2D>();    //Rigidbody2Dを得る
        rbody.bodyType = RigidbodyType2D.Dynamic;
        rbody.gravityScale = 0;


        animator = GetComponent<Animator>();    //Animatorを得る

        //SpriteRendererを得る
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        //ゲーム中以外は何もしない
        if (gameState != "playing")
        {
            return;
        }

        axisH = Input.GetAxisRaw("Horizontal");   //左右キー入力
        axisV = Input.GetAxisRaw("Vertical");     //上下キー入力

        Vector2 move = new Vector2(axisH, axisV);

        if (move != Vector2.zero)
        {
            //移動角度から向いている方向とアニメーション更新
            float angle = Mathf.Atan2(axisV, axisH) * Mathf.Rad2Deg;

            int dir;

            if (angle >= -45f && angle < 45f)
            {
                dir = 3; // 右
            }
            else if (angle >= 45f && angle < 135f)
            {
                dir = 2;          // 上
            }
            else if (angle >= -135f && angle < -45f)
            {
                dir = 0;          // 下
            }
            else
            {
                dir = 1;          // 左
            }

            if (dir != direction)
            {
                direction = dir;
                animator.SetInteger("Direction", direction);
            }
        }

        animator.SetFloat("Speed", move.sqrMagnitude);

        // ダメージ点滅
        if (inDamage)
        {
            spriteRenderer.enabled = Mathf.Sin(Time.time * 50) > 0;
            return;
        }
        spriteRenderer.enabled = true;

        // 無敵点滅
        if (isInvincible)
        {
            if (Mathf.Sin(Time.time * 30) > 0)
            {
                spriteRenderer.color = new Color(1f, 0.4f, 0.4f);
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    void FixedUpdate()
    {
        if (gameState != "playing") return;
        if (inDamage) return;

        Vector2 move = new Vector2(axisH, axisV);
        rbody.velocity = move * Speed;
    }


    public void SetAxis(float h, float v)
    {
        axisH = h;
        axisV = v;
        if (axisH == 0 && axisV == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    //接触
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
    }

    //ダメージ
    void GetDamage(GameObject enemy)
    {
        if (gameState == "playing")
        {
            if (isInvincible) return;

            hp--;  //HPを減らす
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
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
    }

    private IEnumerator RemoveSpeedAfterTime(float amount, float duration)
    {
        yield return new WaitForSeconds(duration);  //10秒待つ
        extraSpeed -= amount;                        //スピード元に戻る
    }
}
