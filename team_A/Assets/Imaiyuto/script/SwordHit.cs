using UnityEngine;

public class SwordHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)//���̃I�u�W�F�N�g�����̓����蔻��ɐG�ꂽ�Ƃ��ɌĂ΂��
    {
        //�G�ꂽ���肪[Enemy]�^�O�������Ă��邩�`�F�b�N
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("�G�ɓ��������I");//�f�o�b�N�p

           // Enemy�X�N���v�g���擾���ă_���[�W�������Ă�
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)//Enemy�X�N���v�g�����Ă����
            {
                enemy.hp--;//�G�̗̑͂����炷
            }
            //EnemyAttack�X�N���v�g���擾���ēG�̋���ł�����
            EnemyAttack enemyattack=collision.GetComponent<EnemyAttack>();
            if(enemy!=null)
            {
              //  enemyattack.hp-=1000;
            }

        }
        }
    }





