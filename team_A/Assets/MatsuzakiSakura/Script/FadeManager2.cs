using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager2 : MonoBehaviour
{
    public Image panelImage;
    public float fadeDuration = 1f;  //フェードにかかる時間

    
    /// <summary>
    /// フェードアウト
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    /// <summary>
    /// パネルの透明度を時間経過で徐々に変化させるフェード処理
    /// </summary>
    /// <param name="startAlpha">開始時アルファ値</param>
    /// <param name="endAlpha">終了時アルファ値</param>
    /// <returns></returns>
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = panelImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            panelImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        panelImage.color = color;
    }
}
