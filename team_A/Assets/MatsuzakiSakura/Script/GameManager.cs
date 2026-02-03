using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// ゲームをリスタート(今までの数値リセット)
    /// </summary>
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
  
    }

    public void Retry()
    {
        //もう一度最初から読み直す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
