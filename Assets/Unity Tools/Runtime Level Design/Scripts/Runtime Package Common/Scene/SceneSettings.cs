using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace RLD
{
    [Serializable]
    public class SceneSettings : Settings
    {
        [SerializeField]
        private ScenePhysicsMode _physicsMode = ScenePhysicsMode.RLD;
        [SerializeField]
        private float _noVolumeObjectSize = 1.0f;

        public ScenePhysicsMode PhysicsMode { get { return _physicsMode; } set { if (!Application.isPlaying) _physicsMode = value; } }
        public float NoVolumeObjectSize { get { return _noVolumeObjectSize; } set { if (!Application.isPlaying) _noVolumeObjectSize = Mathf.Max(1e-1f, value); } }

        #if UNITY_EDITOR
        protected override void RenderContent(UnityEngine.Object undoRecordObject)
        {
            ScenePhysicsMode newPhysicsMode; float newFloat;
            var content = new GUIContent();

            content.text = "Physics mode";
            content.tooltip = "Controls the way in which raycasts, overlap tests etc are performed. It is recommended to leave this to \'RLD\'. Otherwise, some " +
                              "plugin features might not work as expected. You should select \'UnityColliders\' if you " +
                              "are experiencing slow frame rates which can happen for heavily populated scenes. In that case, you will need to attach colliders for " +
                              "all objects that you would like to interact with.";
            newPhysicsMode = (ScenePhysicsMode)EditorGUILayout.EnumPopup(content, PhysicsMode);
            if (newPhysicsMode != PhysicsMode)
            {
                EditorUndoEx.Record(undoRecordObject);
                PhysicsMode = newPhysicsMode;
            }

            content.text = "No-volume object size";
            content.tooltip = "This field is used to define the volume size of the objects that do not have a mesh or sprite (e.g. lights, particle systems etc). This size is " + 
                              "needed to allow the system to perform raycasts or overlap tests for such objects as well as draw their icons in the scene when necessary.";
            newFloat = EditorGUILayout.FloatField(content, NoVolumeObjectSize);
            if (newFloat != NoVolumeObjectSize)
            {
                EditorUndoEx.Record(undoRecordObject);
                NoVolumeObjectSize = newFloat;
            }
        }
        #endif
    }
}
