using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3; // �G�̗̑́i�����l3�j
}

void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.tag == "Arrow")
    { 
    //�_���[�W
    hp--;
    if (hp <= 0) 
    {
        //���S
        //�����蔻�������
        GetComponent<CircleCollider2D>().enabled = false;
        //�ړ���~
        rbody.velocity = Vector2.zero;
        //�A�j���[�V����

        //0.5�b��ɏ���
        Destroy(gameObject, 0.5f);
    }
}
}