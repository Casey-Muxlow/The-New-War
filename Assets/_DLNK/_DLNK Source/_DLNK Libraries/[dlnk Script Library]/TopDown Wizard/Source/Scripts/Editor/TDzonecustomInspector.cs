using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TDZone))]
public class TDzonecustomInspector : Editor
{
    public string UpdateTrigger = "Change Trigger Visible";

    public void Awake()
    {
        // Check trigger visibility
        TDZone tdZone = (TDZone)target;
        if (tdZone.TriggersVisibleOnEditor)
        {
            UpdateTrigger = "Visible Triggers";
        }
        else
            UpdateTrigger = "Invisible Triggers";
    }
    public override void OnInspectorGUI()
    {
        //Set New floor Name
        TDZone tdZone = (TDZone)target;
        tdZone.florname = GUILayout.TextField(tdZone.florname);
        //Button Create Floor
        if (GUILayout.Button("Create New Floor"))
        {
            tdZone.OnDo();
        }
        // Button Change Ceiling Materials
        if (GUILayout.Button("Change Ceiling Shadow Materials"))
        {
            tdZone.OnCeilingTint();
        }
        // Button Change Trigger Visibility
        if (GUILayout.Button(UpdateTrigger))
            if (!tdZone.TriggersVisibleOnEditor)
            {
                tdZone.TriggersVisibleOnEditor = true;
                UpdateTrigger = "Visible Triggers";
                tdZone.OnTrigUpdate();
            }
            else
            {
                tdZone.TriggersVisibleOnEditor = false;
                UpdateTrigger = "Invisible Triggers";
                tdZone.OnTrigUpdate();
            }
        if (GUILayout.Button("Turn Floors Mesh Static"))
            tdZone.OnStaticFloor();
        // Button Create Trigger
        if (GUILayout.Button("Create New Trigger"))
        {
            tdZone.OnTrigCreate();
            Selection.activeGameObject = tdZone.trigerSelected;
        }
        DrawDefaultInspector();
    }
}
