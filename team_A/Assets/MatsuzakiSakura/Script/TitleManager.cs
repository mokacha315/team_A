using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject startButton;      //�X�^�[�g�{�^��
    public string firstSceneName;       //�Q�[���J�n�V�[����

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //�X�^�[�g�{�^������
    public void StartButtonClicked()
    {
        SceneManager.LoadScene(firstSceneName);
    }
}