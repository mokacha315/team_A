using UnityEngine;

public class SpeedItem : MonoBehaviour
{
    public float SpeedAmount = 2.0f; //上昇量
    bool used = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (used)
        {
            return;
        }

        HeroController hero = collision.GetComponentInParent<HeroController>();

        if (hero == null)
        {
            return;
        }

        used = true;
        hero.AddSpeed(SpeedAmount); //速度アップ
        Destroy(gameObject); //アイテム消す
    }
}
