using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float giveDmg = 5f;
    [SerializeField] private float punchRange = 2f;
    [SerializeField] private float timeBetweenPunch = 1.33f;
    private float nextTimeToPunch = 0f;
    [SerializeField] private GameObject aimCanvas;
    public GameObject impactEffect;
    public Animator animator;

    private void Awake()
    {
        aimCanvas.SetActive(false);
    }
    public void Punch()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, punchRange))
        {
            Debug.Log(hitInfo.transform.name);
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>(); //get the object hit

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDmg); //give damage
                
            }

            GameObject impactGo = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToPunch)
        {
            animator.SetBool("Punch",true);
            animator.SetBool("Idle", false);
            nextTimeToPunch = Time.time + timeBetweenPunch;
            Punch();
        }
        else
        {
            animator.SetBool("Punch", false);
            animator.SetBool("Idle", true);
        }
    }
}
