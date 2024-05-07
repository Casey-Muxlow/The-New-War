using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float floorHeight = 3f;   // Height of each floor
    public float speed = 5f;         // Speed of the elevator
    public int originalFloor = 1;    // Original floor number (1 is the ground floor)

    private Vector3 targetPosition;
    private int targetFloor = 1;      // Target floor number
    private bool isMoving = false;    // Flag to track if the elevator is currently moving
    private bool shouldReturn = false; // Flag to track if the elevator should return to the original position

    private int currentFloor;        // Current floor number

    public bool useOriginalFloor = true;    // Toggle option to use the original floor as the target

    private void Start()
    {
        currentFloor = CalculateCurrentFloor();
        SetTargetPosition();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // Move the elevator towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);

            // Check if the elevator has reached the target floor
            if (Mathf.Approximately(Vector3.Distance(transform.position, targetPosition), 0f))
            {
                // Arrived at the target floor, stop the elevator
                isMoving = false;
                currentFloor = CalculateCurrentFloor();

                // Check if the elevator should return to the original floor
                if (shouldReturn && currentFloor != originalFloor)
                {
                    int offset = originalFloor - currentFloor;
                    ChangeTargetFloor(targetFloor + offset);
                    shouldReturn = false;
                }
            }
          //  Debug.Log("Current floor is "+currentFloor);
        }
    }

    private void SetTargetPosition()
    {
        // Calculate the target position based on the target floor number and the original floor offset
        targetPosition = transform.position + Vector3.up * (targetFloor - originalFloor) * floorHeight;
    }

    private int CalculateCurrentFloor()
    {
        // Calculate the current floor based on the height of the transform and the original floor offset
        int calculatedFloor = Mathf.RoundToInt(transform.position.y / floorHeight);
        return calculatedFloor;
    }

    public int GetCurrentFloor()
    {
        return currentFloor;
    }

    public int GetOriginalFloor()
    {
        return originalFloor;
    }

    public void ChangeTargetFloor(int newTargetFloor)
    {
        // Check if the elevator is already at the target floor
        if (newTargetFloor == targetFloor)
            return;

        // Check if the elevator should return to the original position
        if (useOriginalFloor && newTargetFloor == originalFloor)
        {
            shouldReturn = true;
        }

        targetFloor = newTargetFloor;
        SetTargetPosition();
        isMoving = true;
    }
}
