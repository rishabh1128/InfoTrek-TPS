using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public void OnCharacter1()
    {
        SceneManager.LoadScene("Zombie Apocalypse 2");
    }

    public void OnCharacter2()
    {
        SceneManager.LoadScene("Zombie Apocalypse");
    }

    public void OnCharacter3()
    {
        SceneManager.LoadScene("Zombie Apocalypse 1");
    }
}
