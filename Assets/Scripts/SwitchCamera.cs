using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to assign")]
    [SerializeField] private GameObject aimCam;
    [SerializeField] private GameObject aimCanvas;
    [SerializeField] private GameObject TPSCam;
    [SerializeField] private GameObject TPSCanvas;
    public Animator animator;
    [SerializeField] private Player player;

    //TODO: Turn off aiming when running or jumping
    private void Update()
    {
        if (Input.GetButton("Fire2") && !player.isJumping)
        {
            TPSCam.SetActive(false);
            TPSCanvas.SetActive(false);
            aimCam.SetActive(true);
            aimCanvas.SetActive(true);
            /*animator.SetBool("Aim", true);
            animator.SetBool("Idle", false);*/
            PlayAnimation("Aim");
        }
        else
        {
            TPSCam.SetActive(true);
            TPSCanvas.SetActive(true);
            aimCam.SetActive(false);
            aimCanvas.SetActive(false);
            //animator.SetBool("Aim", false);
        }
    }

    private void PlayAnimation(string anim)
    {
        if (anim.Equals("Jump"))
            animator.SetTrigger(anim);
        else
            animator.SetBool(anim, true);
        string[] arr = { "Idle", "Walk", "Running", "Jump", "Aim", "Shoot", "Reloading", "Punch", "Dying" };
        foreach (string s in arr)
        {
            if (s.Equals(anim))
                continue;
            else if (s.Equals("Jump"))
            {
                animator.ResetTrigger(s);
            }
            else
            {
                animator.SetBool(s, false);
            }
        }

        
    }
}
