using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject leaderboard;
    //TODO: remove select char button and activate function on clicking play  --DONE
    [SerializeField] private GameObject instructions;
    [SerializeField] private PlayFabManager playFabManager;

    private void Awake()
    {
        playFabManager.GetMaxScore();
    }
    public void OnPlay()
    {
        instructions.SetActive(true);
        leaderboard.SetActive(false);
        mainMenu.SetActive(false);
    }
    public void OnLeaderboard()
    {
        mainMenu.SetActive(false);
        leaderboard.SetActive(true);
        PlayFabManager.instance.GetLeaderboard();
    }

    public void OnQuit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
