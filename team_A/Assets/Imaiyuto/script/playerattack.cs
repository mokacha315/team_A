using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject swordPrefab; // ���̃v���n�u
    public float attackDelay = 0.25f; // �U���Ԋu
    public float attackDuration = 0.2f; // �U������̎�������
    private bool inAttack = false; // �U�������ǂ���
    private GameObject sword; // ���I�u�W�F�N�g

    void Start()
    {
        // �����v���C���[�̎q�I�u�W�F�N�g�Ƃ��Đ���
        sword = Instantiate(swordPrefab, transform);
        sword.transform.localPosition = Vector3.zero; // �v���C���[�̈ʒu�ɍ��킹��
        sword.SetActive(false); // ������Ԃ͔�\��
        Debug.Log("���̏����������B���͔�A�N�e�B�u�ł�: " + !sword.activeSelf);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !inAttack)//�X�y�[�X�L�[�ōU��
        {
            Attack();
        }
    }
    void Attack()
    {
        inAttack = true;
        sword.SetActive(true); // ���̓����蔻���L����

        // ��莞�Ԍ�ɍU���I��
        Invoke(nameof(StopAttack), attackDuration);
    }

    void StopAttack()
    {
        sword.SetActive(false); // ���I�u�W�F�N�g�S�̂��\���i��A�N�e�B�u�j�ɂ���
        inAttack = false; // �U���t���O����
        Debug.Log("�U���I���I");
        
    }
}