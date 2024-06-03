using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TDGroup))]
public class TDgroupcustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        TDGroup tdGroup = (TDGroup)target;
        tdGroup.zonname = GUILayout.TextField(tdGroup.zonname);
        if (GUILayout.Button("Create New Zone"))
        {
            tdGroup.OnDo();
        }

        DrawDefaultInspector();
    }
}
