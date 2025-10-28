using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float deleteTime = 3.0f;     //削除する時間指定

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime);   //削除設定
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);     //何かに接触したら消す
    }

    // 他のオブジェクトと当たったとき
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 剣に当たったら弾を消す
        if (collision.gameObject.CompareTag("sword"))
        {
            Debug.Log("弾が剣に当たって消えた！");
            Destroy(gameObject);
        }

        // プレイヤーに当たったらダメージ処理して弾を消す
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが弾に当たった！");
            Destroy(gameObject);
        }
    }
}
