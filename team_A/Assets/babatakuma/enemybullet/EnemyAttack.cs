using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("攻撃パラメータ")]
    public GameObject bulletPrefab;    // 弾のプレハブ
    public Transform firePoint;        // 弾の発射位置
    public float attackGaugeMax = 4f;  // 攻撃ゲージの最大値
    public float bulletSpeed = 4f;     // 弾速
    public float attackRange = 10f;    // 攻撃可能範囲
    public float gaugeIncreaseRate = 1f; // 秒あたりのゲージ上昇量

    private float attackGauge = 0f;
    private Transform target;

    void Start()
    {
        target = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null) return;

        // 距離チェック
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange) return;

        // 攻撃ゲージを溜める
        attackGauge += Time.deltaTime * gaugeIncreaseRate;

        // ゲージが最大になったら攻撃
        if (attackGauge >= attackGaugeMax)
        {
            Attack();
            attackGauge = 0f; // リセット
        }
    }

    void Attack()
    {
        // 向きをプレイヤー方向に
        Vector3 direction = (target.position - firePoint.position).normalized;

        // 弾を生成
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 弾のスピード設定
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        // 向き調整
        bullet.transform.right = direction;

        Debug.Log($"{gameObject.name} が攻撃！");
    }
}