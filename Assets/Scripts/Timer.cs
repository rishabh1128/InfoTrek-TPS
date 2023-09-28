
using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{ 

    private float curTime;
    [SerializeField] private Player player;
    private TimeSpan time;
    private float timeToIncreaseScore = 1f;
    private float nextTimeToIncreaseScore = 1f;

    void Start()
    {
        curTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        time = TimeSpan.FromSeconds(curTime);
        gameObject.GetComponent<Text>().text = time.ToString(@"hh\:mm\:ss");

        if(time.Seconds % 5 == 0 && Time.time >= nextTimeToIncreaseScore)
        {
            player.IncreaseScore(10);
            nextTimeToIncreaseScore = Time.time + timeToIncreaseScore;
        }
    }

    public int getTime()
    {
        return time.Seconds;
    }
}
