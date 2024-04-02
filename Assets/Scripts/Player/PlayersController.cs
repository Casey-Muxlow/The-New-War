using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayersController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] public float movementSpeed = 5f;
    [SerializeField] public float gravity = 9.81f;
    private Vector3 input;
    private Vector3 velocity;

    [SerializeField] private Animator animator;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        HandleInput();
        MoveCharacter(input);

        // Check for player input (e.g., pressing the "W" key)
        bool isRunningFwd = Input.GetKey(KeyCode.W);

        // Set the "IsRunningFwd" parameter in the Animator Controller
        animator.SetBool("IsRunningFwd", isRunningFwd);



        // Check for player input (e.g., pressing the "D" key)
        bool RunBack = Input.GetKey(KeyCode.S);

        // Set the "IsRunning" parameter in the Animator Controller
        animator.SetBool("RunBack", RunBack);
    }
}
