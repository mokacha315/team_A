using UnityEngine;

public class DamageItem : MonoBehaviour
{
    public int buffValue = 2;
    bool used = false;

    /// <summary>
    /// 主人公が触れたら攻撃力を上げ、アイテムを消す処理
    /// </summary>
    /// <param name="collision">主人公(アイテムに触れた相手のCollider)</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (used)
        {
            return;
        }

        if (!collision.CompareTag("Player"))
        {
            return;
        }

        PlayerAttack pa = collision.GetComponentInParent<PlayerAttack>();

        if (pa != null)
        {
            used = true;   
            pa.AddDamage(buffValue);
            Destroy(gameObject);
        }

    }
}
