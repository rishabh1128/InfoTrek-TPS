using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject characterSelectScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject leaderboard;

    //TODO: remove select char button and activate function on clicking play  --DONE

    public void OnPlay()
    {
        characterSelectScreen.SetActive(true);
        leaderboard.SetActive(false);
        mainMenu.SetActive(false);
    }
    public void OnLeaderboard()
    {
        mainMenu.SetActive(false);
        characterSelectScreen.SetActive(false);
        leaderboard.SetActive(true);
        PlayFabManager.instance.GetLeaderboard();
    }

    public void OnQuit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
