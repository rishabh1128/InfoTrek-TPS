using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle parameters")]
    [SerializeField] private Camera cam;
    private float turnCamVelocity;
    [SerializeField] private float turnCamTime = 0.1f;
    [SerializeField] private float giveDmg = 10f;
    [SerializeField] private float shootingRange = 100f;
    [SerializeField] private float  timeBetweenShots= 0.25f;
    private float nextTimeToShoot = 0f;
    [SerializeField] private Player player;
    [SerializeField] private Transform hand;

    [Header("Rifle Ammo and shooting")]
    [SerializeField] private int magSize = 30;
    [SerializeField] private int totalAmmo = 330;
    [SerializeField] private GameObject ammoOutDisplay;
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private float shootVol;
    [SerializeField] private AudioClip reloadingSound;
    [SerializeField] private float reloadVol;
    private AudioSource audioSource;

    private int currentAmmo;
    public float reloadTime = 3.3f;
    private bool reloading = false;

    //TODO: change reloading mechanism to use total bullets instead of mags so u can reload as much as u want without losing bullets -- DONE
    
    //TODO: make a function to turn off all animations except the one to turn on, passed as a string -- DONE

    [Header("Rifle effects")]
    public ParticleSystem muzzleFlash;
    public GameObject woodEffect;
    public GameObject goreEffect;
    public Animator animator;

    private void Awake()
    {
        transform.SetParent(hand);
        currentAmmo = magSize;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (totalAmmo > 0 && (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < magSize && !player.isJumping)))
        {
            int remaining = magSize - currentAmmo;
            totalAmmo = Math.Max(0, totalAmmo - remaining);
            StartCoroutine(Reload());
            return;
        }

        if (reloading || animator.GetBool("Dying") || !(Input.GetButton("Fire2") && !player.isJumping))
            return;

        
        //TODO: Whenever shooting or aiming, face the player forward

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            /*animator.SetBool("Shoot", true);
            animator.SetBool("Idle", false);*/
            PlayAnimation("Shoot");                                    //Note: The animation is played here instead of in Shoot() so that it keeps playing when the button is held down
            nextTimeToShoot = Time.time + timeBetweenShots;
            Shoot();
        }
        /*else if (Input.GetButton("Fire1") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            *//*animator.SetBool("Shoot", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Shoot Walk", true);
            animator.SetBool("Idle", false);*//*
            PlayAnimation("Shoot Walk");
        }*/
        else if(Input.GetButton("Fire1"))
        {/*
            animator.SetBool("Shoot", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Shoot Walk", false);
            animator.SetBool("Rifle Walk", false);*/

            PlayAnimation("Shoot");
        }
        
        /*else if (Input.GetButton("Fire2") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            *//* animator.SetBool("Shoot", false);
             animator.SetBool("Rifle Walk", true);
             animator.SetBool("Idle", false);*//*

            PlayAnimation("Rifle Walk");
        }*/

        /*else
        {
            *//*animator.SetBool("Shoot", false);
            animator.SetBool("Shoot Walk", false);
            animator.SetBool("Rifle Walk", false);*//*
            //PlayAnimation("Idle");
        }*/
    }
    private void Shoot()
    {

        //check mag

        if (totalAmmo==0 && currentAmmo==0)
        {
            //TODO: show out of ammo text -- DONE
            StartCoroutine(AmmoOutDisplayTimer());
            return;
        }

        --currentAmmo;
        //TODO: Update UI --- DONE

        AmmoCount.instance.updateAmmo(currentAmmo, totalAmmo);

        //FaceForward();
        muzzleFlash.Play();
        PlaySound(shootingSound,shootVol);

        RaycastHit hitInfo;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward,out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);
            
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>(); //get the object hit
            BasicZombie basicZombie = hitInfo.transform.GetComponent<BasicZombie>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDmg); //give damage
                GameObject wood = Instantiate(woodEffect,hitInfo.point,Quaternion.LookRotation(hitInfo.normal)); //show impact
                Destroy(wood, 2.5f);
                //TODO : add different impact to everything
            }
            else if (basicZombie != null)
            {
                basicZombie.zombieHitDmg(giveDmg);
                GameObject gore = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(gore, 2.5f);
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDmg(giveDmg);
                GameObject gore = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(gore, 2.5f);
            }
        }

        
    }

    private void FaceForward()
    {
        float targetAngle = cam.transform.eulerAngles.y; //get the angle we want the character to face after movement
        float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnCamVelocity, turnCamTime); //to smoothen the angle change
        player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private IEnumerator Reload() //enumerator tells script to wait without hogging CPU and continue after reloading
    {
        player.playerWalk = 0f;
        player.playerSprint = 0f;
        reloading = true;
        //Debug.Log("Reloading..");
        PlayAnimation("Reloading");
        PlaySound(reloadingSound,reloadVol);
        yield return new WaitForSeconds(reloadTime);
        //PlayAnimation("Idle");
        currentAmmo = magSize;
        AmmoCount.instance.updateAmmo(currentAmmo, totalAmmo);
        player.playerWalk = 5f;
        player.playerSprint = 8f;
        reloading = false;
    }

    private void PlayAnimation(string anim)
    {
        string[] arr = { "Idle", "Walk", "Running", "Jump", "Aim", "Shoot", "Reloading", "Punch", "Dying" };
        if (anim.Equals("Jump"))
            animator.SetTrigger(anim);
        else
            animator.SetBool(anim, true);

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
    IEnumerator AmmoOutDisplayTimer()
    {
        ammoOutDisplay.SetActive(true);
        yield return new WaitForSeconds(5f);
        ammoOutDisplay.SetActive(false);
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        audioSource.volume = vol;
        audioSource.PlayOneShot(clip);
    }

    public void IncreaseAmmo(int ammo)
    {
        totalAmmo += ammo;
        AmmoCount.instance.updateAmmo(currentAmmo, totalAmmo);
    }
}
