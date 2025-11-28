using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager2 : MonoBehaviour
{
    public Image panelImage;
    public float fadeDuration = 1f;  //フェードにかかる時間

    //フェードアウト
    public void FadeOut()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    //フェードイン
    public void FadeIn()
    {
        StartCoroutine(Fade(1f, 0f));
    }

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
