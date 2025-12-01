using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public int hp = 7;   //ヒットポイント
    public float reactionDistance = 7.0f;   //反応距離
    public float Speed = 0.1f;    //移動スピード

    public GameObject bulletPrefab;    //弾
    public float shootSpeed = 5.0f;    //弾の速度
    public float shootInterval = 1.5f; //攻撃間隔

    //アイテムドロップ
    public GameObject[] dropItems;   //ドロップするアイテムリスト
    public float dropRate = 1.0f;    //ドロップ確率100％

    //攻撃中フラグ
    bool inAttack = false;
    float shootTimer = 0f;

    //ダメージ時色変更
    bool isBlink = false;
    float blinkTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isBlink)
        {
            blinkTimer -= Time.deltaTime;

            if (blinkTimer > 0f)
            {
                // 赤色
                GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                isBlink = false;
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 pr = player.transform.position;
        float dist = Vector2.Distance(transform.position, pr);

        if (dist <= reactionDistance)
        {
            //プレイヤーについていく
            transform.position = Vector3.MoveTowards(transform.position, pr, Speed * Time.deltaTime);
            
            //攻撃アニメーション
            if (!inAttack)
            {
                inAttack = true;
                GetComponent<Animator>().Play("GhostAttack");
            }

            //一定間隔で弾を撃つ
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                GetComponent<Animator>().Play("GhostAttack");


                Attack();
                shootTimer = 0f;
            }
        }
        else
        {
            //離れたらアニメーションを戻す
            if (inAttack)
            {
                inAttack = false;
                GetComponent<Animator>().Play("Ghost");
            }

            //タイマーリセット
            shootTimer = 0f;
        }

            
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "sword")
        {
            //ダメージ
            SwordHit swordhit = collision.gameObject.GetComponent<SwordHit>();
            if (swordhit == null) return;

          //  hp -= swordhit.CurrentDamage;


            //ダメージ時赤色
            isBlink = true;
            blinkTimer = 0.1f;

            if (hp <= 0)
            {
                //死亡
                //当たりを消す
                GetComponent<Collider2D>().enabled = false;
                //アニメーションを消す
                GetComponent<Animator>().Play("GhostDead");

                //ドロップ
                TryDropItem();

                //１秒後に消す
                Destroy(gameObject, 1);
            }
        }
    }
    //攻撃
    void Attack()
    {
        //発射口オブジェクトを取得
        Transform tr = transform.Find("gate2");
        GameObject gate2 = tr.gameObject;
        //弾を発射するベクトルを作る
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dx = player.transform.position.x - gate2.transform.position.x;
            float dy = player.transform.position.y - gate2.transform.position.y;
            //アークタンジェント２関数で角度（ラジアン）を求める
            float rad = Mathf.Atan2(dy, dx);
            //ラジアンを度に変換して返す
            float angle = rad * Mathf.Rad2Deg + 90f;
            //Prefabから弾のゲームオブジェクトを作る（進行方向に回転）
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate2.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            //発射
            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }

    //アイテムドロップ関数
    void TryDropItem()
    {
        //設定していなければそのまま
        if (dropItems.Length == 0) return;

        float r = Random.value;  //0～1の乱数
        if (r <= dropRate)
        {
            //ランダムでアイテムを選択
            GameObject drop =
                dropItems[Random.Range(0, dropItems.Length)];

            //敵の位置にドロップ
            Instantiate(drop, transform.position,
                Quaternion.identity);
        }
    }
}