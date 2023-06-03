using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")] //Adds headers in the inspector panel in Unity
    private float speed;
    public float playerWalk = 3f;
    public float playerSprint = 6f;
    //TODO : change sprint and walk anims

    [Header("Player Cameras")]
    [SerializeField] Transform playerCamera;

    [Header("Player Animator and Gravity")]
    [SerializeField] CharacterController characterController;
    [SerializeField] private float gravity = -9.81f;
    public Animator animator;

    [Header("Player Jump and velocity")]
    [SerializeField] private float turnCamTime = 0.1f;
    [SerializeField] private float turnCamVelocity;
    [SerializeField] private float jumpRange = 2f;
    Vector3 velocity;
    [SerializeField] private Transform surfaceCheck;
    private bool onSurface;
    [SerializeField] private float surfaceDistance = 0.4f;
    [SerializeField] private LayerMask surfaceMask;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  
    }
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

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; //get the angle we want the character to face after movement
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCamVelocity, turnCamTime); //to smoothen the angle change
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        if (direction.magnitude >= 0.1f)
        {

            animator.SetBool("Idle", false);
            
            //TODO: disable shooting while running and disable running while aiming
            CheckSprint();
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //move player in direction of camera
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }

        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            animator.SetBool("Idle", true);
        }
    }

    private void Jump()
    {
        //TODO : Remove the bug that allows player to jump while in the "Jump" state (add jump time delay) and disallow jumping while firing

        if (Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetTrigger("Jump");
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            
            velocity.y = Mathf.Sqrt(jumpRange * -2f * gravity);
        }
        else
        {
            animator.ResetTrigger("Jump");
        }
    }

    private void CheckSprint()
    {
        if(Input.GetButton("Sprint") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && onSurface)
        {
            speed = playerSprint;
            animator.SetBool("Running", true); 
            animator.SetBool("Walk", false);
        }
        else
        {
            speed = playerWalk;
            if(onSurface)
                animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
        }
    }


}
