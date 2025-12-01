using UnityEngine;

public class DamageItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SwordHit weapon = collision.GetComponent<SwordHit>();
            if (weapon != null)
            {
                weapon.AddDamage(2); // 1å≈íËÇ≈ëùÇ‚Ç∑
            }

            Destroy(gameObject);
        }
    }
}
