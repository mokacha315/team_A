using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");  //プレイヤーを探す
        if (player != null)
        {
            //カメラの更新座標
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;


            transform.position = new Vector3(x, y, z);
        }
    }
}
