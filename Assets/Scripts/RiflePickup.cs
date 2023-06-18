using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickup : MonoBehaviour
{
    [SerializeField] private GameObject PlayerRifle;
    [SerializeField] private GameObject weaponDisplay;
    [SerializeField] private GameObject player;
    private float radius = 2.5f; //Object will be picked up when player is in the radius and presses F
    //TODO : add a text box indicating that player can press F to pick up gun
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    [SerializeField] private float vol = 0.25f;

    private void Awake()
    {
        PlayerRifle.SetActive(false);
        weaponDisplay.SetActive(false);
    }

    // TODO: Fix the bug where the rifle randomly shoots a bullet when being picked up -- related to animation bug??

    private void Update()
    {

        if(Vector3.Distance(transform.position,player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerRifle.SetActive(true);
                weaponDisplay.SetActive(true);
                player.GetComponent<PlayerPunch>().enabled = false;

                //TODO: play pickup sound -- DONE
                PlaySound(clip, vol);

                //TODO: Objective completed -- DONE
                Objectives.instance.CompleteObjective(0);

                Destroy(gameObject, 0.1f);
            }
        }
    }
    private void PlaySound(AudioClip clip, float vol)
    {
        audioSource.volume = vol;
        audioSource.PlayOneShot(clip);
    }
}
