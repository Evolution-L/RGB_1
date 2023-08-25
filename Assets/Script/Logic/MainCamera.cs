using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainCamera : MonoBehaviour
{
    public void AddCameraToStack(Camera myOverlayCamera)
    {
        var cameraData = GetComponent<Camera>().GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(myOverlayCamera);
    }

    public void RemoveCameraToStack(Camera myOverlayCamera)
    {
        var cameraData = GetComponent<Camera>().GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Remove(myOverlayCamera);
    }
    public void ClearCameraStack()
    {
        var cameraData = GetComponent<Camera>().GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Clear();
    }
}
