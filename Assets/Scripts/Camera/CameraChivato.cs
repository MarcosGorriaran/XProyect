using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChivato : MonoBehaviour
{
    private Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    void Update()
    {
        Debug.Log(_camera.rect);
    }
}
