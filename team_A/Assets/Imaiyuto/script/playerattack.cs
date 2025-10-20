using Unity.VisualScripting;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public float AttackDelay = 0.25f;//攻撃間隔
    public GameObject swordPrefab;    //剣のプレハブ
    public float attackDelay = 0.2f;  // 攻撃判定の持続時間
    bool inAttack = false;//攻撃中かどうかを記録
    GameObject sword;       // 剣オブジェクト

    //攻撃
    public void Attack()
    {
        //攻撃中ではない
        if (inAttack == false)
        {
            inAttack = true;//攻撃フラグを立てる

            HeroController playerCnt = GetComponent<HeroController>();  //剣で攻撃する
            //攻撃フラグを下ろす遅延実行
            Invoke("StopAttack", AttackDelay);
        }
    }
    //攻撃中止
    public void StopAttack()
    {
        inAttack = false;
    }
    //start is called before the first frame update
    void Start()
    {
        //剣をプレイヤーキャラクターに配置
        Vector3 pos = transform.position;
        sword = Instantiate(swordPrefab, pos, Quaternion.identity);
        sword.transform.SetParent(transform);//剣の親にプレイヤーキャラクターを設定
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire3"))
        {
            //攻撃キーが押された
            Attack();
        }
    }
}
