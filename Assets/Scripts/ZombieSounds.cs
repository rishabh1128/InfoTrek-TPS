using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] idleSounds;
    [SerializeField] private float idleVol;
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private float stepVol;
    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomSounds()
    {
        return idleSounds[Random.Range(0, idleSounds.Length)];
    }

    private void MakeSound()
    {
        AudioClip clip = GetRandomSounds();
        PlaySound(clip, idleVol);
    }

    private void Step()
    {
        PlaySound(stepSound, stepVol);
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        audioSource.volume = vol;
        audioSource.PlayOneShot(clip);
    }
}
