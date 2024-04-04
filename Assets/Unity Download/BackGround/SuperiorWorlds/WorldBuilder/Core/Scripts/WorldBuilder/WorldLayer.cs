using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;		

namespace SuperiorWorlds
{
    [Serializable]
    public class WorldLayer
    {
        // The name of the layer, with a default value of "New Layer"
        public string name = "New Layer";

        // The PNG heatmap selector for the layer
        public Texture2D heatmapTexture;

	    // The minimum threshold for the layer between 0 and 1
	    public float minimumThreshold = 0.5f;
	
	    // The maximum threshold for the layer between 0 and 1
	    public float maximumThreshold = 1.0f;
		
		// Additive Mode flag, defaults to false
        public bool additiveMode = false;
		
		// List to store Unity Trees in the layer
        [SerializeField] public List<UnityTree> unityTrees = new List<UnityTree>();

        // List to store Unity Details in the layer
        [SerializeField] public List<UnityDetail> unityDetails = new List<UnityDetail>();
		
		// Add the LayerConfig struct here at the class level and make it public
        [Serializable]
        public struct LayerConfig
        {
            public Texture2D heatmapTexture;
            public Bounds terrainBounds;
        	public float minimumThreshold;
        	public float maximumThreshold;
            public bool additiveMode;
        }
		
		// List to keep track of LayerObjects assigned to this layer
    	private List<LayerObject> assignedObjects = new List<LayerObject>();

		// This method is called whenever a serialized property in this script is changed in the Inspector.
        private void OnValidate()
        {
            // Perform actions in response to property changes here.
            // Debug.Log("OnValidate() worldlayer called. Serialized property changed!");
        }
		
		
		public void PopulateLayer(WorldBuilder worldBuilder)
	    {
    		//Debug.Log('selectedLayer minimumThreshold ' + minimumThreshold );
			//Debug.Log('selectedLayer maximumThreshold ' + maximumThreshold );
			
			Stopwatch stopwatch = new Stopwatch(); // Create a new stopwatch
			Bounds terrainBounds = worldBuilder.GetTerrainBounds();

	        if (!IsValid(worldBuilder))
		    {
		        UnityEngine.Debug.LogError($"Layer is not valid. Please check the data before submitting.");
		        // You can add further actions or UI feedback to inform the user about the invalid data.
        		return; // Exit the method if the layer is not valid
		    }

			// Start measuring the execution time
            stopwatch.Start();
			
			Terrain terrain = worldBuilder.terrain;
			
			LayerConfig layerConfig = new LayerConfig
			{
			    heatmapTexture = heatmapTexture,
			    terrainBounds = terrainBounds,
        		minimumThreshold = minimumThreshold,
        	 	maximumThreshold = maximumThreshold,
			    additiveMode = additiveMode // Set the additiveMode flag as needed
			};

            if (!layerConfig.additiveMode)
            {
                // Clear existing instances for both trees and details on the terrain for this layer
                ClearExistingTrees(terrain, unityTrees);
                ClearExistingDetails(terrain, unityDetails);
            }

			// Calculate points for Unity Trees
	        foreach (UnityTree unityTree in unityTrees)
	        {
	        	unityTree.PopulateObject(terrain, layerConfig);
	        }
			
	        // Calculate points for Unity Details
	        foreach (UnityDetail unityDetail in unityDetails)
	        {
				unityDetail.PopulateObject(terrain, layerConfig);
	        }
			
			// Stop measuring the execution time
            stopwatch.Stop();

            // Print the execution time in debug logs
            UnityEngine.Debug.Log($"Populate Layer execution time: {stopwatch.ElapsedMilliseconds} ms");
	    }
		
		public static void ClearExistingDetails(Terrain terrain, List<UnityDetail> unityDetails)
        {
            if (terrain == null)
            {
                UnityEngine.Debug.LogError("Terrain is not set in the WorldBuilder.");
                return;
            }

            if (unityDetails == null || unityDetails.Count == 0)
            {
                UnityEngine.Debug.LogWarning("No Unity Details specified to clear on the terrain.");
                return;
            }

            int detailResolution = terrain.terrainData.detailResolution;
            int detailResolutionPerPatch = terrain.terrainData.detailResolutionPerPatch;

            foreach (UnityDetail unityDetail in unityDetails)
            {
                if (unityDetail.detailPrototypeIndex < 0 || unityDetail.detailPrototypeIndex >= terrain.terrainData.detailPrototypes.Length)
                {
                    UnityEngine.Debug.LogWarning($"Invalid detail prototype index for '{unityDetail.detailPrototypeIndex}' in WorldLayer. Skipping clearing details.");
                    continue;
                }

                // Create an empty detail layer array with zero density
                int[,] emptyDetailLayer = new int[detailResolution, detailResolution];
                for (int x = 0; x < detailResolution; x++)
                {
                    for (int z = 0; z < detailResolution; z++)
                    {
                        emptyDetailLayer[x, z] = 0;
                    }
                }

                // Set the empty detail layer on the terrain to clear existing details
                terrain.terrainData.SetDetailLayer(0, 0, unityDetail.detailPrototypeIndex, emptyDetailLayer);
            }
        }
		
		// Helper method to clear existing trees on the terrain for this layer
        private void ClearExistingTrees(Terrain terrain, List<UnityTree> trees)
        {
            TreeInstance[] treeInstances = terrain.terrainData.treeInstances;

            // Create a new list to hold the updated TreeInstances
            List<TreeInstance> updatedTreeInstances = new List<TreeInstance>();

            // Loop through the existing TreeInstances and remove only those in the unityTrees list
            foreach (TreeInstance treeInstance in treeInstances)
            {
                // Check if the treeInstance is present in the unityTrees list for this layer
                bool shouldRemove = trees.Exists(t => t.treePrototypeIndex == treeInstance.prototypeIndex);

                if (!shouldRemove)
                {
                    // If the treeInstance should not be removed, add it to the updatedTreeInstances list
                    updatedTreeInstances.Add(treeInstance);
                }
            }

            // Assign the updated TreeInstance array back to the terrain
            terrain.terrainData.treeInstances = updatedTreeInstances.ToArray();

            // Update the terrain to apply the changes
            terrain.Flush();
        }
		
		// This checks that the Layer and the layer objects are valid. Objects have their own IsValid method
        public bool IsValid(WorldBuilder worldBuilder)
        {
            // Validate the terrain bounds
            Terrain terrain = worldBuilder.terrain;
            if (terrain == null)
            {
                UnityEngine.Debug.LogError("Terrain is not set in the WorldBuilder.");
                return false;
            }

            Vector3 terrainSize = terrain.terrainData.size;
            Bounds terrainBounds = new Bounds(terrain.transform.position + terrainSize * 0.5f, terrainSize);
            if (terrainBounds.extents == Vector3.zero)
            {
                UnityEngine.Debug.LogError("Terrain bounds could not be calculated. Make sure the terrain is set in the WorldBuilder.");
                return false;
            }

			// Validate the WorldLayer properties
            bool isValid = !string.IsNullOrWhiteSpace(name) && heatmapTexture != null;
            if (!isValid)
            {
                UnityEngine.Debug.LogError("Layer Name or Heatmap is missing.");
                return false;
            }
				
	        // Check for duplicate LayerObjects within current Layer
	        if (!ValidateCurrentLayerDuplicates())
	        {
	            // UnityEngine.Debug.LogError("Duplicate LayerObjects detected within current Layer.");
	            return false;
	        }
			
			// Check for duplicate LayerObjects across WorldLayers
	        if (!ValidateDuplicates(worldBuilder))
	        {
	            // UnityEngine.Debug.LogError("Duplicate LayerObjects detected across WorldLayers.");
	            return false;
	        }
	
			foreach (UnityTree tree in unityTrees)
			{
			    object validationResult = tree.Validate(terrain);
			    if (validationResult is string errorMessage)
			    {
			        UnityEngine.Debug.Log($"Tree '{tree.name} index {tree.treePrototypeIndex}' in WorldLayer '{name}' is not valid: {errorMessage}");
			        return false; // Or handle the error as needed
			    }
			}
			
			foreach (UnityDetail detail in unityDetails)
			{
			    object validationResult = detail.Validate(terrain);
			    if (validationResult is string errorMessage)
			    {
			        UnityEngine.Debug.Log($"Detail '{detail.name} index {detail.detailPrototypeIndex}' in WorldLayer '{name}' is not valid: {errorMessage}");
			        return false; // Or handle the error as needed
			    }
			}

            return true;
        }

        // Helper method to check for duplicates in the current layer
        private bool ValidateCurrentLayerDuplicates()
        {
            // Create a HashSet to keep track of the tree and detail prototype indices used in the current layer
            HashSet<int> usedTreeIndices = new HashSet<int>();
            HashSet<int> usedDetailIndices = new HashSet<int>();

            foreach (UnityTree tree in unityTrees)
            {
                if (!usedTreeIndices.Add(tree.treePrototypeIndex))
                {
                    UnityEngine.Debug.LogError($"Duplicate tree prototype index already in '{name}' layer : {tree.treePrototypeIndex}");
                    return false;
                }
            }

            foreach (UnityDetail detail in unityDetails)
            {
                if (!usedDetailIndices.Add(detail.detailPrototypeIndex))
                {
                    UnityEngine.Debug.LogError($"Duplicate detail prototype index already in '{name}' layer: {detail.detailPrototypeIndex}");
                    return false;
                }
            }

            return true; // No duplicates found in the current layer
        }
		
		// Add a method to check for duplicates in other WorldLayers
		private bool ValidateDuplicates(WorldBuilder worldBuilder)
		{
		    // Create a dictionary to store the assigned layers for each tree and detail prototype index
		    Dictionary<int, List<string>> treePrototypeAssignedLayers = new Dictionary<int, List<string>>();
		    Dictionary<int, List<string>> detailPrototypeAssignedLayers = new Dictionary<int, List<string>>();
		
		    // Collect all unique tree prototype IDs and detail prototype IDs from other layers in HashSets
		    HashSet<int> treePrototypeIDsFromOtherLayers = new HashSet<int>();
		    HashSet<int> detailPrototypeIDsFromOtherLayers = new HashSet<int>();
		
		    foreach (WorldLayer worldLayer in worldBuilder.worldLayers)
		    {
		        if (worldLayer == this)
		            continue; // Skip the current layer
		
		        foreach (UnityTree tree in worldLayer.unityTrees)
		        {
		            treePrototypeIDsFromOtherLayers.Add(tree.treePrototypeIndex);
		
		            if (!treePrototypeAssignedLayers.TryGetValue(tree.treePrototypeIndex, out var assignedLayers))
		            {
		                assignedLayers = new List<string>();
		                treePrototypeAssignedLayers[tree.treePrototypeIndex] = assignedLayers;
		            }
		
		            assignedLayers.Add(worldLayer.name);
		        }
		
		        foreach (UnityDetail detail in worldLayer.unityDetails)
		        {
		            detailPrototypeIDsFromOtherLayers.Add(detail.detailPrototypeIndex);
		
		            if (!detailPrototypeAssignedLayers.TryGetValue(detail.detailPrototypeIndex, out var assignedLayers))
		            {
		                assignedLayers = new List<string>();
		                detailPrototypeAssignedLayers[detail.detailPrototypeIndex] = assignedLayers;
		            }
		
		            assignedLayers.Add(worldLayer.name);
		        }
		    }
		
		    // Check if there are any duplicates in the current layer
		    foreach (UnityTree tree in unityTrees)
		    {
		        if (treePrototypeIDsFromOtherLayers.Contains(tree.treePrototypeIndex))
		        {
		            // Get the assigned layers for this tree prototype index
		            List<string> assignedLayers = treePrototypeAssignedLayers[tree.treePrototypeIndex];
		
		            // If the tree prototypeID is found in the HashSet, it means it's a duplicate
		            // and we should report an error with the assigned layers
		            UnityEngine.Debug.LogError($"Tree '{tree.name} index {tree.treePrototypeIndex}' already exists in other WorldLayers: {string.Join(", ", assignedLayers)}.");
		            return false;
		        }
		    }
		
		    foreach (UnityDetail detail in unityDetails)
		    {
		        if (detailPrototypeIDsFromOtherLayers.Contains(detail.detailPrototypeIndex))
		        {
		            // Get the assigned layers for this detail prototype index
		            List<string> assignedLayers = detailPrototypeAssignedLayers[detail.detailPrototypeIndex];
		
		            // If the detail prototypeID is found in the HashSet, it means it's a duplicate
		            // and we should report an error with the assigned layers
		            UnityEngine.Debug.LogError($"Detail '{detail.name} index {detail.detailPrototypeIndex}' already exists in other WorldLayers: {string.Join(", ", assignedLayers)}.");
		            return false;
		        }
		    }
		
		    return true; // No duplicates found
		}
		
		public void RemoveTreeObject(WorldBuilder worldBuilder, UnityTree unityTree)
        {
            unityTrees.Remove(unityTree);

            // After removing the tree object, clear its instances from the terrain
            Terrain terrain = worldBuilder.terrain;
            if (terrain != null)
            {
                ClearExistingTrees(terrain, new List<UnityTree> { unityTree });
            }
        }
		
		public void RemoveDetailObject(WorldBuilder worldBuilder, UnityDetail unityDetail)
        {
            unityDetails.Remove(unityDetail);

            // After removing the detail object, clear its instances from the terrain
            Terrain terrain = worldBuilder.terrain;
            if (terrain != null)
            {
                ClearExistingDetails(terrain, new List<UnityDetail> { unityDetail });
            }
        }
    }
}
