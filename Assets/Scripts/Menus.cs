using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [Header("All Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject objectiveMenu;
    [SerializeField] private GameObject playerUI;
    /*[SerializeField] private GameObject TPSCanvas;*/

    public static Menus instance;

    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isObjectiveMenu = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Tab) && !isGameOver && isObjectiveMenu)
        {
            RemoveObjectives();
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && !isGameOver && !isObjectiveMenu)
        {
            ShowObjectives();
        }
    }
    public void ShowObjectives()
    {
        CloseAllMenus();
        objectiveMenu.SetActive(true);
        playerUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f; //rate at which time passes in the game
        isObjectiveMenu = true;
        /*TPSCanvas.SetActive(false);*/
        
    }

    public void RemoveObjectives()
    {
        CloseAllMenus();
    }

    private void Pause()
    {
        CloseAllMenus();
        pauseMenu.SetActive(true);
        playerUI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        /*TPSCanvas.SetActive(false);*/
        
    }
    //TODO: solve the bug where button remains selected

    public void Resume()
    {
        CloseAllMenus();
    }

    public void LoadMenu()
    {
        //TODO: solve the animation bug on opening menu from game  -- Solved by removing animation :)
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ShowGameOver()
    {
        CloseAllMenus();
        gameOver.SetActive(true);
        playerUI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        isGameOver = true;
    }

    private void CloseAllMenus()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        objectiveMenu.SetActive(false);
        isObjectiveMenu = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        playerUI.SetActive(true);
       /* TPSCanvas.SetActive(true);*/
        
    }
}
