using Unity.VisualScripting;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public float AttackDelay = 0.25f;//�U���Ԋu
    public GameObject swordPrefab;    //���̃v���n�u
    public float attackDelay = 0.2f;  // �U������̎�������
    bool inAttack = false;//�U�������ǂ������L�^
    GameObject sword;       // ���I�u�W�F�N�g

    //�U��
    public void Attack()
    {
        //�U�����ł͂Ȃ�
        if (inAttack == false)
        {
            inAttack = true;//�U���t���O�𗧂Ă�

            HeroController playerCnt = GetComponent<HeroController>();  //���ōU������
            //�U���t���O�����낷�x�����s
            Invoke("StopAttack", AttackDelay);
        }
    }
    //�U�����~
    public void StopAttack()
    {
        inAttack = false;
    }
    //start is called before the first frame update
    void Start()
    {
        //�����v���C���[�L�����N�^�[�ɔz�u
        Vector3 pos = transform.position;
        sword = Instantiate(swordPrefab, pos, Quaternion.identity);
        sword.transform.SetParent(transform);//���̐e�Ƀv���C���[�L�����N�^�[��ݒ�
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire3"))
        {
            //�U���L�[�������ꂽ
            Attack();
        }
    }
}
