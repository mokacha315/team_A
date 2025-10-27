using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject swordPrefab; // ���̃v���n�u
    public float attackDelay = 0.25f; // �U���Ԋu
    public float attackDuration = 0.2f; // �U������̎�������
    bool inAttack = false; // �U�������ǂ���
    GameObject sword; // ���I�u�W�F�N�g

    void Attack()
    {
        inAttack = true;
        sword.SetActive(true); // ���̓����蔻���L����

        // ��莞�Ԍ�ɍU���I��
        Invoke(nameof(StopAttack), attackDuration);

        Invoke(nameof(ResetAttackFlag), attackDelay);
    }

    void StopAttack()
    {
        sword.SetActive(false); // ���I�u�W�F�N�g�S�̂��\���i��A�N�e�B�u�j�ɂ���
        Debug.Log("�U���I���I");
        
    }

    void ResetAttackFlag()
    {
        inAttack = false; //�U���t���O����
    }

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
        swordCollider.enabled = true; // �����蔻���L����
        Debug.Log("�U���J�n�I");

        Invoke(nameof(StopAttack), attackDuration);
        Invoke(nameof(ResetAttackFlag), attackDelay);
    }

    void StopAttack()
    {
        swordCollider.enabled = false; // ���肾��������
        Debug.Log("�U���I���I");
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
        swordCollider.enabled = false; // ������OFF
        Debug.Log("�������������I");
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !inAttack)
        {
            Attack();
        }
    }
}*/
