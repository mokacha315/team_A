using UnityEngine;

public class DamageItem : MonoBehaviour
{
    public int buffValue = 2;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerAttack pa = collision.GetComponent<PlayerAttack>();



            if (pa != null)
            {
                pa.AddDamage(buffValue);
            }
            Destroy(gameObject);
        }
    }
}
