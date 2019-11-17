using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BehaviourCommunicator : MonoBehaviour
{
    private new Transform camera;
    private SpriteRenderer spriteRenderer;

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;

        spriteRenderer.enabled = sprite;
    }


    private void Update()
    {
        transform.LookAt(camera, camera.up);
    }

    private void Awake()
    {
        camera = Camera.main.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
