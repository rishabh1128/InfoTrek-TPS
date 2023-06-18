using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie spawn variables")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform zombieSpawnPosition;
    [SerializeField] private GameObject dangerZoneWarning;
    private AudioSource audioSource;
    private float repeatCycle = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InvokeRepeating("EnemySpawner", 1f, repeatCycle);
            audioSource.Play();
            StartCoroutine(dangerZoneTimer());
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject, 5f);
        }
    }

    private void EnemySpawner()
    {
        GameObject go = Instantiate(zombiePrefab, zombieSpawnPosition.position,zombieSpawnPosition.rotation);
        go.SetActive(true);
    }

    IEnumerator dangerZoneTimer()
    {
        dangerZoneWarning.SetActive(true);
        yield return new WaitForSeconds(2f);
        dangerZoneWarning.SetActive(false);
    }
}
