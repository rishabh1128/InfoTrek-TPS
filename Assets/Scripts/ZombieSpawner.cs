using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie spawn variables")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform zombieSpawnPosition;
    //[SerializeField] private GameObject dangerZone1;
    private float repeatCycle = 1f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InvokeRepeating("EnemySpawner", 1f, repeatCycle);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, 10f);
        }
    }

    private void EnemySpawner()
    {
        GameObject go = Instantiate(zombiePrefab, zombieSpawnPosition.position,zombieSpawnPosition.rotation);
        go.SetActive(true);
    }
}
