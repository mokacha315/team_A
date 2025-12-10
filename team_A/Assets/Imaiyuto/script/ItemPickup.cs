using NUnit.Framework.Internal.Execution;
using UnityEngine;
using UnityEngine.Audio;

public class ItemPickup : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip ItemSE;//アイテムSE

    private ItemDisplayManager displayManager;//ItemDisplayManagerへの参照

    //start関数でManagerを取得
    void Start()
    {
        // シーン内に一つだけ存在するManagerを取得
        displayManager = FindObjectOfType<ItemDisplayManager>();

        if (displayManager == null)
        {
            Debug.LogError("ItemDisplayManagerが見つかりませんでした。シーンに配置されているか確認してください。");
        }
    }

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