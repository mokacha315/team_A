using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    //�q�b�g�|�C���g
    public int hp = 1;
    //��������
    public float reactionDistance = 7.0f;

    public GameObject bulletPrefab;    //�e
    public float shootSpeed = 5.0f;    //�e�̑��x

    //�U�����t���O
    bool inAttack = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            //Player�̃Q�[���I�u�W�F�N�g�𓾂�
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                //�v���C���[�Ƃ̋����`�F�b�N
                Vector3 plpos = player.transform.position;
                float dist = Vector2.Distance(transform.position, plpos);
                if (dist <= reactionDistance && inAttack == false)
                {
                    //�͈͓�&�U�����ł͂Ȃ�&HP�U��
                    inAttack = true;
                    //�A�j���[�V������؂�ւ���
                    GetComponent<Animator>().Play("BossAttack");
                }
                else if (dist > reactionDistance && inAttack)
                {
                    inAttack = false;
                    //�A�j���[�V������؂�ւ���
                    GetComponent<Animator>().Play("Boss");
                }
            }
            else
            {
                inAttack = false;
                //�A�j���[�V������؂�ւ���
                GetComponent<Animator>().Play("Boss");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "sword")
        {
            //�_���[�W
            hp--;
            if (hp <= 0)
            {
                //���S
                //�����������
                GetComponent<Collider2D>().enabled = false;
                //�A�j���[�V����������
                GetComponent<Animator>().Play("BossDead");
                //�P�b��ɏ���
                Destroy(gameObject, 1);
            }
        }
    }
    //�U��
    void Attack()
    {
        //���ˌ��I�u�W�F�N�g���擾
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;
        //�e�𔭎˂���x�N�g�������
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;
            //�A�[�N�^���W�F���g�Q�֐��Ŋp�x�i���W�A���j�����߂�
            float rad = Mathf.Atan2(dy, dx);
            //���W�A����x�ɕϊ����ĕԂ�
            float angle = rad * Mathf.Rad2Deg + 90f;
            //Prefab����e�̃Q�[���I�u�W�F�N�g�����i�i�s�����ɉ�]�j
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            //����
            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }
}
