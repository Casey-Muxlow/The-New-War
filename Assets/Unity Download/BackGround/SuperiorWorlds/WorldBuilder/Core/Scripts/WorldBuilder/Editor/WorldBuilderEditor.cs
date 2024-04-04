using UnityEditor;
using UnityEngine;

namespace SuperiorWorlds
{
    [CustomEditor(typeof(WorldBuilder))]
    public class WorldBuilderEditor : Editor
    {
        private SuperiorWorlds.WorldBuilder worldBuilder; // Reference to the WorldBuilder script

        // private void OnEnable()
        // {
        //     worldBuilder = (WorldBuilder)target; // Assign the WorldBuilder reference
        // }
		// 
        // public override void OnInspectorGUI()
        // {
        //     // Draw the default inspector GUI
        //     DrawDefaultInspector();
		// 
        //     // Custom editor GUI elements go here
        //     // For example, you can display the terrain object reference like this:
        //     worldBuilder.terrain = EditorGUILayout.ObjectField("Terrain Object", worldBuilder.terrain, typeof(Terrain), true) as Terrain;
        // }
    }
}
