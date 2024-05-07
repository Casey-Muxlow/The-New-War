using UnityEngine;

public class DisableMeshRenderer : MonoBehaviour
{
    public bool disableChildRenderers = true; // Flag to enable or disable MeshRenderer on child objects

    void Start()
    {
        // Disable the MeshRenderer component on this game object
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        // Optionally, disable the MeshRenderer component on all child game objects recursively
        if (disableChildRenderers)
        {
            DisableMeshRendererInChildren(transform);
        }
    }

    void DisableMeshRendererInChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Disable the MeshRenderer component on the child game object
            MeshRenderer childMeshRenderer = child.GetComponent<MeshRenderer>();
            if (childMeshRenderer != null)
            {
                childMeshRenderer.enabled = false;
            }

            // Recursively disable the MeshRenderer on child's children
            DisableMeshRendererInChildren(child);
        }
    }
}
