using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimumScreenSpaceSize : MonoBehaviour
{
    public float minimumScale = 0.1f;

    private new Camera camera;
    private float trueScale;


    private void Update()
    {
        float distance = Vector3.Distance(camera.transform.position, transform.position);
        float frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float realScreenSpaceSize = trueScale / frustumHeight;

        if (realScreenSpaceSize < minimumScale) transform.localScale = Vector3.one * minimumScale * frustumHeight;
        else transform.localScale = Vector3.one * trueScale;
    }

    private void Start()
    {
        camera = Camera.main;
        trueScale = transform.localScale.x;
    }
}
