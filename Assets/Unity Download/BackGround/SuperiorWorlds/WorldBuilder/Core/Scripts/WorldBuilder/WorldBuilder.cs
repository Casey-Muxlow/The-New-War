using UnityEngine;
using System.Collections.Generic;

namespace SuperiorWorlds
{
    public class WorldBuilder : MonoBehaviour
    {
        public Terrain terrain; // Add the terrain property.
        public List<WorldLayer> worldLayers = new List<WorldLayer>();
	    // Dictionary to cache the thumbnails
	    private Dictionary<int, Texture2D> cachedThumbnails = new Dictionary<int, Texture2D>();

        private void Start()
        {
            // Read terrain x and z values to determine scale.
            // Read Trees and Details to get the indexes for child classes (Unity Tree and Unity Detail).
        }
		
		public Bounds GetTerrainBounds()
	    {
	        Terrain terrain = this.terrain;
	
	        if (terrain == null)
	        {
	            Debug.LogError("Terrain is not set in the WorldBuilder.");
	            return new Bounds(Vector3.zero, Vector3.zero); // Return an empty bounds if terrain is not set
	        }
	
	        Vector3 terrainSize = terrain.terrainData.size;
	        Vector3 terrainPosition = terrain.transform.position;
	
	        Vector3 bottomLeft = new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z);
	        Vector3 topRight = new Vector3(terrainPosition.x + terrainSize.x, terrainPosition.y, terrainPosition.z + terrainSize.z);
	
	        return new Bounds((bottomLeft + topRight) * 0.5f, topRight - bottomLeft);
	    }
    }
}
