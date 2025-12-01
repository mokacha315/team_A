using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public float SpeedAmount = 2.0f; //上昇量

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HeroController hero = collision.GetComponent<HeroController>();

            if (hero != null)
            {
                hero.AddSpeed(SpeedAmount); //速度アップ
            }

            Destroy(gameObject); //アイテム消す
        }
    }
}
