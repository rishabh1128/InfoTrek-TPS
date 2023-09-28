using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Wheels Colliders")]
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;

    [Header("Wheels Transforms")]
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform vehicleDoor;

    //TODO: tune vehicle parameters --DONE
    //TODO: add vehicle sound
    //TODO: add a text telling that player can enter when pressing F and display controls

    [Header("Vehicle Engine")]
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float brakeForce = 200f;
    private float currSpeed = 0f;
    private float currBrakeForce = 0f;

    [Header("Vehicle Steering")]
    [SerializeField] private float wheelTorque = 20f;
    private float currTurnAngle = 0f;

    [Header("Vehicle Security")]
    [SerializeField] private Player playerScript;
    private float radius = 5f;

    [Header("Things to Disable")]
    [SerializeField] private GameObject AimCam;
    [SerializeField] private GameObject AimCanvas;
    [SerializeField] private GameObject TPSCam;
    //[SerializeField] private GameObject TPSCanvas;
    [SerializeField] private GameObject playerCharacter;

    [Header("Vehicle Hit variables")]
    [SerializeField] private Camera cam;
    [SerializeField] private float hitRange = 1f;
    [SerializeField] private float giveDmg = 100f;
    [SerializeField] private GameObject goreEffect;


    private bool isObjectiveComplete = false;
    [SerializeField] private Objective2 obj2;
    [SerializeField] private GameObject nextObjective;
    [SerializeField] private GameObject nextObjectiveLights;
    [SerializeField] private GameObject objLights;

    private void Start()
    {
        //solve vehicle jitter

        frontLeftWheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
        frontRightWheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
        backLeftWheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
        backRightWheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
    }

    private void Update()
    {
        if(!playerScript.inCar && Vector3.Distance(transform.position,playerScript.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerScript.inCar = true;
                radius = 5000f;

                TPSCam.SetActive(false);
                //TPSCanvas.SetActive(false);
                AimCam.SetActive(false);
                AimCanvas.SetActive(false);
                playerCharacter.SetActive(false);

                //TODO: objective complete--DONE
                if (!isObjectiveComplete && obj2.obj2Completed)
                {
                    Objectives.instance.CompleteObjective(2);
                    isObjectiveComplete = true;
                    nextObjective.SetActive(true);
                    nextObjectiveLights.SetActive(true);
                    objLights.SetActive(false);
                }
                
            }
            
        }

        if (playerScript.inCar)
        {

            if (Input.GetKeyDown(KeyCode.G))
            {
                playerScript.transform.position = vehicleDoor.transform.position;
                playerScript.inCar = false;
                radius = 5f;

                TPSCam.SetActive(true);
                //TPSCanvas.SetActive(true);
                AimCam.SetActive(true);
                AimCanvas.SetActive(true);
                playerCharacter.SetActive(true);

                return;
            }

            MoveVehicle();
            Steering();
            ApplyBrakes();
            HitZombies();
        }
    }
    private void MoveVehicle()
    {
        frontRightWheelCollider.motorTorque = currSpeed;
        frontLeftWheelCollider.motorTorque = currSpeed;
        /*backLeftWheelCollider.motorTorque = currSpeed;
        backRightWheelCollider.motorTorque = currSpeed;*/
        //four wheel drive

        currSpeed = maxSpeed * -Input.GetAxis("Vertical");
    }

    private void Steering()
    {
        currTurnAngle = wheelTorque * Input.GetAxis("Horizontal");

        frontRightWheelCollider.steerAngle = currTurnAngle;
        frontLeftWheelCollider.steerAngle = currTurnAngle;

        AnimateWheels(frontRightWheelCollider, frontRightWheelTransform);
        AnimateWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        AnimateWheels(backLeftWheelCollider, backLeftWheelTransform);
        AnimateWheels(backRightWheelCollider, backRightWheelTransform);
    }

    private void AnimateWheels(WheelCollider WC,Transform WT)
    {
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);

        WT.position = position;
        WT.rotation = rotation;
    }

    private void ApplyBrakes()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            currBrakeForce = brakeForce;
        }
        else
        {
            currBrakeForce = 0f;
        }

        frontRightWheelCollider.brakeTorque = currBrakeForce;
        frontLeftWheelCollider.brakeTorque = currBrakeForce;
        backLeftWheelCollider.brakeTorque = currBrakeForce;
        backRightWheelCollider.brakeTorque = currBrakeForce;
    }

    private void HitZombies()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(cam.transform.position,1f, cam.transform.forward, out hitInfo, hitRange))
        {
            Debug.Log(hitInfo.transform.name);

            BasicZombie basicZombie = hitInfo.transform.GetComponent<BasicZombie>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();

         
            if (basicZombie != null)
            {
                basicZombie.zombieHitDmg(giveDmg);
                basicZombie.GetComponent<CapsuleCollider>().enabled = false;
                GameObject gore = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(gore, 2.5f);
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDmg(giveDmg);
                zombie2.GetComponent<CapsuleCollider>().enabled = false;
                GameObject gore = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(gore, 2.5f);
            }
        }
    }
}
