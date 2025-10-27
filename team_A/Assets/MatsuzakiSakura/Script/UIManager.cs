using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int hp = 0;                      //�v���C���[��HP
    public GameObject lifeImage;     //HP�̐���\������Image
    public Sprite life100Image;      //HP100�摜
    public Sprite life90Image;       //HP90�摜
    public Sprite life80Image;       //HP80�摜
    public Sprite life70Image;       //HP70�摜
    public Sprite life60Image;       //HP60�摜
    public Sprite life50Image;       //HP50�摜
    public Sprite life40Image;       //HP40�摜
    public Sprite life30Image;       //HP30�摜
    public Sprite life20Image;       //HP20�摜
    public Sprite life10Image;       //HP10�摜
    public Sprite life0Image;        //HP0�摜
    public GameObject mainImage;     //�摜������GameObject
    public GameObject retryButton;   //���g���C�{�^��
    public Sprite gameOverSpr;       //GAME OVER�摜
    public Sprite gameClearSpr;      //GAME CLEAR�摜
    public string retrySceneName = "";  //���g���C����V�[����

    //HP�X�V
    void UpdateHP()
    {
        //Player�擾
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
                        //�v���C���[���S�I
                        retryButton.SetActive(true);   //�{�^���\��
                        mainImage.SetActive(true);     //�摜�\��
                                                       //�摜��ݒ肷��
                        mainImage.GetComponent<Image>().sprite = gameOverSpr;
                        HeroController.gameState = "gameend";    //�Q�[���I��
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

    //���g���C
    public void Retry()
    {
        //HP��߂�
        HeroController.hp = 10;
        //�Q�[�����ɖ߂�
        SceneManager.LoadScene(retrySceneName);         //�V�[���ړ�
    }

    //�摜���\���ɂ���
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHP();         //HP�X�V
        //�摜���\���ɂ���
        Invoke("InactiveImage", 1.0f);
        retryButton.SetActive(false);    //�{�^����\��
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHP();
    }

    //�Q�[���N���A
    public void GameClear()
    {
        //�摜�\��
        mainImage.SetActive(true);
        mainImage.GetComponent<Image>().sprite = gameClearSpr;  //�uGAME CLEAR�v��ݒ肷��
        //�Q�[���N���A�ɂ���
        HeroController.gameState = "gameclear";
        //�R�b��Ƀ^�C�g���ɖ߂�
        Invoke("GoToTitle", 3.0f);
    }
    //�^�C�g���ɖ߂�
    void GoToTitle()
    {
        SceneManager.LoadScene("Title");    //�^�C�g���ɖ߂�
    }
}
