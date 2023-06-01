using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToHit : MonoBehaviour
{
    [SerializeField] private float objectHealth = 30f;

    public void ObjectHitDamage(float amount)
    {
        objectHealth -= amount;
        if (objectHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

}
