using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoost : MonoBehaviour
{
    [Header("Main stuff")]

    [SerializeField] private Rifle rifle;
    private int ammoToGive = 120;
    private float radius = 2.5f;
    private bool given = false;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float vol;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private void Update()
    {
        if (Vector3.Distance(transform.position, rifle.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F) && !given)
            {
                animator.SetBool("Open", true);
                rifle.IncreaseAmmo(ammoToGive);
                given = true;
                PlaySound(audioClip, vol);
                Destroy(gameObject, 1.5f);
            }
            
        }
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        audioSource.volume = vol;
        audioSource.PlayOneShot(clip);
    }
}
