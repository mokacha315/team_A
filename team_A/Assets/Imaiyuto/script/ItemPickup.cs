using NUnit.Framework.Internal.Execution;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // アイテムタグと一致
        if (collision.CompareTag("Item"))
        {
            Debug.Log("アイテムを拾った: " + collision.name);

            // バフアイテム (攻撃力アップなど)
            BuffItem buff = collision.GetComponent<BuffItem>();
            if (buff != null)
            {
                buff.ApplyBuff(gameObject);  // プレイヤーにバフを適用
            }

            // 武器アイテム（SwordItem）
            SwordItem sword = collision.GetComponent<SwordItem>();
            if (sword != null)
            {
                sword.Equip(gameObject);     // プレイヤーに装備させる
            }

            // アイテム削除
            Destroy(collision.gameObject);
        }
    }
}