using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")] //Adds headers in the inspector panel in Unity
    private float speed;
    public float playerWalk = 5f;
    public float playerSprint = 8f;
    //TODO : change sprint and walk anims and increase base speed -- DONE
    //TODO : fix the wonky animation transitions -- DONE
    //TODO : remove rifle walk feature, allow shooting only when standing still
    //TODO: add the water back to the env -- DONE

    [Header("Player Health")]
    [SerializeField] private float playerHealth = 100f;
    private float curHealth;
    [SerializeField] private GameObject playerDmgSplash;
    [SerializeField] private HealthBar healthBar;

    [Header("Player Cameras")]
    [SerializeField] Transform playerCamera;

    [Header("Player Animator and Gravity")]
    [SerializeField] CharacterController characterController;
    [SerializeField] private float gravity = -9.81f;
    public Animator animator;

    [Header("Player Jump and velocity")]
    [SerializeField] private float turnCamTime = 0.1f;
    [SerializeField] private float turnCamVelocity;
    [SerializeField] private float jumpRange = 1f;
    [SerializeField] private float timeBetweenJumps = 1.2f;
    private float nextTimeToJump = 0f;
    Vector3 velocity;
    [SerializeField] private Transform surfaceCheck;
    private bool onSurface;
    [SerializeField] private float surfaceDistance = 0.4f;
    [SerializeField] private LayerMask surfaceMask;

    [Header("Sounds")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip bloodSound;
    [SerializeField] private float bloodVol;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float deathVol;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        curHealth = playerHealth;
        healthBar.GiveFullHealth(playerHealth);
        audioSource = GetComponent<AudioSource>();
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
        if (animator.GetBool("Dying"))
            return;

        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; //get the angle we want the character to face after movement
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCamVelocity, turnCamTime); //to smoothen the angle change
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        if (direction.magnitude >= 0.1f)
        {

            //animator.SetBool("Idle", false);
            
            //TODO: disable shooting while running and disable running while aiming
            CheckSprint();
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //move player in direction of camera
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }

        else
        {
            PlayAnimation("Idle");
        }
    }

    private void Jump()
    {
        //TODO : Remove the bug that allows player to jump while in the "Jump" state (add jump time delay)  -- DONE 
        // TODO : disallow jumping while firing

        if (Input.GetButtonDown("Jump") && onSurface && Time.time >= nextTimeToJump)
        {
            PlayAnimation("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2f * gravity);
            nextTimeToJump = Time.time + timeBetweenJumps;
        }
    }

    private void CheckSprint()
    {
        if(Input.GetButton("Sprint") && onSurface)
        {
            speed = playerSprint;
            PlayAnimation("Running");
        }
        else
        {
            speed = playerWalk;
            PlayAnimation("Walk");
        }
    }

    public void playerHitDmg(float takeDmg)
    {
        curHealth -= takeDmg;
        healthBar.SetHealth(curHealth);
        PlaySound(bloodSound,bloodVol);
        StartCoroutine(PlayerDamageDisplay());

        if (curHealth <= 0 && !animator.GetBool("Dying"))
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        //TODO: death animation?? -- DONE
        StartCoroutine(PlayerDeathAnimation());
    }

    public IEnumerator PlayerDamageDisplay()
    {
        playerDmgSplash.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        playerDmgSplash.SetActive(false);

    }

    public IEnumerator PlayerDeathAnimation()
    {
        //TODO: add death sound -- DONE
        PlaySound(deathSound, deathVol);
        PlayAnimation("Dying");
        yield return new WaitForSeconds(3.667f);
        Destroy(gameObject, 0.5f);
        Menus.instance.ShowGameOver();
    }

    private void PlayAnimation(string anim)
    {
        if (anim.Equals("Jump"))
            animator.SetTrigger(anim);
        else
            animator.SetBool(anim, true);
        string[] arr = { "Idle", "Walk", "Running", "Jump", "Aim", "Shoot", "Reloading", "Rifle Walk","Shoot Walk","Punch","Dying"};
        foreach(string s in arr)
        {
            if (s.Equals(anim))
                continue;
            else if (s.Equals("Jump"))
            {
                animator.ResetTrigger(s);
            }
            else
            {
                animator.SetBool(s, false);
            }
        }
    }

    private void PlaySound(AudioClip clip, float vol)
    {
        audioSource.volume = vol;
        audioSource.PlayOneShot(clip);
    }

    public void IncreaseHealth()
    {
        curHealth = playerHealth;
    }
}
