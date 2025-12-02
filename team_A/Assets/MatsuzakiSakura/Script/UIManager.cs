using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int hp = 0;                      //プレイヤーのHP
    public GameObject lifeImage;     //HPの数を表示するImage
    public Sprite life100Image;      //HP100画像
    public Sprite life90Image;       //HP90画像
    public Sprite life80Image;       //HP80画像
    public Sprite life70Image;       //HP70画像
    public Sprite life60Image;       //HP60画像
    public Sprite life50Image;       //HP50画像
    public Sprite life40Image;       //HP40画像
    public Sprite life30Image;       //HP30画像
    public Sprite life20Image;       //HP20画像
    public Sprite life10Image;       //HP10画像
    public Sprite life0Image;        //HP0画像
    public GameObject mainImage;     //画像を持つGameObject
    public GameObject retryButton;   //リトライボタン
    public Sprite gameOverSpr;       //GAME OVER画像
    public Sprite gameClearSpr;      //GAME CLEAR画像
    public string retrySceneName = "";  //リトライするシーン名

    //BGM
    public AudioClip gameOverBGM;
    public AudioClip gameClearBGM;

    //HP更新
    void UpdateHP()
    {
        //Player取得
        if (HeroController.gameState != "gameend")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (HeroController.hp != hp)
                {
                    hp = HeroController.hp;
                    if (hp <= 0)
                    {
                        lifeImage.GetComponent<Image>().sprite = life0Image;

                        Invoke("GameOver", 2.0f);

                        HeroController.gameState = "gameend";    //ゲーム終了
                    }
                    else if (hp == 1)
                    {
                        lifeImage.GetComponent<Image>().sprite = life10Image;
                    }
                    else if (hp == 2)
                    {
                        lifeImage.GetComponent<Image>().sprite = life20Image;
                    }
                    else if (hp == 3)
                    {
                        lifeImage.GetComponent<Image>().sprite = life30Image;
                    }
                    else if (hp == 4)
                    {
                        lifeImage.GetComponent<Image>().sprite = life40Image;
                    }
                    else if (hp == 5)
                    {
                        lifeImage.GetComponent<Image>().sprite = life50Image;
                    }
                    else if (hp == 6)
                    {
                        lifeImage.GetComponent<Image>().sprite = life60Image;
                    }
                    else if (hp == 7)
                    {
                        lifeImage.GetComponent<Image>().sprite = life70Image;
                    }
                    else if (hp == 8)
                    {
                        lifeImage.GetComponent<Image>().sprite = life80Image;
                    }
                    else if (hp == 9)
                    {
                        lifeImage.GetComponent<Image>().sprite = life90Image;
                    }
                    else if (hp == 10)
                    {
                        lifeImage.GetComponent<Image>().sprite = life100Image;
                    }
                }
            }
        }
    }

    //リトライ
    public void Retry()
    {
        //HPを戻す
        HeroController.hp = 10;
        //ゲーム中に戻す
        SceneManager.LoadScene(retrySceneName);         //シーン移動
    }

    //画像を非表示にする
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = HeroController.hp;
        UpdateHP();         //HP更新
        //画像を非表示にする
        Invoke("InactiveImage", 1.0f);
        retryButton.SetActive(false);    //ボタン非表示
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHP();
    }

    //ゲームクリア
    public void GameClear()
    {
        //画像表示
        mainImage.SetActive(true);
        mainImage.GetComponent<Image>().sprite = gameClearSpr;  //「GAME CLEAR」を設定する

        RectTransform rt = mainImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(800, 800);

        //ゲームクリアにする
        HeroController.gameState = "gameclear";

        if (BGMManager.Instance != null && gameClearBGM != null)
        {
            BGMManager.Instance.ChangeBGM(gameClearBGM);
        }

        //３秒後にタイトルに戻る
        Invoke("GoToTitle", 7.0f);
    }

    public void GameOver()
    {
        //プレイヤー死亡！
        retryButton.SetActive(true);   //ボタン表示
        mainImage.SetActive(true);     //画像表示
                                       //画像を設定する
        mainImage.GetComponent<Image>().sprite = gameOverSpr;

        if (BGMManager.Instance != null && gameOverBGM != null)
        {
            BGMManager.Instance.ChangeBGM(gameOverBGM);
        }
    }


    //タイトルに戻る
    void GoToTitle()
    {
        SceneManager.LoadScene("Title");    //タイトルに戻る
    }
}
