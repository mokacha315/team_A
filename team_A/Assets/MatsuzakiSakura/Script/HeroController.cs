using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public float speed = 7.0f;    //移動スピード
    public int direction = 0;     //移動方向
    float axisH;                  //横軸
    float axisV;                  //縦軸
    public float angleZ = -90.0f; //回転速度
    Rigidbody2D rbody;            //Rigidbody2D
    Animator animator;            //Animator
    bool isMoving = false;        //移動中フラグ
    bool isInvincible = false;    // 無敵時間中フラグ

    //ダメージ対応
    public static int hp = 10;       //プレイヤーのHP
    public static string gameState;  //ゲームの状態
    bool inDamage = false;           //ダメージ中のフラグ

    public float hitBackForce = 4.0f;
    public float hitBackDuration = 0.2f;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        rbody = GetComponent<Rigidbody2D>();    //Rigidbody2Dを得る
        animator = GetComponent<Animator>();    //Animatorを得る

        //ゲームの状態をプレイ中にする
        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム中以外は何もしない
        if (gameState != "playing")
        {
            return;
        }


        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal");   //左右キー入力
            axisV = Input.GetAxisRaw("Vertical");     //上下キー入力
        }
        //キー入力から移動角度を求める
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);
        //移動角度から向いている方向とアニメーション更新
        int dir;
        if (angleZ >= -60 && angleZ < 60)
        {
            //右向き
            dir = 3;
        }
        else if (angleZ >= 60 && angleZ <= 120)
        {
            //上向き
            dir = 2;
        }
        else if (angleZ >= -120 && angleZ <= -60)
        {
            //下向き
            dir = 0;
        }
        else
        {
            //左向き
            dir = 1;
        }
        if (dir != direction)
        {
            direction = dir;
            animator.SetInteger("Direction", direction);
        }
    }

    private void FixedUpdate()
    {
        if (gameObject == null) return;


        if (gameState != "playing")
        {
            rbody.linearVelocity = Vector2.zero;  
            animator.SetFloat("Speed", 0);        
            return;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        if (inDamage)
        {
            float val = Mathf.Sin(Time.time * 50);
            sr.enabled = val > 0;
            return;
        }
        else
        {
            sr.enabled = true;
        }

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


        //移動速度を更新する
        if (!inDamage)
        {
            rbody.linearVelocity = new Vector2(axisH, axisV).normalized * speed;
        }
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

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = true;
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

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
        }
    }
}
