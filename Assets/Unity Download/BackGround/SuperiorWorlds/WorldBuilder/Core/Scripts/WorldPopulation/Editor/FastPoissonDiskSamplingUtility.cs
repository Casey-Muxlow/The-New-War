using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SuperiorWorlds;

namespace SuperiorWorlds
{
    public class FastPoissonDiskSamplingUtility : EditorWindow
    {
        public GameObject spherePrefab;
        public Vector2 boundsBottomLeft = new Vector2(0f, 0f);
        public Vector2 boundsTopRight = new Vector2(100f, 100f);
        public float minimumDistance = 1f;
        public int iterationPerPoint = 30;

        private List<Vector2> sampledPoints;

        [MenuItem("Window/World Builder/Debug/Fast Poisson Disk Sampling Utility")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(FastPoissonDiskSamplingUtility));
        }

        private void OnGUI()
        {
            GUILayout.Label("Fast Poisson Disk Sampling Utility", EditorStyles.boldLabel);

            spherePrefab = EditorGUILayout.ObjectField("Sphere Prefab", spherePrefab, typeof(GameObject), false) as GameObject;
            boundsBottomLeft = EditorGUILayout.Vector2Field("Bounds Bottom Left", boundsBottomLeft);
            boundsTopRight = EditorGUILayout.Vector2Field("Bounds Top Right", boundsTopRight);
            minimumDistance = EditorGUILayout.FloatField("Minimum Distance", minimumDistance);
            iterationPerPoint = EditorGUILayout.IntField("Iteration Per Point", iterationPerPoint);

            if (GUILayout.Button("Simulate"))
            {
                Simulate();
            }
        }

        private void Simulate()
        {
            // Clear previous spheres
            ClearSpheres();

            // Run Poisson Disk Sampling
            sampledPoints = SuperiorWorlds.FastPoissonDiskSampling.Sampling(boundsBottomLeft, boundsTopRight, minimumDistance, iterationPerPoint);

            // Create spheres to represent sampled points
            CreateSpheres();
        }

        private void CreateSpheres()
        {
            foreach (Vector2 point in sampledPoints)
            {
                // Convert 2D point to 3D position on the plane
                Vector3 position = new Vector3(point.x, 0f, point.y);

                // Instantiate a sphere GameObject at the position
                Instantiate(spherePrefab, position, Quaternion.identity);
            }
        }

        private void ClearSpheres()
        {
            GameObject[] spheres = GameObject.FindGameObjectsWithTag("Sphere");
            foreach (GameObject sphere in spheres)
            {
                DestroyImmediate(sphere);
            }
        }
    }
}
