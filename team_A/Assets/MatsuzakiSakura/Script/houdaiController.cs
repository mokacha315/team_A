using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class houdaiController : MonoBehaviour
{
    public GameObject objPrefab;       //発生させるPrefabデータ
    public float delayTime = 3.0f;     //遅延時間
    public float firespeed = 4.0f;     //発射速度
    public float length = 16.0f;        //範囲

    GameObject player;                 //プレイヤー
    Transform WaterTransform;    //発射口のTransform
    float passedTimes = 0;             //経過時間

    //距離チェック
    bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos);
        if (length >= d)
        {
            ret = true;
        }
        return ret;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //発射口オブジェクトのtransformを取得
        WaterTransform = transform.Find("Water");
        //プレイヤーを取得
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        //待機時間加算
        passedTimes += Time.deltaTime;
        //Playerとの距離チェック
        if (CheckLength(player.transform.position))
        {
            //待機時間経過
            if (passedTimes > delayTime)
            {
                passedTimes = 0;       //時間を0にリセット
                //砲弾をプレハブから作る
                Vector2 pos = new Vector2(WaterTransform.position.x,
                                            WaterTransform.position.y);
                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);
                //砲身が向いている方向に発射する
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                float angleZ = transform.localEulerAngles.z;
                float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
                float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
                Vector2 v = Vector2.left * firespeed;
                rbody.AddForce(v, ForceMode2D.Impulse);
            }
        }
    }
    //範囲表示
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, length);
    }
}
