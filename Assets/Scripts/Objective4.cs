using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective4 : MonoBehaviour
{
    [SerializeField] private GameObject nextObjective;
    [SerializeField] private GameObject nextObjectiveFlashScreen;
    [SerializeField] private Player playerScript;
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private GameObject vehicleDoor;
    [SerializeField] private GameObject TPSCam;
    [SerializeField] private GameObject AimCam;
    [SerializeField] private GameObject AimCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vehicle")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Objectives.instance.CompleteObjective(3);
           
            // TODO: do the dramatic stuff leading upto "Survive" like oops car broke down or smthng -- DONE

            playerScript.transform.position = vehicleDoor.transform.position;
            playerScript.inCar = false;
            TPSCam.SetActive(true);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
            playerCharacter.SetActive(true);
            Destroy(other.gameObject, 2f);

            StartCoroutine(WaitForNext());

            nextObjective.SetActive(true);
            Destroy(gameObject, 30f);
        }
    }
    IEnumerator WaitForNext()
    {
        nextObjectiveFlashScreen.SetActive(true);
        yield return new WaitForSeconds(15f);
        nextObjectiveFlashScreen.SetActive(false);
    }
}
