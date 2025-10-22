using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("弾のパラメータ")]
    public float lifeTime = 3f;   // 存在時間
    public int damage = 10;       // プレイヤーに与えるダメージ
    public int bulletHP = 3;      // 弾の体力（例：3なら3回当たるまで消えない）

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーに命中
        if (other.CompareTag("Player"))
        {
            Debug.Log($"プレイヤーに命中！ ダメージ：{damage}");
            TakeDamage(1); // 1回分の耐久を消費
        }
        // 壁や他の障害物に当たった場合
        else if (other.CompareTag("Wall"))
        {
            Debug.Log("壁に命中！");
            TakeDamage(1);
        }
        // 他の弾や敵なども同様に処理したい場合はここに追加
    }

    void TakeDamage(int amount)
    {
        bulletHP -= amount;
        if (bulletHP <= 0)
        {
            Destroy(gameObject);
            Debug.Log("弾が破壊された");
        }
    }
}
