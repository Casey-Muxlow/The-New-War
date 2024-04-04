using System;
using UnityEngine;
using System.Collections.Generic;

namespace SuperiorWorlds
{
    [Serializable]
    public class UnityTree : LayerObject
    {
        public int treePrototypeIndex;
        // Add fields for the height range
        public float minHeight = 0.8f;
        public float maxHeight = 1.2f;
        public List<Vector2> positions; // Add a points list to store calculated points
		
		// Override the PopulateObject method for Unity Trees
		public override void PopulateObject(Terrain terrain, WorldLayer.LayerConfig layer)
		{
		    if (terrain == null)
		    {
		        Debug.LogError("Terrain is not set in the WorldBuilder.");
		        return;
		    }
		
		    Vector2 bottomLeft = new Vector2(layer.terrainBounds.min.x, layer.terrainBounds.min.z);
		    Vector2 topRight = new Vector2(layer.terrainBounds.max.x, layer.terrainBounds.max.z);
			
		    // Calculate the positions using PositionGenerator
		    positions = PositionGenerator.FilterPoints(layer.heatmapTexture, bottomLeft, topRight, minimumDistance, layer.minimumThreshold, layer.maximumThreshold);
		    // Debug.Log($"Calculated trees for '{name}': {positions.Count}");
		
		    // Get the tree prototype index from the UnityTree
		    int treePrototypeIndex = this.treePrototypeIndex;
			
			// Get the terrain's existing TreeInstance array
		    TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
		
		    // Create a new list to hold the updated TreeInstances
		    List<TreeInstance> updatedTreeInstances = new List<TreeInstance>(treeInstances);
		
		    // Remove all instances with the target prototype index
		    updatedTreeInstances.RemoveAll(tree => tree.prototypeIndex == treePrototypeIndex);

		    // Assign the updated TreeInstance array back to the terrain
		    terrain.terrainData.treeInstances = updatedTreeInstances.ToArray();
			
		    // Convert the world positions to terrain space and add the tree instances
		    foreach (Vector2 position in positions)
		    {
		        // Calculate the normalized terrain space positions
		        float normalizedX = Mathf.InverseLerp(bottomLeft.x, topRight.x, position.x);
		        float normalizedZ = Mathf.InverseLerp(bottomLeft.y, topRight.y, position.y);
		
		        // Randomize the scale and position
		        // float scale = Random.Range(randomScaleMin, randomScaleMax);
		        Vector3 treePosition = new Vector3(normalizedX, 0, normalizedZ);
		        // Randomize the height within the specified range
        		float heightScale = UnityEngine.Random.Range(minHeight, maxHeight);

		        // Create a new tree instance and add it to the terrain directly
		        TreeInstance treeInstance = new TreeInstance
		        {
		            position = treePosition,
		            prototypeIndex = treePrototypeIndex,
		            rotation = UnityEngine.Random.Range(0, 360),
		            heightScale = heightScale,
		            widthScale = heightScale,
		            color = Color.white,
		            lightmapColor = Color.white
		        };
		
		        // Add the treeInstance directly to the terrain
		        terrain.AddTreeInstance(treeInstance);
		    }
		
		    // Update the terrain to apply the changes
		    terrain.Flush();
		}


        // Custom validation method
        public object Validate(Terrain terrain)
        {
            if (minimumDistance < 1f)
            {
                return "Minimum distance must be greater than or equal to 1.";
            }

            if (string.IsNullOrEmpty(name.Trim()))
            {
                return "Name cannot be empty or contain only whitespace.";
            }

            if (treePrototypeIndex < 0)
            {
                return "Tree prototype index must be non-negative.";
            }

            // Check if there are no tree prototypes loaded on the terrain
            if (treePrototypeIndex < 0 || treePrototypeIndex >= terrain.terrainData.treePrototypes.Length)
            {
                return "No Unity Trees found for '{name}'. Make sure trees are loaded on the terrain and the tree prototype index is valid.";
            }
            return true; // Validation success
        }

        // Constructor
        public UnityTree()
        {
            name = "Tree";
        }
    }
}
