using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TDActiveElement))]
public class TDActiveElementCustinspector : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        TDActiveElement element = (TDActiveElement)target;
        if (GUILayout.Button("Create Collider"))
        {
            element.OnCreateCollider();
            Selection.activeGameObject = element.TrigSelected;
        }


        DrawDefaultInspector();

        //Enable or unable Gos
        if (element.EnableGo)
        {
            //Add if required
            if (element.scriptEnable == null)
            {
                element.gameObject.AddComponent<ActiveEnableGo>();
                element.scriptEnable = element.GetComponent<ActiveEnableGo>();
            }
        }
        else
          //Delete if not required
          if (element.scriptEnable != null)
            DestroyImmediate(element.scriptEnable);

        //Switch materials
        if (element.SwitchMaterial)
        {
            //Add if required
            if (element.scriptMat == null)
            {
                element.gameObject.AddComponent<ActiveMaterial>();
                element.scriptMat = element.GetComponent<ActiveMaterial>();
            }
        }
        else
          //Delete if not required
          if (element.scriptMat != null)
            DestroyImmediate(element.scriptMat);

        //Do Transform
        if (element.DoTransform)
        {
            //Add if required
            if (element.scriptTrans == null)
            {
                element.gameObject.AddComponent<ActiveTransform>();
                element.scriptTrans = element.GetComponent<ActiveTransform>();
            }
        }
        else
          //Delete if not required
          if (element.scriptTrans != null)
            DestroyImmediate(element.scriptTrans);

        //Animate Go
        if (element.DoAnimate)
        {
            //Add if required
            if (element.scriptAnim == null)
            {
                element.gameObject.AddComponent<ActiveAnim>();
                element.scriptAnim = element.GetComponent<ActiveAnim>();
            }
        }
        else
          //Delete if not required
          if (element.scriptAnim != null)
            DestroyImmediate(element.scriptAnim);
    }
}
