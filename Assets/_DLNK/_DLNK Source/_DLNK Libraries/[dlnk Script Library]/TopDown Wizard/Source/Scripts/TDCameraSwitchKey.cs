using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCameraSwitchKey : MonoBehaviour
{
    public Camera Cam1;
    public Camera Cam2;
    public KeyCode CamSwitchKey = KeyCode.F5;
    public bool UnableTopDownCam2 = false;
    public bool Cam2OnStart = false;

    public void Start()
    {
        if (Cam1.gameObject.activeInHierarchy && Cam2.gameObject.activeInHierarchy)
        {
            if (!Cam2OnStart)
            {
                GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().tdEnabled = true;
                if (GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().ActiveZone != null)
                    GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().ActiveZone.updated = true;
                Cam1.gameObject.SetActive(true);
                Cam2.gameObject.SetActive(false);
            }
            else if (UnableTopDownCam2)
            {
                Cam1.gameObject.SetActive(false);
                Cam2.gameObject.SetActive(true);
                GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().tdEnabled = false;
            }
            else
            {
                Cam1.gameObject.SetActive(false);
                Cam2.gameObject.SetActive(true);
            }
        }

    }
    void Update()
    {
        if (Input.GetKeyDown(CamSwitchKey))
        {
            if (Cam1.gameObject.activeInHierarchy)
            {
                Cam1.gameObject.SetActive(false);
                Cam2.gameObject.SetActive(true);
                if (UnableTopDownCam2)
                {
                    GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().tdEnabled = false;
                    if (GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().ActiveZone != null)
                        GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().ActiveZone.updated = true;
                }
            }
            else
            {
                Cam1.gameObject.SetActive(true);
                Cam2.gameObject.SetActive(false);
                if (UnableTopDownCam2)
                {
                    GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().tdEnabled = true;
                    if (GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().ActiveZone != null)
                        GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().ActiveZone.updated = true;
                }
            }
        }
    }
}
