using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerActivator : MonoBehaviour
{
    public Collider Activator;
    public List<GameObject> TargetGO = new List<GameObject>();
    public bool Once;
    private bool done = false;

    void OnTriggerEnter(Collider Col)
    {
        Debug.Log(Col + " y luego " + Activator);
        // check collider is target and task is undone
        if ((Col.GetComponent<Collider>() == Activator) && (!done))
        {
            foreach (GameObject GO in TargetGO)
                // check if enable or unable to change
                if (GO.activeInHierarchy)
                GO.SetActive(false);
                else
                GO.SetActive(true);
                // check if task is single time and mark as done
                if (Once)
                done = true;
        }
    }

}
