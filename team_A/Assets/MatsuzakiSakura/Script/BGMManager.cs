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
    /// BGMを再生する処理
    /// </summary>
    void Start()
    {
        if (initialBGM != null && bgmSource != null)
        {
            bgmSource.clip = initialBGM; //BGMセット
            bgmSource.volume = 1f;       //音量
            bgmSource.loop = true;       //ループオン
            bgmSource.Play();            //再生
        }
    }

    /// <summary>
    /// BGMのフェードアウト・フェードイン
    /// </summary>
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


    /// <summary>
    /// 曲の切り替え
    /// </summary>
    /// <param name="newClip">これから再生したいBGM</param>
    public void ChangeBGM(AudioClip newClip)
    {
        if (bgmSource.clip == newClip) //同じ曲ならなにもしない
            return;

        nextClip = newClip;
        isFading = true;
    }
}
