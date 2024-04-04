using System;
using UnityEngine;
using System.Collections.Generic;

namespace SuperiorWorlds
{
	[Serializable]
	public class UnityDetail : LayerObject
	{
    	public DetailPrototype prototype; // Reference to the detail prototype
		public int detailPrototypeIndex; // Reference to the prototype index in the terrain
    	public List<Vector2> positions; // Add a points list to store calculated points
		public float clusterRadius = 0.0f;
		public int clusterAmount = 3;
	    // Add other unique fields for Unity Details if needed
	
        // Override the PopulateObject method for Unity Details
		public override void PopulateObject(Terrain terrain, WorldLayer.LayerConfig layer)
		{
		    if (terrain == null)
		    {
		        Debug.LogError("Terrain is not set in the WorldBuilder.");
		        return;
		    }
		
			// Calculate Positions
		    Vector2 bottomLeft = new Vector2(layer.terrainBounds.min.x, layer.terrainBounds.min.z);
		    Vector2 topRight = new Vector2(layer.terrainBounds.max.x, layer.terrainBounds.max.z);
		
		    // Calculate the positions using PositionGenerator
		    positions = PositionGenerator.FilterPoints(layer.heatmapTexture, bottomLeft, topRight, minimumDistance, layer.minimumThreshold, layer.maximumThreshold, true, false, 90);
		    // Debug.Log($"Calculated details for '{name}': {positions.Count}");
		
		    // Populate Detail Positions
		    PopulateDetailPositions(terrain, layer, bottomLeft, topRight);
		}
		
		private void PopulateDetailPositions(Terrain terrain, WorldLayer.LayerConfig layer, Vector2 topRight, Vector2 bottomLeft)
		{
		    // Get the detail prototype index from the UnityDetail
		    int detailPrototypeIndex = this.detailPrototypeIndex;
		
		    // Get the terrain resolution
		    int detailResolution = terrain.terrainData.detailResolution;
		    int detailResolutionPerPatch = terrain.terrainData.detailResolutionPerPatch;
		
		    // Convert the world positions to terrain space
		    int[,] detailLayer = new int[terrain.terrainData.detailResolution, terrain.terrainData.detailResolution];
		
		    foreach (Vector2 position in positions)
		    {
		        // Calculate the terrain space positions
		        float normalizedX = Mathf.InverseLerp(bottomLeft.x, topRight.x, position.x);
		        float normalizedZ = Mathf.InverseLerp(bottomLeft.y, topRight.y, position.y);
		        int x = Mathf.RoundToInt(normalizedX * detailResolution);
		        int z = Mathf.RoundToInt(normalizedZ * detailResolution);
		
		        // Calculate the detail patch index
		        int patchX = x / detailResolutionPerPatch;
		        int patchZ = z / detailResolutionPerPatch;
		
		        // Calculate the detail index within the patch
		        int patchDetailX = x % detailResolutionPerPatch;
		        int patchDetailZ = z % detailResolutionPerPatch;
		
		        // Make sure the indices are within the bounds of the detailLayer array
		        patchX = Mathf.Clamp(patchX, 0, terrain.terrainData.detailResolution / detailResolutionPerPatch - 1);
		        patchZ = Mathf.Clamp(patchZ, 0, terrain.terrainData.detailResolution / detailResolutionPerPatch - 1);
		        patchDetailX = Mathf.Clamp(patchDetailX, 0, detailResolutionPerPatch - 1);
		        patchDetailZ = Mathf.Clamp(patchDetailZ, 0, detailResolutionPerPatch - 1);
		
		        // Set the detail at the calculated index with a density of 1.0 (full density)
		        detailLayer[patchDetailX + patchX * detailResolutionPerPatch, patchDetailZ + patchZ * detailResolutionPerPatch] = 1;
		
		        // Calculate cluster positions using DetailClusterer class
				float angleIncrement = 360f / clusterAmount; // Angle between each cluster
				Vector2[] clusterPositions = DetailClusterer.CalculateConcentricClusterPositions(position, clusterRadius, clusterAmount);
				
				foreach (Vector2 clusterPosition in clusterPositions)
		        {
		            // Calculate the terrain space positions of the clustered point
		            float normalizedClusterX = Mathf.InverseLerp(bottomLeft.x, topRight.x, clusterPosition.x);
		            float normalizedClusterZ = Mathf.InverseLerp(bottomLeft.y, topRight.y, clusterPosition.y);
		            int clusterX = Mathf.RoundToInt(normalizedClusterX * detailResolution);
		            int clusterZ = Mathf.RoundToInt(normalizedClusterZ * detailResolution);
		
		            // Calculate the detail patch index for the clustered point
		            int clusterPatchX = clusterX / detailResolutionPerPatch;
		            int clusterPatchZ = clusterZ / detailResolutionPerPatch;
		
		            // Calculate the detail index within the patch for the clustered point
		            int clusterPatchDetailX = clusterX % detailResolutionPerPatch;
		            int clusterPatchDetailZ = clusterZ % detailResolutionPerPatch;
		
		            // Make sure the indices are within the bounds of the detailLayer array for the clustered point
		            clusterPatchX = Mathf.Clamp(clusterPatchX, 0, terrain.terrainData.detailResolution / detailResolutionPerPatch - 1);
		            clusterPatchZ = Mathf.Clamp(clusterPatchZ, 0, terrain.terrainData.detailResolution / detailResolutionPerPatch - 1);
		            clusterPatchDetailX = Mathf.Clamp(clusterPatchDetailX, 0, detailResolutionPerPatch - 1);
		            clusterPatchDetailZ = Mathf.Clamp(clusterPatchDetailZ, 0, detailResolutionPerPatch - 1);
		
		            // Set the detail at the calculated index with a density of 1.0 (full density)
		            detailLayer[clusterPatchDetailX + clusterPatchX * detailResolutionPerPatch, clusterPatchDetailZ + clusterPatchZ * detailResolutionPerPatch] = 1;
		        }
		    }
		
		    // Set the entire detail layer array on the terrain
		    terrain.terrainData.SetDetailLayer(0, 0, detailPrototypeIndex, detailLayer);
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

            if (detailPrototypeIndex < 0)
            {
                return "Detail prototype index must be non-negative.";
            }
			// Check if there are no detail prototypes loaded on the terrain
		    if (detailPrototypeIndex < 0 || detailPrototypeIndex >= terrain.terrainData.detailPrototypes.Length)
		    {
		        return $"No Unity Details found for '{detailPrototypeIndex}'. Make sure details are loaded on the terrain and the detail prototype index is valid.";
		    }
            return true; // Validation success
        }
		
		// Constructor
        public UnityDetail()
        {
            name = "Detail";
        }
    }
	
	public class DetailClusterer
	{
	    public static Vector2[] CalculateConcentricClusterPositions(Vector2 origin, float clusterRadius, int clusterAmount)
	    {
	        if (clusterRadius <= 0f || clusterAmount <= 0)
	        {
	            // If clusterRadius is zero or clusterAmount is zero, return a single detail at the origin
	            return new Vector2[] { origin };
	        }
	
	        List<Vector2> positions = new List<Vector2>();
	
	        float angleIncrement = 360f / clusterAmount;
	        float radiusIncrement = clusterRadius / Mathf.Sqrt(clusterAmount);
	
	        for (int i = 0; i < clusterAmount; i++)
	        {
	            float angle = i * angleIncrement;
	            float radius = radiusIncrement * Mathf.Sqrt(i + 1);
	
	            float offsetX = radius * Mathf.Cos(angle);
	            float offsetZ = radius * Mathf.Sin(angle);
	
	            Vector2 clusterPosition = origin + new Vector2(offsetX, offsetZ);
	            positions.Add(clusterPosition);
	        }
	
	        return positions.ToArray();
	    }
	}

}