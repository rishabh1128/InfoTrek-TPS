using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle parameters")]
    [SerializeField] private Camera cam;
    [SerializeField] private float giveDmg = 10f;
    [SerializeField] private float shootingRange = 100f;
    [SerializeField] private float  timeBetweenShots= 0.25f;
    private float nextTimeToShoot = 0f;
    [SerializeField] private Player player;
    [SerializeField] private Transform hand;

    [Header("Rifle Ammo and shooting")]
    [SerializeField] private int maxAmmo = 32;
    [SerializeField] private int mag = 10;
    private int currentAmmo;
    [SerializeField] private float reloadTime = 1.3f;
    private bool reloading = false;

    [Header("Rifle effects")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private void Awake()
    {
        transform.SetParent(hand);
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (reloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        /*if (Input.GetButtonDown("Fire1"))
        { 
            Shoot();
        }*/

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + timeBetweenShots;
            Shoot();
        }
    }
    private void Shoot()
    {

        //check mag

        if (mag==0)
        {
            //show out of ammo text
            return;
        }

        if (--currentAmmo == 0)
        {
            mag--;
        }
        //Update UI


        
        muzzleFlash.Play();
        RaycastHit hitInfo;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward,out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>(); //get the object hit

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDmg); //give damage
                GameObject impactGo = Instantiate(impactEffect,hitInfo.point,Quaternion.LookRotation(hitInfo.normal)); //show impact
                //TODO : add different impact to everything
            }
        }

        
    }

    IEnumerator Reload() //enumerator tells script to wait without hogging CPU and continue after reloading
    {
        player.playerWalk = 0f;
        player.playerSprint = 0f;
        reloading = true;
        Debug.Log("Reloading..");
        yield return new WaitForSeconds(reloadTime);
        //play animation and sound
        currentAmmo = maxAmmo;
        player.playerWalk = 1.9f;
        player.playerSprint = 3f;
        reloading = false;
    }
}
