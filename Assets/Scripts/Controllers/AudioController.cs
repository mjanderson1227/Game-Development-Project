using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cardFlip;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip winSound;

    public void CardFlipSound()
    {
        audioSource.PlayOneShot(cardFlip);
    }

    public void LoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }

    public void WinSound()
    {
        audioSource.PlayOneShot(winSound);
    }
}