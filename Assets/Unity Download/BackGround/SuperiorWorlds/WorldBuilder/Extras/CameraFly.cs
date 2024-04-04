using UnityEngine;

namespace SuperiorWorlds
{
	public class CameraFly : MonoBehaviour
	{
	    public float movementSpeed = 10f;
	    public float rotationSpeed = 3f;
	    public float lookSpeed = 2f;
	    public float riseSpeed = 5f;
	    public float fallSpeed = 5f;
	
	    private float rotationX = 0f;
	    private float rotationY = 0f;
	
	    private void Update()
	    {
	        // Get the horizontal and vertical input axes
	        float horizontalInput = Input.GetAxis("Horizontal");
	        float verticalInput = Input.GetAxis("Vertical");
	
	        // Move the camera based on the input axes
	        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * movementSpeed * Time.deltaTime;
	        movement = transform.TransformDirection(movement);
	        transform.position += movement;
	
	        // Look around with the mouse
	        rotationX += Input.GetAxis("Mouse X") * lookSpeed;
	        rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
	        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Limit vertical rotation to avoid camera flipping
	
	        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
	
	        // Rise with Spacebar
	        if (Input.GetKey(KeyCode.Space))
	        {
	            Vector3 rise = Vector3.up * riseSpeed * Time.deltaTime;
	            transform.position += rise;
	        }
	        
	        // Fall with Left Shift
	        if (Input.GetKey(KeyCode.LeftShift))
	        {
	            Vector3 fall = Vector3.down * fallSpeed * Time.deltaTime;
	            transform.position += fall;
	        }
	    }
	}

}
