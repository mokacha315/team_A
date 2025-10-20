using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("�U���p�����[�^")]
    public GameObject bulletPrefab;    // �e�̃v���n�u
    public Transform firePoint;        // �e�̔��ˈʒu
    public float attackGaugeMax = 5f;  // �U���Q�[�W�̍ő�l
    public float bulletSpeed = 8f;     // �e��
    public float attackRange = 10f;    // �U���\�͈�
    public float gaugeIncreaseRate = 1f; // �b������̃Q�[�W�㏸��

    private float attackGauge = 0f;
    private Transform target;

    void Start()
    {
        target = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null) return;

        // �����`�F�b�N
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange) return;

        // �U���Q�[�W�𗭂߂�
        attackGauge += Time.deltaTime * gaugeIncreaseRate;

        // �Q�[�W���ő�ɂȂ�����U��
        if (attackGauge >= attackGaugeMax)
        {
            Attack();
            attackGauge = 0f; // ���Z�b�g
        }
    }

    void Attack()
    {
        // �������v���C���[������
        Vector3 direction = (target.position - firePoint.position).normalized;

        // �e�𐶐�
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // �e�̃X�s�[�h�ݒ�
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        // ��������
        bullet.transform.right = direction;

        Debug.Log($"{gameObject.name} ���U���I");
    }
}
