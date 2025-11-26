using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBossController : MonoBehaviour
{
    public int hp = 7;   //ヒットポイント
    public float reactionDistance = 7.0f;   //反応距離
    public float Speed = 0.1f;    //移動スピード

    public GameObject bulletPrefab;    //弾
    public float shootSpeed = 5.0f;    //弾の速度
    public float shootInterval = 1.5f; //攻撃間隔


    //攻撃中フラグ
    bool inAttack = false;
    float shootTimer = 0f;
   
    //ダメージ時色変更
    bool isBlink = false;
    float blinkTimer = 0f;

    //BGM
    public AudioClip midBossBGM; 
    public AudioClip normalBGM;  
    bool bgmChanged = false;

    GameObject player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }

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

        Vector3 pr = player.transform.position;
        float dist = Vector2.Distance(transform.position, pr);

        if (dist <= reactionDistance && !bgmChanged)
        {
            //範囲内に入った処理
            if (BGMManager.Instance != null)
            {
                BGMManager.Instance.ChangeBGM(midBossBGM);
            }
            bgmChanged = true;
        }
        else if (dist > reactionDistance && bgmChanged)
        {
            //範囲外の処理
            if (BGMManager.Instance != null)
            {
                BGMManager.Instance.ChangeBGM(normalBGM);
            }
            bgmChanged = false;
        }

        if (dist <= reactionDistance)
        {
            //プレイヤーについていく
            transform.position = Vector3.MoveTowards(transform.position, pr, Speed * Time.deltaTime);

            //攻撃アニメーション
            if (!inAttack)
            {
                inAttack = true;
            }

            //一定間隔で弾を撃つ
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
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
                GetComponent<Animator>().Play("Half Boss");
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
            hp--;

            //ダメージ時赤色
            isBlink = true;
            blinkTimer = 0.1f;

            if (hp <= 0)
            {
                //死亡
                //当たりを消す
                GetComponent<Collider2D>().enabled = false;
                //アニメーションを消す
                GetComponent<Animator>().Play("Hlaf Boss Dead");

                //１秒後に消す
                Destroy(gameObject, 1);
            }
        }
    }
    //攻撃
    void Attack()
    {
        //発射口オブジェクトを取得
        Transform tr = transform.Find("ice");
        GameObject ice = tr.gameObject;

        //弾を発射するベクトルを作る
        if (player != null)
        {
            float dx = player.transform.position.x - ice.transform.position.x;
            float dy = player.transform.position.y - ice.transform.position.y;
            //アークタンジェント２関数で角度（ラジアン）を求める
            float radCenter = Mathf.Atan2(dy, dx);

            //角のオフセット
            float angleOffset = 30f;

            float radOffset = angleOffset * Mathf.Deg2Rad;

            float[] launchRads = new float[]
            {
                radCenter - radOffset, //左
                radCenter,             //中央
                radCenter + radOffset  //右
            };

            foreach (float rad in launchRads)
            {
                //ラジアンを度に変換して返す
                float angle = rad * Mathf.Rad2Deg + 270;
                //Prefabから弾のゲームオブジェクトを作る（進行方向に回転）
                Quaternion r = Quaternion.Euler(0, 0, angle);
                GameObject bullet = Instantiate(bulletPrefab, ice.transform.position, r);
                float x = Mathf.Cos(rad);
                float y = Mathf.Sin(rad);
                Vector3 v = new Vector3(x, y) * shootSpeed;
                //発射
                Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
                rbody.AddForce(v, ForceMode2D.Impulse);
            }
        }
    }
}