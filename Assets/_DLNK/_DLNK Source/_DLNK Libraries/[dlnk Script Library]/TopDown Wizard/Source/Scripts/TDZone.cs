using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class TDZone : MonoBehaviour
{
    // set up target floor for new triggers
    [Header("[NEW Trigger] Floor Target")]
    public float tritarg = 0f;

    // set new trigger size
    [Header("[NEW Trigger] Scale")]
    public Vector3 TrigerSize = new Vector3(2f,3f,2f);

    // floors list
    [System.Serializable]
    public class Floor
    {
        [HideInInspector]
        public string FloorName;
        public GameObject FloorGO;
        public float FloorHeight;
        public bool allwaysVisible = false;
        public GameObject StaticFloor;
        public GameObject DecoGO;
        public GameObject CShadowGO;
        public GameObject ActiveGO;
        public GameObject LightingGO;
        public GameObject ExteriorDeco;
    }
    [Header("[FLOORS]")]
    public List<Floor> Floors;

    // set new floor name
    [HideInInspector]
    public string florname = "Floor Name";

    // triggers list
    [System.Serializable]
    public class Trigger
    {
        [HideInInspector]
        public string Zname;
        public TDZoneTrigger ZTriger;
        public float TargetFloor;
        public bool isExit;
    }
    [Header("[TRIGGERS]")]
    public List<Trigger> Triggers;

    // set trigger parent object
    [HideInInspector]
    public GameObject trigfol;
    //changes required
    [HideInInspector]
    public bool updated;
    //save last trigger created to select it
    [HideInInspector]
    public GameObject trigerSelected;
    //save tdscene
    [HideInInspector]
    public TDScene tdscene;
    //switch triggers visibility oneditor
    [HideInInspector]
    public bool TriggersVisibleOnEditor = true;

    public void Start()
    {
        tdscene = GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>();
        if (Triggers == null)
            Triggers = new List<Trigger>();
        foreach (Floor flo in Floors)
            flo.CShadowGO.gameObject.SetActive(false);
    }
    public void Update()
    {
        foreach (Trigger tri in Triggers)
        {
            // check if any trigger deleted and remove from list
            if (tri.ZTriger == null)
                Triggers.Remove(tri);
            // get trigger properties
            tri.TargetFloor = tri.ZTriger.TargetFloor;
            tri.ZTriger.TargetFloor = tri.TargetFloor;
            tri.isExit = tri.ZTriger.exiTrigger;
            // update trigger names (gameobject)
            if (tri.ZTriger.exiTrigger)
                tri.ZTriger.name = "<Trigger> Exterior " + tri.ZTriger.TriggerName;
            else
                tri.ZTriger.name = "<Trigger> #" + tri.TargetFloor + " " + tri.ZTriger.TriggerName;
            // Name elements in list with trigger name
            tri.Zname = tri.ZTriger.TriggerName;
        }

        foreach (Floor flo in Floors)
        {
            //Check if any floor deleted and remove from list
            if (flo.FloorGO == null)
                Floors.Remove(flo);
            // Update elements in floor list
            flo.FloorName = flo.FloorGO.name;
        }
    }
    public void OnDo()
    {
        //Check if list is not initialized
        if (Floors == null)
        {
            Floors = new List<Floor>();
            //create trigger parent
            GameObject florT = new GameObject("Triggers");
            florT.transform.parent = this.transform;
            trigfol = florT;
        }

        //Create core floor childs
        GameObject florG = new GameObject(Floors.Count + " #" + florname);
        GameObject florS = new GameObject("StaticFloor");
        GameObject florD = new GameObject("Decoration");
        GameObject florC = new GameObject("Ceiling Shadow");
        florC.SetActive (false);
        GameObject florA = new GameObject("Active Elements");
        GameObject florL = new GameObject("Lighting");
        //Create aditional floor childs
        GameObject florF = new GameObject("Ground");
        GameObject florW = new GameObject("Walls");
        GameObject florE = new GameObject("Exterior Deco");

        //Set GOs in hierachy
        florG.transform.parent = this.transform;
        florG.transform.localPosition = new Vector3(0,0,0);
        florS.transform.parent = florG.transform;
        florS.transform.localPosition = new Vector3(0, 0, 0);
        florD.transform.parent = florG.transform;
        florD.transform.localPosition = new Vector3(0, 0, 0);
        florC.transform.parent = florG.transform;
        florC.transform.localPosition = new Vector3(0, 0, 0);
        florA.transform.parent = florG.transform;
        florA.transform.localPosition = new Vector3(0, 0, 0);
        florL.transform.parent = florG.transform;
        florL.transform.localPosition = new Vector3(0, 0, 0);
        florE.transform.parent = florG.transform;
        florE.transform.localPosition = new Vector3(0, 0, 0);
        florF.transform.parent = florS.transform;
        florF.transform.localPosition = new Vector3(0, 0, 0);
        florW.transform.parent = florS.transform;
        florW.transform.localPosition = new Vector3(0, 0, 0);

        //Add zone to group list
        Floor floorz = new Floor();
        floorz.FloorGO = florG;
        floorz.FloorHeight = Floors.Count;
        floorz.StaticFloor = florS;
        floorz.DecoGO = florD;
        floorz.CShadowGO = florC;
        floorz.ActiveGO = florA;
        floorz.LightingGO = florL;
        floorz.ExteriorDeco = florE;
        Floors.Add(floorz);
    }
    public void OnTrigCreate()
    {
        //Check if list is not initialized
        if (Triggers == null)
            Triggers = new List<Trigger>();

        //create trigger object
        GameObject zontrig = GameObject.CreatePrimitive(PrimitiveType.Cube);
        zontrig.transform.parent = trigfol.transform;
        zontrig.transform.localPosition = new Vector3 (0,(TrigerSize.y/2),0);
        zontrig.transform.localRotation = new Quaternion (0,0,0,100);
        zontrig.name = "ZoneTrigger";

        //Compose trigger object
        zontrig.AddComponent<TDZoneTrigger>();
        //Set collider trigger
        zontrig.GetComponent<BoxCollider>().isTrigger = enabled;
        //Set material
        zontrig.GetComponent<MeshRenderer>().material = tdscene.ZTriggerMat;
        zontrig.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //set TDZTrigger properties
        zontrig.GetComponent<TDZoneTrigger>().ParentZone = this;
        zontrig.GetComponent<TDZoneTrigger>().TargetFloor = tritarg;
        zontrig.transform.localScale = TrigerSize;

        //add to list
        Trigger trigz = new Trigger();
        trigz.TargetFloor = tritarg;
        trigz.ZTriger = zontrig.GetComponent<TDZoneTrigger>();
        Triggers.Add(trigz);

        //select trigger
        trigerSelected = zontrig;

    }
    public void OnTrigUpdate()
    {
        foreach (Trigger tri in Triggers)
        {
            // make triggers visible on editor
            if (TriggersVisibleOnEditor)
                tri.ZTriger.gameObject.GetComponent<Renderer>().enabled = true;
            else
                tri.ZTriger.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
    public void OnCeilingTint()
    {
        foreach (Floor flo in Floors)
        {
            foreach (Transform child in flo.CShadowGO.transform)
            {
                child.gameObject.isStatic = false;
                if (child.gameObject.GetComponent<Renderer>())
                child.gameObject.GetComponent<Renderer>().material = tdscene.CeilingShadow;
                foreach (Transform children in child)
                    {
                    children.gameObject.isStatic = false;
                    if (children.gameObject.GetComponent<Renderer>())
                        children.gameObject.GetComponent<Renderer>().material = tdscene.CeilingShadow;
                       foreach (Transform childs in children)
                    {
                        childs.gameObject.isStatic = false;
                        if (childs.gameObject.GetComponent<Renderer>())
                            childs.gameObject.GetComponent<Renderer>().material = tdscene.CeilingShadow;
                    }
                }
            }
        }
    }
    
    public void OnStaticFloor()
    {
        foreach (Floor flo in Floors)
        {
            foreach (Transform child in flo.StaticFloor.transform)
            {
                child.gameObject.isStatic = true;
                foreach (Transform children in child)
                {
                    children.gameObject.isStatic = true;
                    foreach (Transform childrens in children)
                    {
                        childrens.gameObject.isStatic = true;
                        foreach (Transform childreno in childrens)
                            childreno.gameObject.isStatic = true;
                    }
                }

            }
        }
    }
    public void FixedUpdate()
    {
        //TOP DOWN Floor Control

        if (updated && tdscene.tdEnabled)
        {
            foreach (Floor flo in Floors)
            {
                if (tdscene.isInTDZone)
                {
                    //unable static mesh, lights & active elements from upper plants
                    if (flo.FloorHeight > tdscene.ActiveFloor)
                    {
                        flo.StaticFloor.SetActive(false);
                        flo.ExteriorDeco.SetActive(false);
                        flo.LightingGO.SetActive(false);
                        flo.ActiveGO.SetActive(false);
                        flo.DecoGO.SetActive(false);
                        flo.CShadowGO.SetActive(true);
                    }
                    else
                    {
                        flo.StaticFloor.SetActive(true);
                        flo.ExteriorDeco.SetActive(true);
                        flo.LightingGO.SetActive(true);
                        flo.ActiveGO.SetActive(true);
                        flo.DecoGO.SetActive(true);
                        flo.CShadowGO.SetActive(false);
                    }
                    //enable ceiling shadow for actual plant
                    if (flo.FloorHeight == tdscene.ActiveFloor)
                    {
                        flo.CShadowGO.SetActive(true);
                        flo.LightingGO.SetActive(true);
                    }
                    else if (tdscene.OptimizeDeco)
                        flo.DecoGO.SetActive(false);
                    updated = false;
                }
                else
                {
                    flo.StaticFloor.SetActive(true);
                    flo.ExteriorDeco.SetActive(true);
                    flo.ActiveGO.SetActive(true);
                    flo.DecoGO.SetActive(false);
                    flo.CShadowGO.SetActive(false);
                    flo.LightingGO.SetActive(true);
                    updated = false;
                }
            }
        }
        // reactiva todas las plantas al salir del sistema topdown
        else if (updated)
        {
            foreach (Floor flo in Floors)
            {
                flo.StaticFloor.SetActive(true);
                flo.ExteriorDeco.SetActive(true);
                flo.LightingGO.SetActive(true);
                flo.ActiveGO.SetActive(true);
                flo.DecoGO.SetActive(true);
                flo.CShadowGO.SetActive(false);
                updated = false;
            }
        }
    }
}
