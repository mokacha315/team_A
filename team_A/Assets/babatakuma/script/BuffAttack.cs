using UnityEngine;

public class AttackBuffItem : MonoBehaviour
{
    public int increaseAmount = 1; //バフ上昇量

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerAttack playerAttack = collision.GetComponent<PlayerAttack>();
            if (playerAttack != null)
            {
                //playerAttack.AttackPower += increaseAmount;
                //Debug.Log("攻撃力アップ！ 現在の攻撃力: " + playerAttack.AttackPower);
            }

            Destroy(gameObject); //アイテム消滅
        }
    }
}
