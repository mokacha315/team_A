using UnityEngine;

public class SwordHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)//他のオブジェクトが剣の当たり判定に触れたときに呼ばれる
    {
        //触れた相手が[Enemy]タグを持っているかチェック
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("敵に当たった！");//デバック用

           // Enemyスクリプトを取得してダメージ処理を呼ぶ
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)//Enemyスクリプトがついていれば
            {
                enemy.hp--;//敵の体力を減らす
            }
            //EnemyAttackスクリプトを取得して敵の球を打ち消す
            EnemyAttack enemyattack=collision.GetComponent<EnemyAttack>();
            if(enemy!=null)
            {
              //  enemyattack.hp-=1000;
            }

        }
        }
    }





