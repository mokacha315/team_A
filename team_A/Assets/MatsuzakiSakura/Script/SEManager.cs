using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;

    public AudioSource seSource;

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

    public void PlaySE(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        seSource.PlayOneShot(clip);
    }
}
