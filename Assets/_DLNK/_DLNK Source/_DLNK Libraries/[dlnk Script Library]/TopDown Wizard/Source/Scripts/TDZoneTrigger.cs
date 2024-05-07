using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDZoneTrigger : MonoBehaviour
{
    public TDZone ParentZone;
    public string TriggerName;
    public float TargetFloor;
    public bool exiTrigger;
    private TDScene tdscene;
    // Start is called before the first frame update
    void Start()
    {
        //set scene manager
        tdscene = GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>();
        //make object invisible
        if (!tdscene.VisibleTriggers)
        this.gameObject.GetComponent<Renderer>().enabled = false;
        //make collider trigger (for if the flies xD)
        this.gameObject.GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider trig)
    {
        // Check if player has enter collider
        if ((trig.GetComponent<Collider>() == tdscene.PlayerChar) & !exiTrigger)
        {
            //check if it's entering building for update
            if ((tdscene.ActiveZone == null) & tdscene.ActiveFloor == 0f)
                ParentZone.updated = true;

            //warn tdscene about changes
            tdscene.isInTDZone = true;
            tdscene.ActiveZone = ParentZone;
            tdscene.ActiveFloor = TargetFloor;
        }
        else if (exiTrigger)
        {
            //warn tdscene & parent zone about exit
            tdscene.isInTDZone = false;
            ParentZone.updated = true;
            tdscene.ActiveZone = null;
            tdscene.ActiveFloor = 0f;
        }
    }
}
