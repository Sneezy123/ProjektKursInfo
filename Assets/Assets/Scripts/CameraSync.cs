using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSync : MonoBehaviour
{
    private Camera thisCamera;
    public Camera originalCamera;
    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    void Update()
    {
        thisCamera.fieldOfView = originalCamera.fieldOfView;
        thisCamera.transform.position = originalCamera.transform.position;
        thisCamera.transform.rotation = originalCamera.transform.rotation;
    }
}
