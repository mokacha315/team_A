using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public float fadeSpeed = 1.0f;

    private AudioClip nextClip = null; //次の曲
    private bool isFading = false;


    void Update()
    {
        if (isFading)
        {
            //フェードアウト
            if (bgmSource.volume > 0f && nextClip != null)
            {
                bgmSource.volume -= fadeSpeed * Time.deltaTime;
            }
            //フェードアウト終わり → 曲切り替え
            else if (nextClip != null)
            {
                bgmSource.clip = nextClip;
                bgmSource.Play();
                nextClip = null;
            }
            //フェードイン
            else if (bgmSource.volume < 1f)
            {
                bgmSource.volume += fadeSpeed * Time.deltaTime;
            }
            else
            {
                isFading = false;
            }
        }
    }


    public void ChangeBGM(AudioClip newClip)
    {
        if (bgmSource.clip == newClip) //同じ曲ならなにもしない
            return;

        nextClip = newClip;
        isFading = true;
    }
}
