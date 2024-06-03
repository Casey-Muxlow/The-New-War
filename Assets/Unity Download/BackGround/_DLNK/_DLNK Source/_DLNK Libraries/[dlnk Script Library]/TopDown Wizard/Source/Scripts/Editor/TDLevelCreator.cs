using System.Collections;
using UnityEngine;
using UnityEditor;

public class TDLevelCreator : ScriptableWizard
{
    public string LevelName = "Level Name";
    public int ZoneGroups = 1;
    private int _enum;

    [MenuItem ("Window/DLNK/TopDownWizzard/LevelCreator")]

    static void LevelCreatorWizzard()
    {
        ScriptableWizard.DisplayWizard<TDLevelCreator>("Level Creator", "Create Level", "Update Selected");
    }
    private void OnWizardCreate()
    {
        //set or create "TdLevelManager" tag
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProxy = tagManager.FindProperty("tags");
        string tagString = "TdLevelManager";
        //check if tag exists
        bool found = false;
        for (int i = 0; i < tagsProxy.arraySize; i++)
        {
            SerializedProperty t = tagsProxy.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tagString)) { found = true; break; }
        }
        //create if it doesnt
        if (!found)
        {
            tagsProxy.InsertArrayElementAtIndex(0);
            SerializedProperty sp = tagsProxy.GetArrayElementAtIndex(0);
            sp.stringValue = tagString;
            //Debug.Log("Tag: " + tagString + " has been added");
            tagManager.ApplyModifiedProperties();
        }
     

        //create level object
        GameObject levelGO = new GameObject(LevelName);
        levelGO.AddComponent<TDScene>();
        TDLevel tDLevelComponent = levelGO.AddComponent<TDLevel>();
        while (_enum < ZoneGroups)
        {
            levelGO.GetComponent<TDLevel>().OnDo();
            _enum++;
        }

        //edit level object
        levelGO.tag = tagString;

        //select level
        Selection.activeGameObject = levelGO;
    }
}
