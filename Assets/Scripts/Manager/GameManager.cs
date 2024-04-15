using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance;
    [SerializeField] RawImage frameChoice;
    public float timeScaleOriginal;
    public bool isPaused;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        timeScaleOriginal = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PausedGame()
    {
        isPaused = !isPaused;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void UnpauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisplayGunImage(Texture2D texture)
    {
        //References to Textures that are added in the GunSO on Unity
        {
            if (texture != null)
            {
                frameChoice.texture = texture;
            }
        }
    }
}
