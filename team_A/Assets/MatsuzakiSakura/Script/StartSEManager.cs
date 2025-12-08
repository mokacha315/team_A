using UnityEngine;

public class StartSEManager : MonoBehaviour
{
    public static StartSEManager Instance;

    public AudioSource seSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySE(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        seSource.PlayOneShot(clip);
    }
}
