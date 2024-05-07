using UnityEngine;

public class CameraOrbitCollision : MonoBehaviour
{
    public Transform playerTransform; // The player object to orbit around
    public float distance = 10.0f; // The distance from the player object
    public float distanceScrollSpeed = 2.0f; // The speed at which to zoom in/out
    public float xSpeed = 120.0f; // The x rotation speed
    public float ySpeed = 120.0f; // The y rotation speed
    public float yMinLimit = -20f; // The minimum y rotation limit
    public float yMaxLimit = 80f; // The maximum y rotation limit
    public float xMinLimit = -360f; // The minimum x rotation limit
    public float xMaxLimit = 360f; // The maximum x rotation limit
    public float raycastOffset = 0.2f; // The offset used to avoid clipping through walls

    public LayerMask excludeLayers; // The layers to exclude from the raycast

    private float x = 0.0f; // The current x rotation
    private float y = 0.0f; // The current y rotation

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Update distance based on scroll wheel input
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance -= scroll * distanceScrollSpeed;
            distance = Mathf.Clamp(distance, 1.0f, Mathf.Infinity);

            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            y = ClampAngle(y, yMinLimit, yMaxLimit);
            x = ClampAngle(x, xMinLimit, xMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + playerTransform.position;

            RaycastHit hit;
            if (Physics.Linecast(playerTransform.position, position, out hit, ~excludeLayers))
            {
                position = hit.point + (hit.normal * raycastOffset);
            }

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
