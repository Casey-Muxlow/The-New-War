using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Content;
//using Mono.Reflection;

[CustomEditor(typeof(TDScene))]
public class TDLevelSceneCustinsp : Editor
{
    Texture texture;
    public override void OnInspectorGUI()
    {
        //GUILayout.Space(20);
        //GUI.Label(new Rect(0, 0, 100, 20), new GUIContent("image", (Resources.Load("logoTdw") as Texture), "tooltip"));
        //texture = Resources.Load("logoTdw") as Texture;
        //EditorGUILayout.LabelField(new GUIContent(texture), GUILayout.ExpandWidth(true), GUILayout.Height(50));
        DrawDefaultInspector();

    }
}