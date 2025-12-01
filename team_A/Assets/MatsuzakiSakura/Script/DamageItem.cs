using UnityEngine;

public class DamageItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            var player = collision.GetComponent<Player>();
            if (player != null && player.weapon != null)
            {
                player.weapon.AddDamage(2); // 1å≈íËÇ≈ëùÇ‚Ç∑
            }

            Destroy(gameObject);
        }
    }
}
