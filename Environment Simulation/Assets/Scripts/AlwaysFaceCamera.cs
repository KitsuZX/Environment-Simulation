using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{

    private new Transform camera;

    
    void Update()
    {
        transform.LookAt(camera, Vector3.up);
    }

    private void Awake()
    {
        camera = Camera.main.transform;
    }
}
