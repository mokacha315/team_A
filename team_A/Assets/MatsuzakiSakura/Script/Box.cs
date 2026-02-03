using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    //ヒットポイント
    public int hp = 1;
    //移動スピード
    public float speed = 1.0f;
    //反応距離
    public float reactionDistance = 8.0f;
    float axisH;                //横軸値（-1.0 ∼ 0.0 ∼ 1.0）
    float axisV;                //縦軸値（-1.0 ∼ 0.0 ∼ 1.0）
    Rigidbody2D rbody;          //Rigidbody 2D
    public int arrange = 0;     //配置の識別に使う

    //アイテムドロップ
    public GameObject[] dropItems;   //ドロップするアイテムリスト
    public float dropRate = 1.0f;    //ドロップ確率100％

    GameObject player;
    PlayerAttack playerAttack;


    /// <summary>
    /// 必要なオブジェクトをコンポーネントして、プレイヤーの情報がわかるようにする
    /// </summary>
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerAttack = player.GetComponent<PlayerAttack>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 主人公の剣(エフェクト)に当たるとHPを減らす。
    /// HPが0になるとアニメーション変更、アイテムドロップ、0.5秒後に消す
    /// </summary>
    /// <param name="collision">剣(エフェクト)のオブジェクト</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "sword" || collision.gameObject.tag == "buster_sword_effect" || collision.gameObject.tag == "kenn_effect")
        {
            //ダメージ
            SwordHit swordhit = collision.gameObject.GetComponent<SwordHit>();
            if (swordhit == null) return;

            int extra = playerAttack.extraDamage;
            int totalDamage = swordhit.damage + extra;
            //HPを減らす
            hp -= totalDamage;

            if (hp <= 0)
            {
                Debug.Log(name + " should play death animation now.");
                GetComponent<Collider2D>().enabled = false;

                //死亡
                //当たりを消す
                GetComponent<Collider2D>().enabled = false;

                //アイテムドロップ
                TryDropItem();

                //0.5秒後に消す
                Destroy(gameObject, 0.5f);
            }
        }
    }

    /// <summary>
    /// アイテムをドロップさせる処理
    /// </summary>
    void TryDropItem()
    {
        //設定していなければそのまま
        if (dropItems.Length == 0) return;

        float r = Random.value;  //0～1の乱数
        if (r <= dropRate)
        {
            //ランダムでアイテムを選択
            GameObject drop =
                dropItems[Random.Range(0, dropItems.Length)];

            //敵の位置にドロップ
            Instantiate(drop, transform.position,
                Quaternion.identity);
        }
    }
}
