using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject swordPrefab;         // 剣のプレハブ
    public float attackDuration = 0.2f;    // 攻撃判定の持続時間
    public Vector2 swordOffset = new Vector2(1.0f, 0f); // 前方に出す位置（右方向）

    private bool inAttack = false;
    private GameObject sword;
    private Transform swordTransform;

    void Start()
    {
        // 剣をプレイヤーの子オブジェクトとして生成
        sword = Instantiate(swordPrefab, transform);
        swordTransform = sword.transform;

        swordTransform.localPosition = Vector3.zero; // 最初は自分の位置に
        sword.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inAttack)
        {
            Attack();
        }
    }

    void Attack()
    {
        inAttack = true;
        sword.SetActive(true);

        // プレイヤーの向きに合わせて剣の位置を設定
        Vector3 offset = swordOffset;
        if (transform.localScale.x < 0) // 左を向いてるとき
            offset.x *= -1;

        // ローカル座標（プレイヤーを親にしてるので相対位置）
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
