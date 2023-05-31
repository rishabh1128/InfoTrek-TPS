using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")] //Adds headers in the inspector panel in Unity
    private float speed;
    [SerializeField] private float playerWalk = 1.9f;
    [SerializeField] private float playerSprint = 3f;


    [Header("Player Cameras")]
    [SerializeField] Transform playerCamera;

    [Header("Player Animator and Gravity")]
    [SerializeField] CharacterController characterController;
    [SerializeField] private float gravity = -9.81f;

    [Header("Player Jump and velocity")]
    [SerializeField] private float turnCamTime = 0.1f;
    [SerializeField] private float turnCamVelocity;
    [SerializeField] private float jumpRange = 1f;
    Vector3 velocity;
    [SerializeField] private Transform surfaceCheck;
    private bool onSurface;
    [SerializeField] private float surfaceDistance = 0.4f;
    [SerializeField] private LayerMask surfaceMask;

    private void Update()
    {
        ApplyGravity();
        ProcessMove();
        Jump();
    }

    private void ApplyGravity()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask); //to check if the player is on the ground
        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void ProcessMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; //get the angle we want the character to face after movement
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,ref turnCamVelocity,turnCamTime); //to smoothen the angle change
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            CheckSprint();
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            velocity.y = Mathf.Sqrt(jumpRange * -2f * gravity);
        }
    }

    private void CheckSprint()
    {
        if(Input.GetButton("Sprint") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && onSurface)
        {
            speed = playerSprint;
        }
        else
        {
            speed = playerWalk;
        }
    }
}
