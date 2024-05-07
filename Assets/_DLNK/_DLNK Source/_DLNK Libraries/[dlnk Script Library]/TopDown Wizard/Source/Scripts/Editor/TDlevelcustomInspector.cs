using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TDLevel))]
public class TDlevelcustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        TDLevel tdLevel = (TDLevel)target;
        tdLevel.zongrup = GUILayout.TextField(tdLevel.zongrup);
        if (GUILayout.Button("Create New ZoneGroup"))
        {
            tdLevel.OnDo();
        }

        DrawDefaultInspector();
    }
}

