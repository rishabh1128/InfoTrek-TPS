using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickup : MonoBehaviour
{
    [SerializeField] private GameObject PlayerRifle;

    [SerializeField] private GameObject player;
    private float radius = 2.5f; //Object will be picked up when player is in the radius and presses F
    

    private void Awake()
    {
        PlayerRifle.SetActive(false);

    }

    private void Update()
    {

        if(Vector3.Distance(transform.position,player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerRifle.SetActive(true);
                player.GetComponent<PlayerPunch>().enabled = false;

                //play pickup sound

                //Objective completed
                gameObject.SetActive(false);
            }
        }
    }
}
