using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public GameObject sword;       // ���I�u�W�F�N�g
    public float attackDelay = 0.2f;  // �U������̎�������
    private bool isAttack = false;//�U�������ǂ������L�^

    public void Attack()
    {
        //�U�����ł͂Ȃ�
        if (isAttack == false)
        {
            isAttack = true;//�U���t���O
                            //���ōU������

        }
    }

}

//�U��
