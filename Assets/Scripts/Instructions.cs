using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject selectCharacter;

    public void OnPlay()
    {
        instructions.SetActive(false);
        selectCharacter.SetActive(true);
    }
}
