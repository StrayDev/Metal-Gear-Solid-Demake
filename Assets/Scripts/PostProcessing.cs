using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    [SerializeField]
    private Material postProcessingMaterial;
    private Camera cam;

    private void Start()
    {
        //access depth and normal buffer
        cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.DepthNormals;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //converts viewspace matrix to worldspace matrix
        Matrix4x4 viewToWorld = cam.cameraToWorldMatrix;
        postProcessingMaterial.SetMatrix("_ViewToWorld", viewToWorld);

        Graphics.Blit(source, destination, postProcessingMaterial);
    }
}
