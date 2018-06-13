using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VuforiaCameraScaler : MonoBehaviour {

    public Camera scaledCamera;
    public Camera vuforiaCamera;
    public GameObject AnchorStage;
    //public Vector3 scaledObjectOrigin; //ARKit version
    public float cameraScale = 1.0f;

    // Use this for initialization
    void Start()
    {
        ContentScaleManager.ContentScaleChangedEvent += ContentScaleChanged;
    }

    void ContentScaleChanged(float scale, float prevScale)
    {
        cameraScale = scale;
    }


    void Update()
    {
        if (scaledCamera != null && cameraScale > 0.0001f && cameraScale < 10000.0f)
        {
            //Matrix4x4 matrix = UnityARSessionNativeInterface.GetARSessionNativeInterface().GetCameraPose(); //ARKit version
            float invScale = 1.0f / cameraScale;
            //Vector3 cameraPos = UnityARMatrixOps.GetPosition (matrix); //ARKit version
            Vector3 cameraPos = vuforiaCamera.transform.position; //My Vuforia version
            //Vector3 vecAnchorToCamera =  cameraPos - scaledObjectOrigin; //ARKit version
            Vector3 vecAnchorToCamera = cameraPos - AnchorStage.transform.position; //My Vuforia version
            //scaledCamera.transform.localPosition = scaledObjectOrigin + (vecAnchorToCamera * invScale);
            scaledCamera.transform.localPosition = AnchorStage.transform.position + (vecAnchorToCamera * invScale);
            //scaledCamera.transform.localRotation = UnityARMatrixOps.GetRotation (matrix); //ARKit version
            scaledCamera.transform.localRotation = vuforiaCamera.transform.localRotation; //My Vuforia version


            //this needs to be adjusted for near/far
            //scaledCamera.projectionMatrix = UnityARSessionNativeInterface.GetARSessionNativeInterface().GetCameraProjection (); //ARKit version
            scaledCamera.projectionMatrix = vuforiaCamera.projectionMatrix; //My Vuforia version
        }
    }
}