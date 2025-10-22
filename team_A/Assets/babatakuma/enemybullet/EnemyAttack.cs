using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("攻撃パラメータ")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackGaugeMax = 4f;
    public float bulletSpeed = 4f;
    public float attackRange = 10f;
    public float gaugeIncreaseRate = 1f;

    private float attackGauge = 0f;
    private Transform target;

    void Start()
    {
        target = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange) return;

        attackGauge += Time.deltaTime * gaugeIncreaseRate;

        if (attackGauge >= attackGaugeMax)
        {
            Attack();
            attackGauge = 0f;
        }
    }

    void Attack()
    {
        if (firePoint == null)
        {
            Debug.LogWarning($"{gameObject.name} の firePoint が設定されていません！");
            return;
        }

        Vector3 direction = (target.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        bullet.transform.right = direction;

        Debug.Log($"{gameObject.name} が攻撃！ 発射位置：{firePoint.position}");
    }
}
