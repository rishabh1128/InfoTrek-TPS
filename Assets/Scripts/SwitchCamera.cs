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

    private void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            TPSCam.SetActive(false);
            TPSCanvas.SetActive(false);
            aimCam.SetActive(true);
            aimCanvas.SetActive(true);
            animator.SetBool("Aim", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            TPSCam.SetActive(true);
            TPSCanvas.SetActive(true);
            aimCam.SetActive(false);
            aimCanvas.SetActive(false);
            animator.SetBool("Aim", false);
        }
    }
}
