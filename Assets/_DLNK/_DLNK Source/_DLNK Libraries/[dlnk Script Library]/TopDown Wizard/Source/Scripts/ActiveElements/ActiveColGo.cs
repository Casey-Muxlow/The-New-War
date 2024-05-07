using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveColGo : MonoBehaviour
{
    public TDActiveElement activeParent;
    public Collider PlayCol;
    [HideInInspector]
    public bool actived;
    [HideInInspector]
    public bool hasexit;

    private void Start()
    {
        //find player in scene
        if (activeParent.UseTopDown)
            PlayCol = GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().PlayerChar;
        else 
            PlayCol = GameObject.FindWithTag("Player").GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider trig)
    {
        //find player in scene if required
        if (PlayCol == null)
        {
            if (activeParent.UseTopDown)
                PlayCol = GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().PlayerChar;
            else
                PlayCol = GameObject.FindWithTag("Player").GetComponent<Collider>();
        }
        //Debug.Log("Player is " + PlayCol);
        //Check character in range and keycode pressed or automatic to start action
        if (trig.GetComponent<Collider>() == PlayCol)
        {
            hasexit = false;
            actived = true;
        }
    }
    void OnTriggerExit(Collider trig)
    {
        if (trig.GetComponent<Collider>() == PlayCol)
        {
            actived = false;
            hasexit = true;
        }

    }
}
