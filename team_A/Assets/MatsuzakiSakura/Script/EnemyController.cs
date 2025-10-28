using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //ヒットポイント
    public int hp = 10;
    //移動スピード
    public float speed = 1.0f;  //反応距離
    public float reactionDistance = 4.0f;
    float axisH;                //横軸値（-1.0 ∼ 0.0 ∼ 1.0）
    float axisV;                //縦軸値（-1.0 ∼ 0.0 ∼ 1.0）
    Rigidbody2D rbody;          //Rigidbody 2D
    Animator animator;          //Animator
    bool isActive = false;      //アクティブフラグ
    public int arrange = 0;     //配置の識別に使う


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();      //Rigidbody2Dを得る
        animator = GetComponent<Animator>();      //Animatorを得る
    }

    // Update is called once per frame
    void Update()
    {
        //移動値初期化
        axisH = 0;
        axisV = 0;
        //Playerのゲームオブジェクトを得る
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //プレイヤーとの距離チェック
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < reactionDistance)
            {
                isActive = true;       //アクティブにする
            }
            else
            {
                isActive = false;      //非アクティブにする
            }
            //アニメーションを切り替える
            animator.SetBool("IsActive", isActive);
            if (isActive)
            {
                animator.SetBool("IsActive", isActive);
                //プレイヤーへの角度を求める
                float dx = player.transform.position.x - transform.position.x;
                float dy = player.transform.position.y - transform.position.y;
                float rad = Mathf.Atan2(dy, dx);
                float angle = rad * Mathf.Rad2Deg;
                //移動角度でアニメーションを変更する
                int direction;
                if (angle > -45.0f && angle <= 45.0f)
                {
                    direction = 3;    //右向き
                }
                else if (angle > 45.0f && angle <= 135.0f)
                {
                    direction = 2;    //上向き
                }
                else if (angle >= 135.0f && angle <= -45.0f)
                {
                    direction = 0;    //下向き
                }
                else
                {
                    direction = 1;    //左向き
                }
                animator.SetInteger("Direction", direction);
                //移動するベクトルを作る
                axisH = Mathf.Cos(rad) * speed;
                axisV = Mathf.Sin(rad) * speed;
            }
        }
        else
        {
            isActive = false;
        }
    }


    void FixedUpdate()
    {
        if (isActive && hp > 0)
        {
            //移動
            rbody.linearVelocity = new Vector2(axisH, axisV).normalized;
        }
        else
        {
            rbody.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "sword")
        {
            //ダメージ
            hp--;
            if (hp <= 0)
            {
                //死亡
                //当たりを消す
                GetComponent<Collider2D>().enabled = false;
                //アニメーションを切り替える
                animator.SetBool("IsDead", true);
                //0.5秒後に消す
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
