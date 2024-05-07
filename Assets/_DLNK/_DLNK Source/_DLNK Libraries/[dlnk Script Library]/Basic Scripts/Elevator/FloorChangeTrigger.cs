using UnityEngine;

public class FloorChangeTrigger : MonoBehaviour
{
    public Elevator elevator;                   // Reference to the Elevator script
    public int targetFloor = 2;                 // Target floor number to change to
    public KeyCode floorChangeKey = KeyCode.E;  // Key used to trigger the floor change
    public bool useOriginalFloor = true;        // Toggle option to use the original floor as the target

    private bool playerInRange = false;         // Indicates if the player is within the trigger
    private int originalFloor;                  // Original floor number
    private int currentTargetFloor;             // Current target floor number

    private void Start()
    {
        originalFloor = elevator.GetOriginalFloor();
        currentTargetFloor = targetFloor;

        
             // Disable the MeshRenderer component on start
             GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press '" + floorChangeKey.ToString() + "' to change floor.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left the trigger.");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(floorChangeKey))
        {
            int currentFloor = elevator.GetCurrentFloor();

            if (useOriginalFloor && currentFloor == currentTargetFloor)
            {
                currentTargetFloor = (currentFloor == originalFloor) ? targetFloor : originalFloor;
                elevator.ChangeTargetFloor(currentTargetFloor);
            }
            else
            {
                elevator.ChangeTargetFloor(currentTargetFloor);
            }

            Debug.Log("Floor changed to " + currentTargetFloor.ToString());
        }
    }
}
