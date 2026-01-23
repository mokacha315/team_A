using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// ゲームをリスタート(今までの数値リセット)
    /// </summary>
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HeroController.ResetStaticVariables();
    }
}
