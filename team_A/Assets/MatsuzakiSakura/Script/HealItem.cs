using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 3; //‰ñ•œ—Ê

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collision.CompareTag("Player")) return;

            HeroController.hp = Mathf.Min(HeroController.hp + healAmount, 10);
            Destroy(gameObject);
        }
    }
}
