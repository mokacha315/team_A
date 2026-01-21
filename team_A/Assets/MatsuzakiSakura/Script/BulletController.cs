using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float deleteTime = 3.0f;     //削除する時間指定

    /// <summary>
    /// deleteTimeに設定された時間がたつと弾が消える
    /// </summary>
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime);   //削除設定
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// オブジェクトに当たると弾を消す
    /// </summary>
    /// <param name="collision">ぶつかったオブジェクト</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
       Destroy(gameObject);     //何かに接触したら消す
    }

    /// <summary>
    /// 剣に当たると弾を消す処理と、主人公に当たったらダメージ
    /// </summary>
    /// <param name="collision">ぶつかったオブジェクト</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "sword" || collision.gameObject.tag == "buster_sword_effect" || collision.gameObject.tag == "kenn_effect")
        {
            Debug.Log("弾が剣に当たって消えた！");
            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが弾に当たった！");
            Destroy(gameObject);
        }
    }
}
