using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayersController : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 5f;
   

    [SerializeField] public float mouseSensitivity = 5f;
    [SerializeField] public float jumpSpeed = 10f;

    [SerializeField] private float rotationLeftRight;
    [SerializeField] private float verticalRotation;
    [SerializeField] private float forwardspeed;
    [SerializeField] private float sideSpeed;
    [SerializeField] private float verticalVelocity;
    [SerializeField] private Vector3 speedCombined;
    [SerializeField] private CharacterController playerController;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Animator animator;


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
        }

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        speedCombined = new Vector3(sideSpeed, verticalVelocity, forwardspeed);

        speedCombined = transform.rotation * speedCombined;

        playerController.Move(speedCombined * Time.deltaTime);
    }

    private void Jump()
    {
        if (playerController.isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = jumpSpeed;
        }
    }

    private void Animation()
    {
        // Check for player input (e.g., pressing the "W" key)
        bool isRunningFwd = Input.GetKey(KeyCode.W) || Input.GetAxis("ControllerHorizontal") > 0f;

        // Set the "IsRunningFwd" parameter in the Animator Controller
        animator.SetBool("IsRunningFwd", isRunningFwd);



        // Check for player input (e.g., pressing the "D" key)
        bool RunBack = Input.GetKey(KeyCode.S) || Input.GetAxis("ControllerHorizontal") < 0f;

        // Set the "IsRunning" parameter in the Animator Controller
        animator.SetBool("RunBack", RunBack);


        
    }
}
