using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("�e�̃p�����[�^")]
    public float lifeTime = 3f;   // ���ݎ���
    public int damage = 10;       // �v���C���[�ɗ^����_���[�W
    public int bulletHP = 3;      // �e�̗̑́i��F3�Ȃ�3�񓖂���܂ŏ����Ȃ��j

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �v���C���[�ɖ���
        if (other.CompareTag("Player"))
        {
            Debug.Log($"�v���C���[�ɖ����I �_���[�W�F{damage}");
            TakeDamage(1); // 1�񕪂̑ϋv������
        }
        // �ǂ⑼�̏�Q���ɓ��������ꍇ
        else if (other.CompareTag("Wall"))
        {
            Debug.Log("�ǂɖ����I");
            TakeDamage(1);
        }
        // ���̒e��G�Ȃǂ����l�ɏ����������ꍇ�͂����ɒǉ�
    }

    void TakeDamage(int amount)
    {
        bulletHP -= amount;
        if (bulletHP <= 0)
        {
            Destroy(gameObject);
            Debug.Log("�e���j�󂳂ꂽ");
        }
    }
}
