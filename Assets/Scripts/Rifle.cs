using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle stuff")]
    [SerializeField] private Camera cam;
    [SerializeField] private float giveDmg = 10f;
    [SerializeField] private float shootingRange = 100f;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward,out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDmg);
            }
        }
    }
}
