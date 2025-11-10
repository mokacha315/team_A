using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PlayerAttack : MonoBehaviour
{
    public GameObject swordPrefab;// 剣のプレハブ
    public GameObject sword_effectPrefab;//剣のエフェクトのプレハブ
    public float attackDuration = 0.2f;    // 攻撃判定の持続時間
    public Vector2 swordOffset = new Vector2(1.0f, 0f); // 前方に出す位置（右方向）
    public HeroController heroController;//HeroController参照取得

    private bool inAttack = false;
    private GameObject sword;
    private GameObject sword_effect;
    private Transform swordTransform;


    void Start()
    {
        //HeroControllerへの参照取得
        if(heroController == null)
        {
            heroController=GetComponent<HeroController>();
        }
        // 剣と剣のエフェクトをプレイヤーの子オブジェクトとして生成
        sword = Instantiate(swordPrefab, transform);
        sword_effect = Instantiate(sword_effectPrefab, transform);
        swordTransform = sword.transform;

        swordTransform.localPosition = Vector3.zero; //最初は自分の位置に
        sword.SetActive(false);
        sword_effect.SetActive(false);//エフェクトも初期は非表示に
    }
          void Update()
            {
                if (Input.GetKeyDown(KeyCode.Space) && !inAttack)
                {
                    Attack();
                }
        if (inAttack && heroController != null)
        {
            inAttack = false;
        }
                //剣と斬撃の回転と優先順位
                float swordz = -1;//(キャラクターの前に出す)
                HeroController plmv = GetComponent<HeroController>();
                if (plmv.angleZ > 30 && plmv.angleZ < 150)
                {
                    //上向き
                    swordz = 1;
                }
                //剣エフェクトを回転させる
                sword.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);
            //剣エフェクトの位置(Z軸だけ調整)
            sword.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            swordz
        );
            }
            void Attack()
            {
                inAttack = true;
                sword.SetActive(true);

                //プレイヤーの向きに合わせて剣の位置を設定
                Vector3 offset = swordOffset;
                if (transform.localScale.x > 0) //左を向いてるとき
                    offset.x *= -1;
                else if (transform.localScale.x < 0)
                {
                    offset.x *= 5;
                }
        else if(transform.localScale.y > 0)
        {
            offset.y *= -1;
        }
        else if (transform.localScale.y < 0)
        {
            offset.y *= 1;
        }
        //ローカル座標（プレイヤーを親にしてるので相対位置）
        swordTransform.localPosition = offset;

                Debug.Log($"攻撃開始！ 剣の位置: {swordTransform.localPosition}");
                Invoke(nameof(StopAttack), attackDuration);
            }

            void StopAttack()
            {
                sword.SetActive(false);
                inAttack = false;
                Debug.Log("攻撃終了！");
            }
        }
    
