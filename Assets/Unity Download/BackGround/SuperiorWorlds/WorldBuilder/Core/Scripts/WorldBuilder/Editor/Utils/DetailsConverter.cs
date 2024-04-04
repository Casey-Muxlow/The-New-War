using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace SuperiorWorlds {
	public class DetailsConverter : EditorWindow
	{
	    [MenuItem("Window/World Builder/Details Converter", priority = 2)]

        [SerializeField]
        private static void ShowWindow()
        {
            GetWindow<DetailsConverter>("Detail Converter");
        }

        [SerializeField]
        private GameObject[] prefabs = new GameObject[10]; // Use an array of 10 elements

		private void OnGUI()
		{
		    GUILayout.Label("Prefabs to Process:", EditorStyles.boldLabel);
		    EditorGUILayout.BeginVertical(GUI.skin.box);
		    EditorGUI.indentLevel++;
		
		    for (int i = 0; i < prefabs.Length; i++)
		    {
		        prefabs[i] = EditorGUILayout.ObjectField("Prefab " + (i + 1), prefabs[i], typeof(GameObject), false) as GameObject;
		    }
		
		    EditorGUI.indentLevel--;
		    EditorGUILayout.EndVertical();
		
		    EditorGUILayout.Space();
		
		    if (GUILayout.Button("Process Prefabs"))
		    {
		        ProcessPrefabs();
		    }
		
		    EditorGUILayout.Space();
		
		    // Display a message with information about the prepared details
		    EditorGUILayout.HelpBox("Details are saved in the 'Converted Details' folder.", MessageType.Info);
			
			EditorGUILayout.Space();

			if (GUILayout.Button("Clear Selections"))
		    {
		        ClearSelections();
		    }
		}
		
		private void ClearSelections()
		{
		    for (int i = 0; i < prefabs.Length; i++)
		    {
		        prefabs[i] = null;
		    }
		}
				
		private void ProcessPrefabs()
		{
		    for (int i = 0; i < prefabs.Length; i++)
		    {
		        GameObject targetPrefab = prefabs[i];
		        if (targetPrefab == null)
		        {
		            continue; // Skip processing empty elements
		        }
		
		        LODGroup lodGroup = targetPrefab.GetComponentInChildren<LODGroup>();
		        GameObject firstLODChild = FindFirstLODChild(targetPrefab.transform);
		
		        LOD[] lods = null; // Declare lods outside the block
		
				if (lodGroup != null && firstLODChild != null)
				{
				    // LODGroup and LOD0 child found, convert the prefab with LODs
				    lods = lodGroup.GetLODs(); // Get the LODs of the LODGroup
				    // Duplicate the firstLODChild (Note: This won't place it in the scene)
				    GameObject duplicate = Instantiate(firstLODChild);
				
				    // Create the new prefab at the specified path
				    string prefabName = targetPrefab.name; // Use the name of the targetPrefab
				    if (targetPrefab.transform.parent != null)
				    {
				        prefabName = targetPrefab.transform.parent.name;
				    }
				
				    string directoryPath = "Assets/Converted Details";
				    if (!AssetDatabase.IsValidFolder(directoryPath))
				    {
				        AssetDatabase.CreateFolder("Assets", "Converted Details");
				    }
				
				    string path = "Assets/Converted Details/" + prefabName + ".prefab";
				    GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset(duplicate, path);
				
				    // Destroy the duplicate GameObject used for saving the prefab
				    DestroyImmediate(duplicate);
				
				    // Replace the original LOD child with the new prefab in the LODGroup
				    LOD[] newLods = new LOD[lods.Length];
				    for (int j = 0; j < lods.Length; j++)
				    {
				        if (lods[j].renderers != null && lods[j].renderers.Length > 0 && lods[j].renderers[0] == firstLODChild)
				        {
				            newLods[j].screenRelativeTransitionHeight = lods[j].screenRelativeTransitionHeight;
				            newLods[j].fadeTransitionWidth = lods[j].fadeTransitionWidth;
				            newLods[j].renderers = new Renderer[] { newPrefab.GetComponent<Renderer>() };
				        }
				        else
				        {
				            newLods[j] = lods[j];
				        }
				    }
				
				    lodGroup.SetLODs(newLods);
				    EditorUtility.SetDirty(lodGroup);
				
				    Debug.Log("LOD Group prefab converted and saved at: " + path);
				}
				else
				{
				    // No LODGroup or LOD0 child found, continue without an error message
				
				    // Duplicate the targetPrefab (Note: This won't place it in the scene)
				    GameObject duplicate = Instantiate(targetPrefab);
				
				    // Create the new prefab at the specified path
				    string prefabName = targetPrefab.name; // Use the name of the targetPrefab
				    if (targetPrefab.transform.parent != null)
				    {
				        prefabName = targetPrefab.transform.parent.name;
				    }
				
				    string directoryPath = "Assets/Converted Details";
				    if (!AssetDatabase.IsValidFolder(directoryPath))
				    {
				        AssetDatabase.CreateFolder("Assets", "Converted Details");
				    }
				
				    string path = "Assets/Converted Details/" + prefabName + ".prefab";
				    GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset(duplicate, path);
				
				    // Destroy the duplicate GameObject used for saving the prefab
				    DestroyImmediate(duplicate);
				
				    Debug.Log("Non-LOD prefab saved at: " + path);
				}
		    }
		
		    Debug.Log("All selected prefabs processed.");
		}
	
	    private GameObject FindFirstLODChild(Transform parent)
	    {
	        int childCount = parent.childCount;
	        for (int i = 0; i < childCount; i++)
	        {
	            Transform child = parent.GetChild(i);
	            if (child.name.Contains("LOD0"))
	            {
	                return child.gameObject;
	            }
	            else
	            {
	                GameObject foundChild = FindFirstLODChild(child);
	                if (foundChild != null)
	                    return foundChild;
	            }
	        }
	        return null;
	    }
	}
}