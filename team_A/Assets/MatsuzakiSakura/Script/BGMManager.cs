using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    public AudioClip initialBGM;

    public AudioSource bgmSource;
    public float fadeSpeed = 1.0f;

    private AudioClip nextClip = null; //次の曲
    private bool isFading = false;

    /// <summary>
    /// BGMManagerをシーン内１つだけにする
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        if (initialBGM != null && bgmSource != null)
        {
            bgmSource.clip = initialBGM;
            bgmSource.volume = 1f;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
    void Update()
    {
        if (TitleManager.startPressed)
        {
            //フェードアウト
            if (bgmSource.volume > 0f && nextClip != null)
            {
                bgmSource.volume -= fadeSpeed * Time.deltaTime;
            }
        }

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
