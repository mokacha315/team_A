using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //�q�b�g�|�C���g
    public int hp = 3;
    //�ړ��X�s�[�h
    public float speed = 0.5f;  //��������
    public float reactionDistance = 4.0f;
    float axisH;                //�����i-1.0 ? 0.0 ? 1.0�j
    float axisV;                //�c���i-1.0 ? 0.0 ? 1.0�j
    Rigidbody2D rbody;          //Rigidbody 2D
    Animator animator;          //Animator
    bool isActive = false;      //�A�N�e�B�u�t���O
    public int arrange = 0;     //�z�u�̎��ʂɎg��


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();      //Rigidbody2D�𓾂�
        animator = GetComponent<Animator>();      //Animator�𓾂�
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ��l������
        axisH = 0;
        axisV = 0;
        //Player�̃Q�[���I�u�W�F�N�g�𓾂�
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //�v���C���[�Ƃ̋����`�F�b�N
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < reactionDistance)
            {
                isActive = true;       //�A�N�e�B�u�ɂ���
            }
            else
            {
                isActive = false;      //��A�N�e�B�u�ɂ���
            }
            //�A�j���[�V������؂�ւ���
            animator.SetBool("IsActive", isActive);
            if (isActive)
            {
                animator.SetBool("IsActive", isActive);
                //�v���C���[�ւ̊p�x�����߂�
                float dx = player.transform.position.x - transform.position.x;
                float dy = player.transform.position.y - transform.position.y;
                float rad = Mathf.Atan2(dy, dx);
                float angle = rad * Mathf.Rad2Deg;
                //�ړ��p�x�ŃA�j���[�V������ύX����
                int direction;
                if (angle > -45.0f && angle <= 45.0f)
                {
                    direction = 3;    //�E����
                }
                else if (angle > 45.0f && angle <= 135.0f)
                {
                    direction = 2;    //�����
                }
                else if (angle >= 135.0f && angle <= -45.0f)
                {
                    direction = 0;    //������
                }
                else
                {
                    direction = 1;    //������
                }
                animator.SetInteger("Direction", direction);
                //�ړ�����x�N�g�������
                axisH = Mathf.Cos(rad) * speed;
                axisV = Mathf.Sin(rad) * speed;
            }
        }
        else
        {
            isActive = false;
        }
    }


    void FixedUpdate()
    {
        if (isActive && hp > 0)
        {
            //�ړ�
            rbody.linearVelocity = new Vector2(axisH, axisV).normalized;
        }
        else
        {
            rbody.linearVelocity = Vector2.zero;
        }
    }
}
