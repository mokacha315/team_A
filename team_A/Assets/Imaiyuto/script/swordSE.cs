using UnityEngine;
using UnityEngine.EventSystems;
public class swordSE : MonoBehaviour
{
    public AudioClip sword_effect_se1;

    public AudioClip sword_se2;

    public AudioClip sword_se3;

    AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnSpaceDicSe1()
    {
        audioSource.PlayOneShot(sword_effect_se1);
    }

}