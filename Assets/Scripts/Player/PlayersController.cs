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


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        HandleInput();
        MoveCharacter(input);
    }
}
