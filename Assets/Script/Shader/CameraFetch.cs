using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFetch : MonoBehaviour
{
    [SerializeField] private Camera cam;
    

    void Start()
    {
        // get distance from current pixel to the camera
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
    }

    void Update()
    {
        
    }
}
