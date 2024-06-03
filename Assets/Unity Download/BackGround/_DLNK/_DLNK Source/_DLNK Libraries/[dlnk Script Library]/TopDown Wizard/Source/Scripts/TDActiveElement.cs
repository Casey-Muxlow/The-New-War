using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDActiveElement : MonoBehaviour
{
    [Header("[Active Element Properties]")]
    public bool UseTopDown = true;
    public KeyCode ActiveElementKey = KeyCode.E;
    public ActiveColGo ActiveCollider;
    public bool Automatic;
    public bool AutoOnExit = true;
    [Header("[Active Actions]")]
    public bool EnableGo;
    public ActiveEnableGo scriptEnable;
    public bool SwitchMaterial;
    public ActiveMaterial scriptMat;
    public bool DoTransform;
    public ActiveTransform scriptTrans;
    public bool DoAnimate;
    public ActiveAnim scriptAnim;
    private TDScene tdscene;
    [HideInInspector]
    public GameObject TrigSelected;


    // Start is called before the first frame update
    void Start()
    {
        //Secure Collider as trigger
        ActiveCollider.gameObject.GetComponent<Collider>().isTrigger = true;

        //Check if TopDown exists in the scene
        if (GameObject.FindWithTag("TdLevelManager") == null)
            UseTopDown = false;

        //Link TopDown control if enabled
            if (UseTopDown)
        {
            // find scene control component
            tdscene = GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>();

            // find keycode override in scene settings
            ActiveElementKey = tdscene.ActiveKey;

            //Collider object visibility
            if (!tdscene.VisibleTriggers)
                ActiveCollider.GetComponent<Renderer>().enabled = false;
            else
                ActiveCollider.GetComponent<Renderer>().enabled = true;
        }
        else
            //Collider object invisible
            ActiveCollider.GetComponent<Renderer>().enabled = false;
    }

    public void OnCreateCollider()
    {
        // Create object
        GameObject activcol = GameObject.CreatePrimitive(PrimitiveType.Cube);
        activcol.transform.parent = this.transform;
        activcol.transform.position = this.transform.position;
        activcol.name = "[ColliderActive]";
        activcol.AddComponent<ActiveColGo>();
        activcol.GetComponent<ActiveColGo>().activeParent = this;

        //Set material
        activcol.GetComponent<MeshRenderer>().material = GameObject.FindWithTag("TdLevelManager").GetComponent<TDScene>().ActiveMat;
        activcol.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        //Set in ActiveElement
        ActiveCollider = activcol.GetComponent<ActiveColGo>();

        //Secure Collider as trigger
        ActiveCollider.gameObject.GetComponent<Collider>().isTrigger = true;

        //select trigger
        TrigSelected = activcol;
    }
}
