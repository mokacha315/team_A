using UnityEngine;

public class DamageItem : MonoBehaviour
{
    public int buffValue = 2;

    /// <summary>
    /// 主人公が触れたら攻撃力を上げ、アイテムを消す処理
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            return;

        PlayerAttack pa = collision.GetComponentInParent<PlayerAttack>();

        if (pa != null)
        {
            pa.AddDamage(buffValue);
            Destroy(gameObject);
        }

    }
}
