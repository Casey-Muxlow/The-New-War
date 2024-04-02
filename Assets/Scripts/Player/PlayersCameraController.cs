using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersCameraController : MonoBehaviour
{
    [SerializeField] public int sensitivity;
    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invertY;

    [SerializeField] AudioSource aud;

    [SerializeField] Transform characterTransform;
    [SerializeField] Transform headTransform;

    private float xRot;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        switch (invertY)
        {
            case true:
                xRot += mouseY;
                break;
            case false:
                xRot -= mouseY;
                break;
        }

        xRot = Mathf.Clamp(xRot, lockVertMin, lockVertMax);
        transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        
        //Rotate the character
        if(characterTransform != null)
        {
            characterTransform.Rotate(Vector3.up * mouseX);
        }

        //if (gameManager.instance.isPaused)
        //{
        //    aud.Pause();
        //}
        //else
        //{
        //    aud.UnPause();
        //}
    }
}
