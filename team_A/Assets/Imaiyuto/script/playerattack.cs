using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject sword_effectPrefab; // 剣のプレハブ
    public float attackInterval = 0.1f; // 攻撃間隔
    public float attackDuration = 0.2f; // 攻撃判定の持続時間
    bool inAttack = false; // 攻撃中かどうか
    GameObject sword_effect; // 剣オブジェクト

    private float timer = 0.0f;

    void Attack()
    {
        inAttack = true;
        sword_effect.SetActive(true); // 剣の当たり判定を有効化

        // 一定時間後に攻撃終了
        Invoke(nameof(StopAttack), attackDuration);

        Invoke(nameof(ResetAttackFlag), attackInterval);
    }

    void StopAttack()
    {
        sword_effect.SetActive(false); // 剣オブジェクト全体を非表示（非アクティブ）にする
        Debug.Log("攻撃終了！");
        
    }

    void ResetAttackFlag()
    {
        inAttack = false; //攻撃フラグ解除
    }

    void Start()
    {
        // 剣をプレイヤーの子オブジェクトとして生成
        sword_effect = Instantiate(sword_effectPrefab, transform);
        sword_effect.transform.localPosition = Vector3.zero; // プレイヤーの位置に合わせる
        sword_effect.SetActive(false); // 初期状態は非表示
        Debug.Log("剣の初期化完了。剣は非アクティブです: " + !sword_effect.activeSelf);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackInterval)
        {
            if (Input.GetButtonDown("Jump") && !inAttack)//スペースキーで攻撃
            {
                Attack();
            }
        }
    }
}
/*using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject sword_effectPrefab;
    public float attackDelay = 0.25f;
    public float attackDuration = 0.2f;
    bool inAttack = false;
    GameObject sword_effect;
    Collider2D sword_effectCollider;

    void Attack()
    {
        inAttack = true;
        sword_effectCollider.enabled = true; // 当たり判定を有効化
        Debug.Log("攻撃開始！");

        Invoke(nameof(StopAttack), attackDuration);
        Invoke(nameof(ResetAttackFlag), attackDelay);
    }

    void StopAttack()
    {
        sword_effectCollider.enabled = false; // 判定だけ無効化
        Debug.Log("攻撃終了！");
    }

    void ResetAttackFlag()
    {
        inAttack = false;
    }

    void Start()
    {
        sword_effect = Instantiate(sword_effectPrefab, transform);
        sword_effect.transform.localPosition = Vector3.zero;

        sword_effectCollider = sword_effect.GetComponent<Collider2D>();
        sword_effectCollider.enabled = false; // 初期はOFF
        Debug.Log("剣初期化完了！");
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !inAttack)
        {
            Attack();
        }
    }
}*/
