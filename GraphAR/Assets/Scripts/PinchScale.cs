using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchScale : MonoBehaviour {

    [SerializeField] private float minScale = 0.1f; // zoom-in and zoom-out limits
    [SerializeField] private float maxScale = 2.0f;
    [SerializeField] private float scaleSpeed = 0.00001f;

    public VuforiaCameraScaler  scaler;

    public bool Enabled = true;

    void ResetScale()
    {
        scaler.cameraScale = 1f;
    }

    void Update()
    {
        // If there are two touches on the device...
        if (Enabled && Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            float currentScale = scaler.cameraScale;

            currentScale += deltaMagnitudeDiff * scaleSpeed;

            currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

            scaler.cameraScale = currentScale; 
        }
    }
}