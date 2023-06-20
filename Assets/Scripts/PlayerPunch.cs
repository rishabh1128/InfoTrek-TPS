using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float giveDmg = 5f;
    [SerializeField] private float punchRange = 5f;
    [SerializeField] private float timeBetweenPunch = 1.33f;
    private float nextTimeToPunch = 0f;
    [SerializeField] private GameObject aimCanvas;
    /*public GameObject woodEffect;
    public GameObject goreEffect;*/
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
            BasicZombie basicZombie = hitInfo.transform.GetComponent<BasicZombie>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDmg); //give damage
                /*GameObject wood = Instantiate(woodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(wood, 2.5f);*/
            }
            else if (basicZombie != null)
            {
                basicZombie.zombieHitDmg(giveDmg);
                /*GameObject gore = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(gore, 2.5f);*/
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDmg(giveDmg);
                /*GameObject gore = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(gore, 2.5f);*/
            }
        }
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToPunch)
        {
            /*animator.SetBool("Punch",true);
            animator.SetBool("Idle", false);*/
            PlayAnimation("Punch");
            nextTimeToPunch = Time.time + timeBetweenPunch;
            Punch();
        }
        else
        {
            //PlayAnimation("Idle");
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
