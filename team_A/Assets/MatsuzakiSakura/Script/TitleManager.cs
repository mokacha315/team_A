using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject startButton;      //スタートボタン
    public string firstSceneName;       //ゲーム開始シーン名

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
        SceneManager.LoadScene(firstSceneName);
    }
}