using NUnit.Framework.Internal.Execution;
using UnityEngine;
using UnityEngine.Audio;

public class ItemPickup : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip ItemSE;//アイテムSE


    //start関数でManagerを取得
    void Start()
    {

    }
    /// <summary>
    /// アイテムを拾った時の反映とSEなど
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        // アイテムタグと一致
        if (collision.CompareTag("Item"))
        {
            Debug.Log("アイテムを拾った: " + collision.name);
            if (audioSource != null && ItemSE != null)
            {
                audioSource.PlayOneShot(ItemSE);
            }
            //バフアイテム (攻撃力アップなど)
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
            //アイテム削除
            Destroy(collision.gameObject);

        }

    }
}