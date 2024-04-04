using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SuperiorWorlds {
    public class UnityTreesTabDrawer : ITabDrawer {
	    private WorldBuilder worldBuilder;
	    private int selectedLayerIndex;
	
	    public UnityTreesTabDrawer(WorldBuilder worldBuilder, int selectedLayerIndex) {
	        this.worldBuilder = worldBuilder;
	        this.selectedLayerIndex = selectedLayerIndex;
	    }
	
	    public void DrawTab() {
	        if (selectedLayerIndex == -1) return;
	
	        EditorGUILayout.LabelField("Unity Trees", StyleSheet.GetDictionaryTitleStyle(), GUILayout.Height(35f));
	
	        WorldLayer selectedLayer = worldBuilder.worldLayers[selectedLayerIndex];
	        List<UnityTree> unityTreesList = selectedLayer.unityTrees;
	
	        DrawLayerTreeElements(unityTreesList);
	    }
	
	    private void DrawLayerTreeElements<T>(List<T> elementsList) where T : LayerObject, new()
		{
			// Create a new GUIStyle with a 1px border
		    GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
		    boxStyle.border = new RectOffset(1, 1, 1, 1);
    		boxStyle.normal.background = StyleSheet.MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 0.7f)); // Darker background color
				
			// Iterate through the elementsList and display the properties of each element
			for (int i = 0; i < elementsList.Count; i++)
			{
			    EditorGUILayout.BeginVertical(boxStyle); // Apply the new GUIStyle here
			
		        UnityTree unityTree = elementsList[i] as UnityTree;
		
		        // Show the Unity Tree-specific properties (name and minimum distance)
		        // EditorGUILayout.LabelField("Unity Tree", EditorStyles.boldLabel);
		
		        // Left Column (150px wide)
		        GUIStyle thumbnailColumnStyle = new GUIStyle(GUI.skin.box)
		        {
		            normal = new GUIStyleState() { background = StyleSheet.MakeTex(1, 1, new Color(0f, 0f, 0f, 0.75f)) },
		            stretchHeight = true
		        };
		
		        EditorGUILayout.BeginHorizontal(); // Start Horizontal Layout
		
		        EditorGUILayout.BeginVertical(GUILayout.Width(150));
		
		        int selectedPrefabIndex = GetPrefabIndexFromTree(unityTree);
				
				if (selectedPrefabIndex >= 0 && selectedPrefabIndex < Terrain.activeTerrain.terrainData.treePrototypes.Length) 
				{
					TreePrototype selectedTreePrototype = GetTreePrototypeFromIndex(selectedPrefabIndex);
	                // Generate the thumbnail using the ThumbnailGenerator class
                    Texture2D prefabThumbnail = ThumbnailGenerator.Instance.GenerateTreeThumbnail(selectedTreePrototype, worldBuilder.terrain);
					
                    GUILayout.Label(prefabThumbnail, GUILayout.Width(150), GUILayout.Height(150));
			
			        EditorGUILayout.EndVertical();
			
			        // Right Column (400px wide)
			        GUIStyle attributesColumnStyle = new GUIStyle(GUI.skin.box)
			        {
			            normal = new GUIStyleState() { background = StyleSheet.MakeTex(1, 1, new Color(0f, 0f, 0f, 0.75f)) },
			            stretchHeight = true
			        };
			
			        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
			
			        // Dropdown to select the prefab for Unity Trees
					// unityTree.name = EditorGUILayout.TextField("Name", unityTree.name);
			        selectedPrefabIndex = EditorGUILayout.Popup("Tree Prefab", selectedPrefabIndex, GetTreePrefabNames());
			        unityTree.treePrototypeIndex = selectedPrefabIndex;

					unityTree.minimumDistance = EditorGUILayout.FloatField(new GUIContent("Minimum Distance", unityTree.minimumDistanceTooltip), unityTree.minimumDistance);

			        GUILayout.Space(10f); // Add some space below the header
			        unityTree.minHeight = EditorGUILayout.FloatField("Min Height", unityTree.minHeight);
			        unityTree.maxHeight = EditorGUILayout.FloatField("Max Height", unityTree.maxHeight);
			
			        // Add other UnityTree properties here if needed
				}
		        else {
					// Skip drawing the element if the selectedPrefabIndex is out of range
            		EditorGUILayout.LabelField("Invalid or Missing Trees on Terrain Object.");
				}
		
		        // Right-justify the "Remove" button
		        EditorGUILayout.BeginHorizontal();
		        GUILayout.FlexibleSpace(); // Add flexible space before the button
		
		        if (GUILayout.Button("Remove", GUILayout.Width(100f)))
		        {
		            // Get the selected element to remove
    				T elementToRemove = elementsList[i];
					
					// Get the selected WorldLayer based on the selectedLayerIndex
		            WorldLayer selectedLayer = worldBuilder.worldLayers[selectedLayerIndex];
					
					// Remove the current element from the list
		            elementsList.RemoveAt(i);
		            
					// Call the RemoveTreeObject method on the selected WorldLayer
		            selectedLayer.RemoveTreeObject(worldBuilder, unityTree);		
		        }

		        EditorGUILayout.EndHorizontal();
				
		        EditorGUILayout.EndVertical();
		
		        EditorGUILayout.EndHorizontal(); // End Horizontal Layout

			    EditorGUILayout.EndVertical();
				// GUILayout.Space(20f); // Add some space below the header
			}

		
		    if (GUILayout.Button("+ Add", GUILayout.Width(100f)))
		    {
		        // Add a new element to the list
		        elementsList.Add(new T());
		    }
		}
		
				private int GetPrefabIndexFromTree(UnityTree unityTree)
		{
		    Terrain terrain = worldBuilder.terrain; // Get the reference to your Terrain here;
		    if (terrain != null && terrain.terrainData != null)
		    {
		        TreePrototype[] treePrototypes = terrain.terrainData.treePrototypes;
		        for (int i = 0; i < treePrototypes.Length; i++)
		        {
		            // Compare the prototype index instead of the object reference
		            if (i == unityTree.treePrototypeIndex)
		            {
		                return i;
		            }
		        }
		    }
		
		    return -1;
		}
		
		private TreePrototype GetTreePrototypeFromIndex(int index)
		{
		    Terrain terrain = worldBuilder.terrain;
		    if (terrain != null && terrain.terrainData != null)
		    {
		        TreePrototype[] treePrototypes = terrain.terrainData.treePrototypes;
		        if (index >= 0 && index < treePrototypes.Length)
		        {
		            return treePrototypes[index];
		        }
		    }
		
		    return null;
		}
		
		private string[] GetTreePrefabNames()
		{
		    List<string> prefabNames = new List<string>();
		    Terrain terrain = worldBuilder.terrain; // Get the reference to your Terrain here;
		
		    if (terrain != null && terrain.terrainData != null)
		    {
		        TreePrototype[] treePrototypes = terrain.terrainData.treePrototypes;
		        for (int i = 0; i < treePrototypes.Length; i++)
		        {
		            TreePrototype treePrototype = treePrototypes[i];
		            if (treePrototype != null && treePrototype.prefab != null)
		            {
		                string prefabNameWithIndex = $"{i} - {treePrototype.prefab.name}";
		                prefabNames.Add(prefabNameWithIndex);
		            }
		        }
		    }
		
		    return prefabNames.ToArray();
		}
	}
}