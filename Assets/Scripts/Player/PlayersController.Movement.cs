using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayersController
{
    private void MoveCharacter(Vector2 input)
    {
        // Convert input to a 3D movement vector based on the player's forward and right directions.
        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;

        // Apply movement speed.
        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);

        // Apply gravity.
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        // Simulate gravity by applying a downward force.
        if (!characterController.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        else
        {
            // Reset velocity when grounded.
            velocity.y = -0.5f;
        }
    }

}
