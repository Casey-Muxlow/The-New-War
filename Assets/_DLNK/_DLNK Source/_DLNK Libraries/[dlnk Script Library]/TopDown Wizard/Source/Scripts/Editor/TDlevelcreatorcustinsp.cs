using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Content;

[CustomEditor(typeof(TDLevelCreator))]
public class TDLevelCreatorCustInsp : Editor
{
    Texture2D texture;
    public override void OnInspectorGUI()
    {
        GUILayout.Box("logoTdw.png");
        texture = (Texture2D)Resources.Load("logoTdw.png");
        EditorGUI.DrawPreviewTexture(new Rect(500, 200, 100, 100), texture);
        DrawDefaultInspector();

    }
}
