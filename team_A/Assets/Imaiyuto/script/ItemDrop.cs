/*using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private void OnTriggerEnter(Collider2D collision)
    {
        //アイテムタグと一致
        if (collision.CompareTag("Item"))
        {
            Debug.Log("アイテムを拾った");
        }
        //バフアイテム
        BuffItem buff = collision.GetComponent<BuffItem>();
        if (buff != null)
        {
            buff.ApplyBuff(gameObject);//プレイヤーにバフ効果付与
        }
        //武器アイテム(swordItem)
        swordItem sword = collision.GetComponent<SwordItem>();
        if (sword != null)
        {
            sword.Equip(gameObject);//プレイヤーの剣変更
        }

        //アイテム削除
        Destroy(Collision.gameobject);
    }

}*/
