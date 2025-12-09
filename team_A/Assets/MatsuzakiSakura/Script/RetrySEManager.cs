using UnityEngine;

public class RetrySEManager : MonoBehaviour
{
    public static RetrySEManager Instance{ get; private set;}

    public AudioSource audioSource;
    public AudioClip retryClip;

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

        DontDestroyOnLoad(gameObject);
    }
    public void PlayRetrySE()
    {
        if (retryClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(retryClip);
        }
    }
}
