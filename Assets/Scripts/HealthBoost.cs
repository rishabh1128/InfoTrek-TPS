using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    [Header("Main stuff")]

    [SerializeField] private Player player;
   /* private float healthToGive = 100f;*/
    private float radius = 2.5f;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float vol;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private void Update()
    {
        if(Vector3.Distance(transform.position,player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.SetBool("Open", true);
                player.IncreaseHealth();

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
