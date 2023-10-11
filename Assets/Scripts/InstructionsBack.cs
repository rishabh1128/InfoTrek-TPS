using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsBack : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject instructionsScreen;
    public void Back()
    {
        instructionsScreen.SetActive(false);
        pauseMenu.SetActive(true);
        Menus.instance.isInstructions = false;
    }
}
