using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    [SerializeField] private Text ammoText;

    public static AmmoCount instance;

    private void Awake()
    {
        instance = this;
    }

    public void updateAmmo(int curAmmo,int totalAmmo)
    {
        ammoText.text = curAmmo.ToString() + " I " + totalAmmo.ToString();
    }
}
