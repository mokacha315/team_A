using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 3; //回復量
    bool used = false;

    /// <summary>
    /// 主人公が触れたらHPを回復し、アイテムを消す処理
    /// </summary>
    /// <param name="collision">主人公(アイテムに触れた相手のCollider)</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (used)
        {
            return;
        }

        HeroController hero = collision.GetComponentInParent<HeroController>();

        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (hero == null)
        {
            return;
        }

        used = true;
        HeroController.hp = Mathf.Min(HeroController.hp + healAmount, 10);
        Destroy(gameObject);
    }
}
