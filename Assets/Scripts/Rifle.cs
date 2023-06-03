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
    public float reloadTime = 2.5f;
    private bool reloading = false;

    [Header("Rifle effects")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Animator animator;

    private void Awake()
    {
        transform.SetParent(hand);
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (reloading)
            return;

        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            mag--;
            StartCoroutine(Reload());
            return;
        }

        //TODO: Whenever shooting or aiming, face the player forward

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            animator.SetBool("Shoot", true);
            animator.SetBool("Idle", false);
            nextTimeToShoot = Time.time + timeBetweenShots;
            Shoot();
        }
        else if (Input.GetButton("Fire1") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Shoot Walk", true);
            animator.SetBool("Idle", false);
        }
        else if(Input.GetButton("Fire1"))
        {
            animator.SetBool("Shoot", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Shoot Walk", false);
            animator.SetBool("Rifle Walk", false);
        }
        else
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("Shoot Walk", false);
            animator.SetBool("Rifle Walk", false);
        }

        if (Input.GetButton("Fire2") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("Rifle Walk", true);
            animator.SetBool("Idle", false);
        }
    }
    private void Shoot()
    {

        //check mag

        if (mag==0)
        {
            //TODO: show out of ammo text
            return;
        }

        --currentAmmo;
        //TODO: Update UI


        
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
        animator.SetBool("Reloading", true);
        //play sound
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("Reloading", false);
        currentAmmo = maxAmmo;
        player.playerWalk = 3f;
        player.playerSprint = 6f;
        reloading = false;
    }
}
