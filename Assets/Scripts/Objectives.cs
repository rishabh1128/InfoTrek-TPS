using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objectives : MonoBehaviour
{
    [Header("Objectives")]
    public GameObject[] objs;
    [SerializeField] private GameObject objCompletedText;
    [SerializeField] private Timer timer;
    [SerializeField] private Player player;

    public static Objectives instance;
    //TODO: display objective complete text and add more spotlights to guide players
    private void Start()
    {
        instance = this;
    }

    public void CompleteObjective(int obj_id)
    {
        Text obj = objs[obj_id].GetComponent<Text>();
        obj.color = Color.green;
        if (obj_id < objs.Length - 1)
        {
            objs[obj_id + 1].SetActive(true);
        }
        StartCoroutine(ObjectiveCompletedTextTimer());
        player.IncreaseScore(5000 - timer.getTime());
    }

    IEnumerator ObjectiveCompletedTextTimer()
    {
        objCompletedText.SetActive(true);
        yield return new WaitForSeconds(2f);
        objCompletedText.SetActive(false);
    }
}
