using UnityEngine;

public class ObjectRemover : MonoBehaviour
{
    public MonoBehaviour scriptToRemove; // Reference to the script to check for

    private void OnTriggerEnter(Collider other)
    {
    Debug.Log("Trigger Entered");
        // Check if the collided object has the specified script attached
        if (other.gameObject.TryGetComponent(scriptToRemove.GetType(), out var scriptComponent))
        {
            // Remove the collided object from the scene
            Destroy(other.gameObject);
        }
    }
}
