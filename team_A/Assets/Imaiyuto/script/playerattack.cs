using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject sword_effectPrefab; // ���̃v���n�u
    public float attackInterval = 0.1f; // �U���Ԋu
    public float attackDuration = 0.2f; // �U������̎�������
    bool inAttack = false; // �U�������ǂ���
    GameObject sword_effect; // ���I�u�W�F�N�g

    private float timer = 0.0f;

    void Attack()
    {
        inAttack = true;
        sword_effect.SetActive(true); // ���̓����蔻���L����

        // ��莞�Ԍ�ɍU���I��
        Invoke(nameof(StopAttack), attackDuration);

        Invoke(nameof(ResetAttackFlag), attackInterval);
    }

    void StopAttack()
    {
        sword_effect.SetActive(false); // ���I�u�W�F�N�g�S�̂��\���i��A�N�e�B�u�j�ɂ���
        Debug.Log("�U���I���I");
        
    }

    void ResetAttackFlag()
    {
        inAttack = false; //�U���t���O����
    }

    void Start()
    {
        // �����v���C���[�̎q�I�u�W�F�N�g�Ƃ��Đ���
        sword_effect = Instantiate(sword_effectPrefab, transform);
        sword_effect.transform.localPosition = Vector3.zero; // �v���C���[�̈ʒu�ɍ��킹��
        sword_effect.SetActive(false); // ������Ԃ͔�\��
        Debug.Log("���̏����������B���͔�A�N�e�B�u�ł�: " + !sword_effect.activeSelf);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackInterval)
        {
            if (Input.GetButtonDown("Jump") && !inAttack)//�X�y�[�X�L�[�ōU��
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
        sword_effectCollider.enabled = true; // �����蔻���L����
        Debug.Log("�U���J�n�I");

        Invoke(nameof(StopAttack), attackDuration);
        Invoke(nameof(ResetAttackFlag), attackDelay);
    }

    void StopAttack()
    {
        sword_effectCollider.enabled = false; // ���肾��������
        Debug.Log("�U���I���I");
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
        sword_effectCollider.enabled = false; // ������OFF
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
