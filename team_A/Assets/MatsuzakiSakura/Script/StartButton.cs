using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartButton : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button testButton;

    public string StartSceneName = "";  //ÉVÅ[Éìñº

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
        FadeManager.Instance.LoadScene(StartSceneName, 1.0f);
    }


}
