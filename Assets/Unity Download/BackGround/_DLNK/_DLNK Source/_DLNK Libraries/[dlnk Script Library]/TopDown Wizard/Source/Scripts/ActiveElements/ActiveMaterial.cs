using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMaterial : MonoBehaviour
{
    [System.Serializable]
    public class Target
    {
        public GameObject TargetGO;
        public Material MaterialTarget;
        [HideInInspector]
        public Material tmpMaterial;
    }
    public List<Target> Targets;
    public bool OverrideMatTarget;
    public bool Switched = false;
    public Material OverrideMaterial;
    public bool _localAuto = false;

    private TDActiveElement activeParent;
    private bool _waiting = false;

    void Start()
    {
        // find reference for vars
        activeParent = this.GetComponent<TDActiveElement>();
        foreach (Target tar in Targets)
        {
            tar.tmpMaterial = tar.TargetGO.GetComponent<Renderer>().material;
        }

        // check if global auto enabled
        if (activeParent.Automatic)
            _localAuto = true;
    }

    void Update()
    {
        // check if character hit collider
        if (activeParent.ActiveCollider.actived)
        {
            // check if character has activated action
            if ((_localAuto && !_waiting) || Input.GetKeyDown(activeParent.ActiveElementKey))
            {
                if (!Switched)
                {
                    foreach (Target tar in Targets)
                    {
                        if (OverrideMatTarget)
                            tar.TargetGO.GetComponent<Renderer>().material = OverrideMaterial;
                        else
                            tar.TargetGO.GetComponent<Renderer>().material = tar.MaterialTarget;
                    }
                    _waiting = true;
                    Switched = true;
                }
                else
                {
                    foreach (Target tar in Targets)
                    {
                            tar.TargetGO.GetComponent<Renderer>().material = tar.tmpMaterial;
                    }
                    _waiting = true;
                    Switched = false;
                }
            }
        }
        // check if character is leaving scene with automode
        else if (_localAuto && activeParent.ActiveCollider.hasexit && _waiting)
        {
            if (activeParent.AutoOnExit)
            {
                if (!Switched)
                {
                    foreach (Target tar in Targets)
                    {
                        if (OverrideMatTarget)
                            tar.TargetGO.GetComponent<Renderer>().material = OverrideMaterial;
                        else
                            tar.TargetGO.GetComponent<Renderer>().material = tar.MaterialTarget;
                    }
                    _waiting = false;
                    Switched = true;
                }
                else
                {
                    foreach (Target tar in Targets)
                    {
                        tar.TargetGO.GetComponent<Renderer>().material = tar.tmpMaterial;
                    }
                    _waiting = false;
                    Switched = false;
                }
            }
            else _waiting = false;
        }
    }
}
