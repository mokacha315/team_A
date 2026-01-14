using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 3; //‰ñ•œ—Ê

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (HeroController.hp > 0)
            {
                HeroController.hp = Mathf.Min(HeroController.hp + healAmount, 10);
            }

            //HP§ŒÀ
            if (HeroController.hp > 10)
            {
                HeroController.hp = 10;
            }

            Destroy(gameObject); //ƒAƒCƒeƒ€Á‚·
        }
    }
}
