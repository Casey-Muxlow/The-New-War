using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayersController : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 5f;
   

    [SerializeField] public float mouseSensitivity = 5f;
    [SerializeField] public float jumpHeight = 10f;

    [SerializeField] private float rotationLeftRight;
    [SerializeField] private float verticalRotation;
    [SerializeField] private float forwardspeed;
    [SerializeField] private float sideSpeed;
    [SerializeField] private Vector3 speedCombined;
    [SerializeField] private CharacterController playerController;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Animator animator;

    private Vector3 verticalVelocity;
    private float runLayerWeight = 0f;

    public bool IsDead = false;

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        playerController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        Movement();
        Animation();
        Jump();
    }

    private void Movement()
    {
        rotationLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotationLeftRight, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -60f, 60f);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        forwardspeed = Input.GetAxis("Vertical") * movementSpeed;
        sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            forwardspeed *= 2f;
            runLayerWeight = 1f;
        }
        else
        {
            runLayerWeight = 0f;
            animator.SetBool("NoWeaponRunningFwd", false);
            animator.SetBool("NoWeaponStrafeLeft", false);
            animator.SetBool("NoWeaponStrafeRight", false);
            animator.SetBool("NoWeaponRunLeft", false);
            animator.SetBool("NoWeaponRunRight", false);
            animator.SetBool("NoWeaponRunBack", false);
            animator.SetBool("NoWeaponRunBwdLeft", false);
            animator.SetBool("NoWeaponRunBwdRight", false);
        }

        animator.SetLayerWeight(animator.GetLayerIndex("Run Layer"), runLayerWeight);

        verticalVelocity.y += Physics.gravity.y * Time.deltaTime;

        speedCombined = new Vector3(sideSpeed, verticalVelocity.y, forwardspeed);

        speedCombined = transform.rotation * speedCombined;

        playerController.Move(speedCombined * Time.deltaTime);
    }

    private void Jump()
    {
        if (playerController.isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity.y = jumpHeight;
        }
    }

    private void Animation()
    {
        //BEGINNING OF NO WEAPON ANIMATIONS!!!

        if(runLayerWeight == 1f)
        {
            
            // Check for player input (e.g., pressing the "W" key)
            bool isRunningFwd = Input.GetKey(KeyCode.W) || Input.GetAxis("ControllerHorizontal") > 0f;

            // Set the "IsRunningFwd" parameter in the Animator Controller
            animator.SetBool("NoWeaponRunningFwd", isRunningFwd);

            bool isRunningLeft = Input.GetKey(KeyCode.A);
            animator.SetBool("NoWeaponRunLeft", isRunningLeft);
            if(isRunningLeft)
            {
                speedCombined *= 3f;
            }
            if(!isRunningLeft)
            {
                speedCombined /= 3f;
            }

            bool isRunningRight = Input.GetKey(KeyCode.D);
            animator.SetBool("NoWeaponRunRight", isRunningRight);
            if(isRunningRight)
            {
                speedCombined *= 3f;
            }
            if(!isRunningRight)
            {
                speedCombined /= 3f;
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                animator.SetBool("NoWeaponRunLeft", false);
                animator.SetBool("NoWeaponRunningFwd", false);
                animator.SetBool("NoWeaponStrafeLeft", true);
                
            }
            else
            {
                animator.SetBool("NoWeaponStrafeLeft", false);
                
            }


            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                animator.SetBool("NoWeaponRunningFwd", false);
                animator.SetBool("NoWeaponRunRight", false);
                animator.SetBool("NoWeaponStrafeRight", true);
                
            }
            else
            {
                animator.SetBool("NoWeaponStrafeRight", false);
            }

            
            // Check for player input (e.g., pressing the "D" key)
            bool RunBack = Input.GetKey(KeyCode.S) || Input.GetAxis("ControllerHorizontal") < 0f;

            // Set the "IsRunning" parameter in the Animator Controller
            animator.SetBool("NoWeaponRunBack", RunBack);

            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                animator.SetBool("NoWeaponRunBack", false);
                animator.SetBool("NoWeaponRunLeft", false);
                animator.SetBool("NoWeaponRunBwdLeft", true);
            }
            else
            {
                animator.SetBool("NoWeaponRunBwdLeft", false);
            }

            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                animator.SetBool("NoWeaponRunBack", false);
                animator.SetBool("NoWeaponRunRight", false);
                animator.SetBool("NoWeaponRunBwdRight", true);
            }
            else
            {
                animator.SetBool("NoWeaponRunBwdRight", false);
            }

        }
        else
        {
            bool isWalkingFwd = Input.GetKey(KeyCode.W);
            animator.SetBool("IsWalkingFwd", isWalkingFwd);

            bool isWalkingBwd = Input.GetKey(KeyCode.S);
            animator.SetBool("IsWalkingBwd", isWalkingBwd);

            bool isWalkingRight = Input.GetKey(KeyCode.D);
            animator.SetBool("IsWalkingRight", isWalkingRight);

            bool isWalkingLeft = Input.GetKey(KeyCode.A);
            animator.SetBool("IsWalkingLeft", isWalkingLeft);
        }

        //END OF NO WEAPON ANIMATIONS.



    }
}
