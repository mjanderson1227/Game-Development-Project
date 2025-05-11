using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cardFlip;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip hitOrStand;

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

    public void HitOrStandSound()
    {
        audioSource.PlayOneShot(hitOrStand);
    }
}