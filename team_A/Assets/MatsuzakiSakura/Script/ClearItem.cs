using UnityEngine;

public class ClearItem : MonoBehaviour
{
    /// <summary>
    /// 主人公がドアに触れたらゲームクリアにする
    /// </summary>
    /// <param name="other">触れた相手(主人公)</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //UI触れたらゲームクリア
            UIManager ui = FindAnyObjectByType<UIManager>();

            if (ui != null)
            {
                ui.GameClear();
            }

        }
    }
}
