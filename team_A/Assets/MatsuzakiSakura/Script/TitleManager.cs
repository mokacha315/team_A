using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject startButton;      //スタートボタン
    public string firstSceneName;       //ゲーム開始シーン名

    //スタートボタン
    public static bool startPressed = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //スタートボタン押し
    public void StartButtonClicked()
    {
        startPressed = true;
        FadeManager.Instance.LoadScene(firstSceneName, 1.0f);
    }
}