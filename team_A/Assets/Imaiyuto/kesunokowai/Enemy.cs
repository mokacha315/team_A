/*using UnityEngine;
 
public class Enemy : MonoBehaviour

{

    public int hp = 3; // �G�̗̑́i�����l3�j
 
    void OnCollisionEnter2D(Collision2D collision)

    {

        if (collision.gameObject.tag == "sword")

        {

            //�_���[�W

            hp--;

            if (hp <= 0)

            {

                //���S

                //�����蔻�������

                GetComponent<CircleCollider2D>().enabled = false;

                //�ړ���~

               // rbody.velocity = Vector2.zero;

                //�A�j���[�V����
 
                //0.5�b��ɏ���

                Destroy(gameObject, 0.5f);

            }

        }
 
    }

}*/

using UnityEngine;
public class Enemy : MonoBehaviour
{
    public int hp = 3; // �G�̗̑́i�����l3�j
    private Rigidbody2D rbody;
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }
    // ��OnCollisionEnter2D��OnTriggerEnter2D�ɕύX
    void OnTriggerEnter2D(Collider2D other)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O�����S�ɔ�
        if (other.CompareTag("sword"))
        {
            // �_���[�W
            hp--;
            // ... (���̑��̏���)
            if (hp <= 0)
            {
                // ���S
                Collider2D col = GetComponent<Collider2D>();
                if (col != null)
                {
                    col.enabled = false;
                }
                // �ړ���~
                if (rbody != null)
                {
                    rbody.linearVelocity = Vector2.zero;
                    rbody.isKinematic = true;
                }
                // 0.5�b��ɏ���
                Destroy(gameObject, 0.5f);
            }

        }

    }

}

