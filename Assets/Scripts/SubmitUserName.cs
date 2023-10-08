using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitUserName : MonoBehaviour
{
    [SerializeField] private Text nameInput;

    public void OnSubmit()
    {
        PlayFabManager.instance.SubmitNameButton(nameInput.text);
    }
}
