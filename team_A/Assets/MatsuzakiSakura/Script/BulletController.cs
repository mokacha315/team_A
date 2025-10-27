using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float deleteTime = 3.0f;     //�폜���鎞�Ԏw��

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime);   //�폜�ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);     //�����ɐڐG���������
    }

    // ���̃I�u�W�F�N�g�Ɠ��������Ƃ�
    void OnTriggerEnter2D(Collider2D collision)
    {
        // ���ɓ���������e������
        if (collision.gameObject.CompareTag("sword"))
        {
            Debug.Log("�e�����ɓ������ď������I");
            Destroy(gameObject);
        }

        // �v���C���[�ɓ���������_���[�W�������Ēe������
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�v���C���[���e�ɓ��������I");
            Destroy(gameObject);
        }
    }
}
