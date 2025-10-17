using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーに命中！ダメージ：" + damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}