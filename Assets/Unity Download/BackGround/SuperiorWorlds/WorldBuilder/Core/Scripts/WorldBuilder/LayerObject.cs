using System;
using UnityEngine;

namespace SuperiorWorlds
{
    [Serializable]
    public class LayerObject
    {
        [NonSerialized] public WorldLayer parentLayer; // Reference to the parent WorldLayer
				 
		[SerializeField]
		public string name; // New name field
	     
		[SerializeField]
		public float minimumDistance = 5;
		
        // Stub method for Populating the object (to be overridden by subclasses)
        public virtual void PopulateObject(Terrain terrain, WorldLayer.LayerConfig layer)
        {
            // Implement the specific logic for populating the object in the subclasses.
            // UnityTree and UnityDetail classes will override this method.
        }
		
		// Tooltip for the minimum distance field
        public string minimumDistanceTooltip = "How far apart this object will be populated. Must be at least 1. Add variations to achieve higher more density.";

    }
}
