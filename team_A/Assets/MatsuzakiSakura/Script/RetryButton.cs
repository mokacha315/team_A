using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RetryButton : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button testButton;

    public string retrySceneName = "";  //リトライするシーン名

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        testButton = GetComponent<UnityEngine.UI.Button>();

        testButton.onClick.AddListener(OnButtonClicked);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnButtonClicked();
        }
    }

    private void OnButtonClicked()
    {
        //HPを戻す
        HeroController.hp = 10;
        //ゲーム中に戻す
        SceneManager.LoadScene(retrySceneName);         //シーン移動
    }


}
