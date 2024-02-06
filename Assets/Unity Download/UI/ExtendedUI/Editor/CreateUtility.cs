using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DIG.Tools
{
    public static class CreateUtility
    {
        public static void CreatePrefab(string path)
        {
            GameObject newObject = PrefabUtility.InstantiatePrefab(Resources.Load(path)) as GameObject;
            Place(newObject);
        }

        public static void CreateObject(string name, params Type[] types)
        {
            GameObject newObject = ObjectFactory.CreateGameObject(name, types);
            Place(newObject);
        }

        public static void CreateUIObject(string path)
        {
            GameObject selectedObject = Selection.activeGameObject;

            if (GameObject.FindObjectOfType<EventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }

            if (selectedObject != null)
            {
                //If selected object contains canvas
                if(selectedObject.GetComponentInParent<Canvas>())
                {
                    GameObject newObject = GameObject.Instantiate(Resources.Load(path),
                    Selection.activeGameObject.transform) as GameObject;
                    newObject.name = path;
                    newObject.transform.localPosition = Vector3.zero;
                    Place(newObject);

                }
                else
                {
                    var canvs = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                    canvs.transform.parent = Selection.activeGameObject.transform;

                    //GameObject newCanvas = GameObject.Instantiate(Resources.Load("Canvas"),
                    //    Selection.activeGameObject.transform) as GameObject;
                    //newCanvas.name = "Canvas";
                    Place(canvs);
                    GameObject newObject = GameObject.Instantiate(Resources.Load(path),
                    canvs.transform) as GameObject;
                    newObject.name = path;
                    newObject.transform.localPosition = Vector3.zero;
                    Place(newObject);
                }
            }
            else
            {
                var canvs = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

                //GameObject newCanvas = GameObject.Instantiate(Resources.Load("Canvas")) as GameObject;
                Place(canvs);
                //newCanvas.name = "Canvas";
                GameObject newObject = GameObject.Instantiate(Resources.Load(path),
                canvs.transform) as GameObject;
                newObject.name = path;
                newObject.transform.localPosition = Vector3.zero;
                Place(newObject);
            }
        }


        public static void Place(GameObject gameObject)
        {
            // Find location
            SceneView lastView = SceneView.lastActiveSceneView;
            gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;

            // Make sure we place the object in the proper scene, with a relevant name
            StageUtility.PlaceGameObjectInCurrentStage(gameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

            // Record undo, and select
            Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
            Selection.activeGameObject = gameObject;

            // For prefabs, let's mark the scene as dirty for saving
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}