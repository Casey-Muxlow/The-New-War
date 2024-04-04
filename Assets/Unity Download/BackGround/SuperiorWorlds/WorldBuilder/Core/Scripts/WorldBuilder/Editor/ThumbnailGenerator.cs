using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SuperiorWorlds
{
    public class ThumbnailGenerator
    {
        // Singleton instance
        private static ThumbnailGenerator instance;
        public static ThumbnailGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ThumbnailGenerator();
                }
                return instance;
            }
        }

        private ThumbnailGenerator()
        {
            // Private constructor to prevent external instantiation
        }

        // Cache for texture thumbnails
        private Dictionary<Texture2D, Texture2D> textureThumbnailCache = new Dictionary<Texture2D, Texture2D>();

        // Cache for mesh thumbnails
        private Dictionary<GameObject, Texture2D> meshThumbnailCache = new Dictionary<GameObject, Texture2D>();

        public Texture2D GenerateMeshThumbnail(GameObject meshPrefab)
        {
            if (meshPrefab == null || !PrefabUtility.IsPartOfPrefabAsset(meshPrefab))
            {
                Debug.LogWarning("Invalid mesh prefab. Thumbnail generation failed.");
                return null;
            }

            if (meshThumbnailCache.TryGetValue(meshPrefab, out Texture2D cachedThumbnail))
            {
                return cachedThumbnail;
            }

            Texture2D thumbnail = AssetPreview.GetAssetPreview(meshPrefab);

            // Cache the thumbnail for future use
            meshThumbnailCache.Add(meshPrefab, thumbnail);

            return thumbnail;
        }
		
        public Texture2D GenerateTreeThumbnail(TreePrototype treePrototype, Terrain terrain)
        {
            if (treePrototype == null || terrain == null)
            {
                Debug.LogWarning("Invalid tree prototype or terrain. Thumbnail generation failed.");
                return null;
            }

            return GeneratePreviewThumbnail(treePrototype.prefab, terrain);
        }

        public Texture2D GenerateDetailThumbnail(DetailPrototype detailPrototype, Terrain terrain)
        {
            if (detailPrototype == null || terrain == null)
            {
                Debug.LogWarning("Invalid detail prototype or terrain. Thumbnail generation failed.");
                return null;
            }

            if (detailPrototype.usePrototypeMesh)
            {
                return GeneratePreviewThumbnail(detailPrototype.prototype, terrain);
            }
            else
            {
                return AssetPreview.GetAssetPreview(detailPrototype.prototypeTexture);
            }
        }

        private Texture2D GeneratePreviewThumbnail(GameObject prefab, Terrain terrain)
        {
            GameObject previewGameObject = new GameObject(prefab.name);
            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            prefabInstance.transform.SetParent(previewGameObject.transform);

            // Match position and rotation to terrain
            Vector3 terrainPosition = terrain.transform.position;
            Vector3 terrainCenter = terrain.terrainData.bounds.center;
            Vector3 offset = terrainCenter - terrainPosition;

            previewGameObject.transform.position = offset;
            previewGameObject.transform.rotation = terrain.transform.rotation;

            Texture2D thumbnail = AssetPreview.GetAssetPreview(previewGameObject);

            GameObject.DestroyImmediate(previewGameObject);

            return thumbnail;
        }
    }
}
