using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private float stepVol;
    [SerializeField] private AudioClip footstepSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Step()
    {
        PlaySound(footstepSound, stepVol);
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        audioSource.volume = vol;
        audioSource.PlayOneShot(clip);
    }
}
