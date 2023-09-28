using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective2 : MonoBehaviour
{
    public bool obj2Completed = false;
    [SerializeField] private GameObject obj3Lights;
    [SerializeField] private GameObject obj2Lights;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Objectives.instance.CompleteObjective(1);
            obj2Completed = true;
            obj3Lights.SetActive(true);
            obj2Lights.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
}
