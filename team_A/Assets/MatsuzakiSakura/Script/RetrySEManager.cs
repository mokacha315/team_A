using UnityEngine;

public class RetrySEManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip retryClip;

    public void PlayRetrySE()
    {
        if (retryClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(retryClip);
        }
    }

}
