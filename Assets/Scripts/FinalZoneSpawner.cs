using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZoneSpawner : MonoBehaviour
{
    [Header("Zombie spawn variables")]
    [SerializeField] private GameObject[] zombiePrefabs;
    [SerializeField] private Transform[] zombieSpawnPositions;
    [SerializeField] private GameObject waveIncomingWarning;

    private AudioSource audioSource;
    [SerializeField] private float spawnCount = 3f;
    [SerializeField] private float waitTime = 60f;
    private float nextTimeToSpawn = 0f;
    private bool startSpawn = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            startSpawn = true;
        }
    }

    private void Update()
    {
        if (startSpawn && Time.time >= nextTimeToSpawn)
        {
            audioSource.Play();
            StartCoroutine(WaveWarning());
            for (int i = 0; i < spawnCount; i++)
            {
                Invoke("EnemySpawner", 1f);
            }
            nextTimeToSpawn = Time.time + waitTime;
        }
        
    }

    private void EnemySpawner()
    {
        for(int i = 0; i < zombiePrefabs.Length; i++)
        {
            GameObject go = Instantiate(zombiePrefabs[i], zombieSpawnPositions[i].position, zombieSpawnPositions[i].rotation);
            go.SetActive(true);
        }
    }

    IEnumerator WaveWarning()
    {
        waveIncomingWarning.SetActive(true);
        yield return new WaitForSeconds(2f);
        waveIncomingWarning.SetActive(false);
    }
}
