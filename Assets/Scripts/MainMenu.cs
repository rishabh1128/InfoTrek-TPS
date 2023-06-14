using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject characterSelectScreen;
    [SerializeField] private GameObject mainMenu;

    //TODO: remove select char button and activate function on clicking play  --DONE

    public void OnPlay()
    {
        characterSelectScreen.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OnQuit()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
