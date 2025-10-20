using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3; // 敵の体力（初期値3）
}

void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.tag == "Arrow")
    { 
    //ダメージ
    hp--;
    if (hp <= 0) 
    {
        //死亡
        //当たり判定を消す
        GetComponent<CircleCollider2D>().enabled = false;
        //移動停止
        rbody.velocity = Vector2.zero;
        //アニメーション

        //0.5秒後に消す
        Destroy(gameObject, 0.5f);
    }
}
}