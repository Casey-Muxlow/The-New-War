using UnityEditor;
using UnityEngine;
using System.Collections.Generic; // Add this line for the List<> type
using System.Diagnostics;

namespace SuperiorWorlds
{
    public class PositionGeneratorUtility : EditorWindow
    {
        private Texture2D heatmapTexture;
        private float minimumThreshold = 0.5f;
        private float maximumThreshold = 1f;
        private float minimumDistance = 1f;
        private float areaSizeX = 10f;
        private float areaSizeZ = 10f;

        [MenuItem("Window/World Builder/Debug/Position Generator Utility")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(PositionGeneratorUtility));
        }

        private void OnGUI()
        {
            GUILayout.Label("Heatmap Point Filter", EditorStyles.boldLabel);

            heatmapTexture = (Texture2D)EditorGUILayout.ObjectField("Heatmap Texture", heatmapTexture, typeof(Texture2D), false);
            minimumThreshold = EditorGUILayout.Slider("Minimum Threshold", minimumThreshold, 0f, 1f);
            maximumThreshold = EditorGUILayout.Slider("Maximum Threshold", maximumThreshold, 0f, 1f);
            minimumDistance = EditorGUILayout.FloatField("Minimum Distance", minimumDistance);
            areaSizeX = EditorGUILayout.FloatField("Area Size X", areaSizeX);
            areaSizeZ = EditorGUILayout.FloatField("Area Size Z", areaSizeZ);

            if (GUILayout.Button("Simulate Points"))
            {
                SimulatePoints();
            }
        }

		private void SimulatePoints()
		{
		    Vector2 bottomLeft = new Vector2(0f, 0f);
		    Vector2 topRight = new Vector2(areaSizeX, areaSizeZ);
		
		    Stopwatch stopwatch = new Stopwatch();
		    stopwatch.Start();
			
		    List<Vector2> filteredPoints = PositionGenerator.FilterPoints(heatmapTexture, bottomLeft, topRight, minimumDistance, minimumThreshold, maximumThreshold);
		
		    stopwatch.Stop();
		    UnityEngine.Debug.Log("Number of filtered points: " + filteredPoints.Count);
            UnityEngine.Debug.Log("Time taken to fully process: " + stopwatch.ElapsedMilliseconds + " ms");

		
		    // Create or find the parent game object "FilteredPoints"
		    // GameObject parentObject = GameObject.Find("FilteredPoints");
		    // if (parentObject == null)
		    // {
		    //     parentObject = new GameObject("FilteredPoints");
		    // }
		    // else
		    // {
		    //     // Remove any existing child objects
		    //     foreach (Transform child in parentObject.transform)
		    //     {
		    //         DestroyImmediate(child.gameObject);
		    //     }
		    // }
			// 
		    // // Instantiate spheres as child objects
		    // foreach (Vector2 point in filteredPoints)
		    // {
		    //     GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		    //     sphere.transform.position = new Vector3(point.x, 0f, point.y);
		    //     sphere.transform.SetParent(parentObject.transform);
		    // }
		}

    }
}
