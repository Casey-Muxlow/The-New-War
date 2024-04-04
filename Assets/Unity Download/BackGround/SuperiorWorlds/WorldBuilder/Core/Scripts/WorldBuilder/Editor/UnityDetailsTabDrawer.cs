using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SuperiorWorlds {
	public class UnityDetailsTabDrawer : ITabDrawer {
        private WorldBuilder worldBuilder;
        private int selectedLayerIndex;

        public UnityDetailsTabDrawer(WorldBuilder worldBuilder, int selectedLayerIndex) {
            this.worldBuilder = worldBuilder;
            this.selectedLayerIndex = selectedLayerIndex;
        }

        public void DrawTab() {
            if (selectedLayerIndex == -1) return;

            EditorGUILayout.LabelField("Unity Details", StyleSheet.GetDictionaryTitleStyle(), GUILayout.Height(35f));

            WorldLayer selectedLayer = worldBuilder.worldLayers[selectedLayerIndex];
            List<UnityDetail> unityDetailsList = selectedLayer.unityDetails;

            DrawLayerDetailElements(unityDetailsList);
        }

        private void DrawLayerDetailElements<T>(List<T> elementsList) where T : LayerObject, new()
		{
			// Create a new GUIStyle with a 1px border
		    GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
		    boxStyle.border = new RectOffset(1, 1, 1, 1);
    		boxStyle.normal.background = StyleSheet.MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, 0.7f)); // Darker background color
				
			// Iterate through the elementsList and display the properties of each element
			for (int i = 0; i < elementsList.Count; i++)
			{
			    EditorGUILayout.BeginVertical(boxStyle); // Apply the new GUIStyle here
			    
		        UnityDetail unityDetail = elementsList[i] as UnityDetail;
		
		        // Show the Unity Detail-specific properties (name and minimum distance)
		        // EditorGUILayout.LabelField("Unity Detail", EditorStyles.boldLabel);
		
		        // Left Column (180px wide)
		        GUIStyle thumbnailColumnStyle = new GUIStyle(GUI.skin.box)
		        {
		            normal = new GUIStyleState() { background = StyleSheet.MakeTex(1, 1, new Color(0f, 0f, 0f, 0.75f)) },
		            stretchHeight = true
		        };
		
		        EditorGUILayout.BeginHorizontal(); // Start Horizontal Layout
		
		        EditorGUILayout.BeginVertical(GUILayout.Width(140));
		
		        int selectedPrefabIndex = GetPrefabIndexFromDetail(unityDetail);
				
				if (selectedPrefabIndex >= 0 && selectedPrefabIndex < Terrain.activeTerrain.terrainData.detailPrototypes.Length) 
				{
					DetailPrototype selectedDetailPrototype = GetDetailPrototypeFromIndex(selectedPrefabIndex);
					// Generate the thumbnail using the ThumbnailGenerator class
                    Texture2D prefabThumbnail = ThumbnailGenerator.Instance.GenerateDetailThumbnail(selectedDetailPrototype, worldBuilder.terrain);
					
                    GUILayout.Label(prefabThumbnail, GUILayout.Width(140), GUILayout.Height(140));

			        EditorGUILayout.EndVertical();
			
			        // Right Column (400px wide)
			        GUIStyle attributesColumnStyle = new GUIStyle(GUI.skin.box)
			        {
			            normal = new GUIStyleState() { background = StyleSheet.MakeTex(1, 1, new Color(0f, 0f, 0f, 0.75f)) },
			            stretchHeight = true
			        };
			
			        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
			
			        // Dropdown to select the prefab for Unity Details
					// unityDetail.name = EditorGUILayout.TextField("Name", unityDetail.name);
					selectedPrefabIndex = EditorGUILayout.Popup("Detail Prefab", selectedPrefabIndex, GetDetailPrefabNames());
			        unityDetail.detailPrototypeIndex = selectedPrefabIndex;
					
					// GUILayout.Space(10f); // Add some space below the header
			        unityDetail.minimumDistance = EditorGUILayout.FloatField(new GUIContent("Minimum Distance", unityDetail.minimumDistanceTooltip), unityDetail.minimumDistance);
					unityDetail.clusterRadius = EditorGUILayout.Slider("Cluster Radius", unityDetail.clusterRadius, 0.0f, 10.0f);
					unityDetail.clusterAmount = EditorGUILayout.IntSlider("Cluster Amount", unityDetail.clusterAmount, 0, 60);
			        // Add other UnityDetail properties here if needed
				}
		        else {
					// Skip drawing the element if the selectedPrefabIndex is out of range
            		EditorGUILayout.LabelField("Invalid or Missing Details on Terrain Object.");
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
					
		            // Call the RemoveDetailObject method on the selected WorldLayer
		            selectedLayer.RemoveDetailObject(worldBuilder, unityDetail);
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
		
		private int GetPrefabIndexFromDetail(UnityDetail unityDetail)
		{
		    Terrain terrain = worldBuilder.terrain;// Get the reference to your Terrain here;
		    if (terrain != null && terrain.terrainData != null)
		    {
		        DetailPrototype[] detailPrototypes = terrain.terrainData.detailPrototypes;
		        for (int i = 0; i < detailPrototypes.Length; i++)
		        {
		            // Compare the prototype index instead of the object reference
		            if (i == unityDetail.detailPrototypeIndex)
		            {
		                return i;
		            }
		        }
		    }
		
		    return -1;
		}
		
		private DetailPrototype GetDetailPrototypeFromIndex(int index)
		{
		    Terrain terrain = worldBuilder.terrain;
		    if (terrain != null && terrain.terrainData != null)
		    {
		        DetailPrototype[] detailPrototypes = terrain.terrainData.detailPrototypes;
		        if (index >= 0 && index < detailPrototypes.Length)
		        {
		            return detailPrototypes[index];
		        }
		    }
		
		    return null;
		}
		
		private string[] GetDetailPrefabNames()
		{
		    List<string> prefabNames = new List<string>();
		    Terrain terrain = worldBuilder.terrain; // Get the reference to your Terrain here;
		
		    if (terrain != null && terrain.terrainData != null)
		    {
		        DetailPrototype[] detailPrototypes = terrain.terrainData.detailPrototypes;
		        for (int i = 0; i < detailPrototypes.Length; i++)
		        {
		            DetailPrototype detailPrototype = detailPrototypes[i];
		            if (detailPrototype != null)
		            {
		                if (detailPrototype.usePrototypeMesh)
		                {
		                    // If it uses a mesh, add the mesh name to the list
		                    if (detailPrototype.prototype != null)
		                    {
		                        string prefabNameWithIndex = $"{i} - {detailPrototype.prototype.name}";
		                        prefabNames.Add(prefabNameWithIndex);
		                    }
		                }
		                else
		                {
		                    // If it uses a 2D texture, add the texture name to the list
		                    if (detailPrototype.prototypeTexture != null)
		                    {
		                        string prefabNameWithIndex = $"{i} - {detailPrototype.prototypeTexture.name}";
		                        prefabNames.Add(prefabNameWithIndex);
		                    }
		                }
		            }
		        }
		    }
		
		    return prefabNames.ToArray();
		}
	}
}