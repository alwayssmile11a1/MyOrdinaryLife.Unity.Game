using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class CameraFitter : MonoBehaviour
{
    public float masterCameraSize = 5;
    public Vector2 masterAspectRatio;

    void Awake()
    {
        FitCamera();
    }

#if UNITY_EDITOR
    private void Update()
    {
        FitCamera();
    }
#endif

    void FitCamera()
    {
        Camera camera = GetComponent<Camera>();
        float masterWidth = masterCameraSize * 2 * masterAspectRatio.x / masterAspectRatio.y;
        float currentAspectRatio = camera.aspect;
        float desiredCameraSize = masterWidth / currentAspectRatio * 1 / 2;
        camera.orthographicSize = desiredCameraSize;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        FitCamera();
    }
#endif
}
