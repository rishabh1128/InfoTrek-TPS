using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Objectives.instance.CompleteObjective(1);
            Destroy(gameObject, 1f);
        }
    }
}
