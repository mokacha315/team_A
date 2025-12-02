using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    //ヒットポイント
    public int hp = 15;
    //反応距離
    public float reactionDistance = 7.0f;

    public GameObject bulletPrefab;    //弾
    public float shootSpeed = 5.0f;    //弾の速度

    public float shootInterval = 1.0f; //攻撃間隔
    float shootTimer = 0f;

    //攻撃中フラグ
    bool inAttack = false;

    //ダメージ時色変更
    bool isBlink = false;
    float blinkTimer = 0f;

    //BGM
    public AudioClip Boss;
    public AudioClip normalBGM;
    bool bgmChanged = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D bossCollider;
    private Animator animator;

    GameObject player;

    PlayerAttack playerAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        spriteRenderer = GetComponent<SpriteRenderer>();
        bossCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if (player != null)
        {
            playerAttack = player.GetComponent<PlayerAttack>();
        }
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

        if (hp > 0)
        {
            //プレイヤーとの距離チェック
            Vector3 plpos = player.transform.position;
            float dist = Vector2.Distance(transform.position, plpos);

            //BGM範囲内
            if (dist <= reactionDistance && !bgmChanged)
            {
                if (BGMManager.Instance != null)
                {
                    BGMManager.Instance.ChangeBGM(Boss);
                }
                bgmChanged = true;
            }
            //BGM範囲外
            else if (dist > reactionDistance && bgmChanged)
            {
                if (BGMManager.Instance != null)
                {
                    BGMManager.Instance.ChangeBGM(normalBGM);
                }
                bgmChanged = false;
            }


            if (dist <= reactionDistance)
            {
                //範囲内&攻撃中ではない&HP攻撃
                if (inAttack == false)
                {
                    inAttack = true;
                }

                //攻撃中はタイマー止める
                if (inAttack)
                {
                    shootTimer += Time.deltaTime;
                    if (shootTimer >= shootInterval)
                    {
                        Attack();
                        shootTimer = 0f;
                    }
                }
            }
            else
            {
                if (inAttack)
                {
                    inAttack = false;
                    //タイマーリセット
                    shootTimer = 0f;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "sword" || collision.gameObject.tag == "master_sowd_effect" || collision.gameObject.tag == "kenn_effect")
        {
            SwordHit swordhit = collision.gameObject.GetComponent<SwordHit>();
            if (swordhit == null) return;

            PlayerAttack playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();

            int extra = playerAttack.extraDamage;
            int totalDamage = swordhit.damage + extra;
            //ボスのHPを減らす
            hp -= totalDamage;

            //ダメージ時赤色
            isBlink = true;
            blinkTimer = 0.1f;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1f, 0.4f, 0.4f);
            }

            if (hp <= 0)
            {
                //死亡
                //当たりを消す
                GetComponent<Collider2D>().enabled = false;
                //アニメーションを消す
                GetComponent<Animator>().Play("BossDead");

                FindObjectOfType<UIManager>().GameClear();


                //１秒後に消す
                Destroy(gameObject, 1);
            }
        }
    }
    //攻撃
    void Attack()
    {
        if (player == null)
        {
            return;
        }

        //発射口オブジェクトを取得
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;
        //弾を発射するベクトルを作る
        if (player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;
            //アークタンジェント２関数で角度（ラジアン）を求める
            float rad = Mathf.Atan2(dy, dx);
            //ラジアンを度に変換して返す
            float angle = rad * Mathf.Rad2Deg + 90f;
            //Prefabから弾のゲームオブジェクトを作る（進行方向に回転）
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            //発射
            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }
}
