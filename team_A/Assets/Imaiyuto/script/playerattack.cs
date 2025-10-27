using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject swordPrefab; // 剣のプレハブ
    public float attackDelay = 0.25f; // 攻撃間隔
    public float attackDuration = 0.2f; // 攻撃判定の持続時間
    bool inAttack = false; // 攻撃中かどうか
    GameObject sword; // 剣オブジェクト

    void Attack()
    {
        inAttack = true;
        sword.SetActive(true); // 剣の当たり判定を有効化

        // 一定時間後に攻撃終了
        Invoke(nameof(StopAttack), attackDuration);

        Invoke(nameof(ResetAttackFlag), attackDelay);
    }

    void StopAttack()
    {
        sword.SetActive(false); // 剣オブジェクト全体を非表示（非アクティブ）にする
        Debug.Log("攻撃終了！");
        
    }

    void ResetAttackFlag()
    {
        inAttack = false; //攻撃フラグ解除
    }

    void Start()
    {
        // 剣をプレイヤーの子オブジェクトとして生成
        sword = Instantiate(swordPrefab, transform);
        sword.transform.localPosition = Vector3.zero; // プレイヤーの位置に合わせる
        sword.SetActive(false); // 初期状態は非表示
        Debug.Log("剣の初期化完了。剣は非アクティブです: " + !sword.activeSelf);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !inAttack)//スペースキーで攻撃
        {
            Attack();
        }
    }
}
/*using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject swordPrefab;
    public float attackDelay = 0.25f;
    public float attackDuration = 0.2f;
    bool inAttack = false;
    GameObject sword;
    Collider2D swordCollider;

    void Attack()
    {
        inAttack = true;
        swordCollider.enabled = true; // 当たり判定を有効化
        Debug.Log("攻撃開始！");

        Invoke(nameof(StopAttack), attackDuration);
        Invoke(nameof(ResetAttackFlag), attackDelay);
    }

    void StopAttack()
    {
        swordCollider.enabled = false; // 判定だけ無効化
        Debug.Log("攻撃終了！");
    }

    void ResetAttackFlag()
    {
        inAttack = false;
    }

    void Start()
    {
        sword = Instantiate(swordPrefab, transform);
        sword.transform.localPosition = Vector3.zero;

        swordCollider = sword.GetComponent<Collider2D>();
        swordCollider.enabled = false; // 初期はOFF
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
