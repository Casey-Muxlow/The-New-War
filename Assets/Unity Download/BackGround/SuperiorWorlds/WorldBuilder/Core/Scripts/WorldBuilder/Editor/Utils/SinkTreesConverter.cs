using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SuperiorWorlds
{
    public class SinkTreesConverter : ScriptableObject 
    {

        public GameObject[] prefabs = new GameObject[10];
        public float sinkAmount = 0.25f;

        // Method to handle the conversion
        public void ConvertTrees(GameObject[] prefabs, float[] sinkAmounts)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                GameObject prefab = prefabs[i];

                if (prefab != null)
                {
                    if (HasLODGroupOrChildObjects(prefab))
                    {
                        GameObject convertedPrefab = ConvertTreePrefab(prefab, sinkAmount);
						
						// Create the "Converted Trees" directory if it doesn't exist
                        string directoryPath = "Assets/Converted Trees";
                        if (!AssetDatabase.IsValidFolder(directoryPath))
                        {
                            AssetDatabase.CreateFolder("Assets", "Converted Trees");
                        }
						
                        // Create and save a new prefab with the modified tree
                        string path = "Assets/Converted Trees/" + prefab.name + "_Converted.prefab";
                        PrefabUtility.SaveAsPrefabAsset(convertedPrefab, path);

                        // Clean up the instantiated convertedPrefab
                        DestroyImmediate(convertedPrefab);
                    }
                    else
                    {
                        Debug.LogError($"Prefab '{prefab.name}' must have LODs or child objects.");
                    }
                }
            }
        }

        // Helper method to check if a prefab has LODGroup or child objects
        private bool HasLODGroupOrChildObjects(GameObject prefab)
        {
            LODGroup lodGroup = prefab.GetComponent<LODGroup>();
            if (lodGroup != null)
            {
                return true;
            }

            if (prefab.transform.childCount > 0)
            {
                return true;
            }

            return false;
        }

		// Helper method to convert the tree prefab and lower the first level of child objects
		private GameObject ConvertTreePrefab(GameObject prefab, float sinkAmount)
		{
		    // Instantiate the original prefab in memory
		    GameObject originalPrefab = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
		
		    // Traverse through the child objects and adjust their positions
		    Transform[] allChildTransforms = originalPrefab.GetComponentsInChildren<Transform>();
		    foreach (Transform child in allChildTransforms)
		    {
		        if (child == originalPrefab.transform)
		            continue; // Skip the root object (prefab itself)
		
		        Vector3 newPosition = child.position;
		        newPosition.y -= sinkAmount;
		        child.position = newPosition;
		    }
		
		    // Save the converted prefab and connect it properly to preserve the LODGroup
		    string path = "Assets/Converted Trees/" + prefab.name + "_Converted.prefab";
		    GameObject convertedPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(originalPrefab, path, InteractionMode.UserAction);
		
		    // Destroy the instantiated prefab from memory
		    DestroyImmediate(originalPrefab);
		
		    return convertedPrefab;
		}
    }

    public class SinkTreesConverterWindow : EditorWindow
    {
        private GameObject[] prefabs = new GameObject[10];
        private float sinkAmount = 0.25f;

        [MenuItem("Window/World Builder/Sink Trees Converter", priority = 3)]
        private static void ShowSinkTreesConverterWindow()
        {
            GetWindow<SinkTreesConverterWindow>("Sink Trees Converter");
        }

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

            sinkAmount = EditorGUILayout.FloatField("Sink Amount", sinkAmount);

            EditorGUILayout.Space();

            if (GUILayout.Button("Process Prefabs"))
            {
                ProcessPrefabs();
            }
			
			EditorGUILayout.Space();
		
		    // Display a message with information about the prepared details
		    EditorGUILayout.HelpBox("Trees are saved in the 'Converted Trees' folder.", MessageType.Info);
			
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
		    if (prefabs == null || prefabs.Length == 0)
		    {
		        Debug.LogError("Prefab list is empty. Add prefabs to process.");
		        return;
		    }
		
		    if (sinkAmount <= 0f)
		    {
		        Debug.LogError("Sink Amount must be greater than 0.");
		        return;
		    }
		
		    // Create the "Converted Trees" directory if it doesn't exist
		    string directoryPath = "Assets/Converted Trees";
		    if (!AssetDatabase.IsValidFolder(directoryPath))
		    {
		        AssetDatabase.CreateFolder("Assets", "Converted Trees");
		    }
		
		    // Convert the trees by passing prefabs and sinkAmounts
		    SinkTreesConverter sinkTreesConverter = ScriptableObject.CreateInstance<SinkTreesConverter>();
		    sinkTreesConverter.ConvertTrees(prefabs, CreateSinkAmountsArray(prefabs.Length, sinkAmount));
		
		    EditorGUILayout.HelpBox("Trees have been processed and saved as new prefabs.", MessageType.Info);
		}


		
		// Helper method to create an array of sinkAmounts
        private float[] CreateSinkAmountsArray(int length, float sinkAmount)
        {
            float[] sinkAmounts = new float[length];
            for (int i = 0; i < length; i++)
            {
                sinkAmounts[i] = sinkAmount;
            }
            return sinkAmounts;
        }
    }
}
