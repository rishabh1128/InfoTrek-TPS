using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")] //Adds headers in the inspector panel in Unity
    [SerializeField] private float playerSpeed = 1.9f;

    [Header("Player Animator and Gravity")]
    [SerializeField] private CharacterController characterController;
    
    private void Update()
    {
        playerMove();
    }
    
    private void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if(direction.magnitude >= 0.1f)
        {
            characterController.Move(direction * playerSpeed * Time.deltaTime);
        }
    }
}
