using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRendererController : MonoBehaviour
{
    public bool isLocalPlayer = true; // Set this to true for the local player character

    private Renderer meshRenderer;

    private void Start()
    {
        // Get the MeshRenderer component
        meshRenderer = GetComponent<Renderer>();

        // Disable rendering if this is the local player character
        if (isLocalPlayer)
        {
            DisableRendering();
        }
    }

    private void DisableRendering()
    {
        // Disable the MeshRenderer component
        meshRenderer.enabled = false;
    }
}
