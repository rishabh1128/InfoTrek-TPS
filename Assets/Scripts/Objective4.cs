using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective4 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vehicle")
        {
            Objectives.instance.CompleteObjective(3);
            // TODO: do the dramatic stuff leading upto "Survive" like oops car broke down or smthng
            Destroy(gameObject, 1f);
        }
    }
}
