using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHealthBarUI : MonoBehaviour
{
    [SerializeField] private Transform mainCam;

    private void LateUpdate() //called after every Update() has been processed
    {
        transform.LookAt(transform.position + mainCam.forward);
    }
}
