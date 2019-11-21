using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BehaviourCommunicator : MonoBehaviour
{
    [SerializeField] private float maxShowDistance = 10;

    private new Transform camera;
    private SpriteRenderer spriteRenderer;

    private bool isHidden;


    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;

        spriteRenderer.enabled = !isHidden && sprite;
    }


    private void Update()
    {
        float distance = Vector3.Distance(transform.position, camera.position);

        if (isHidden && distance < maxShowDistance) Show();
        else if (!isHidden && distance > maxShowDistance) Hide();

        if (!isHidden) transform.LookAt(camera, camera.up);
    }

    private void Show()
    {
        foreach (Transform transform in transform)
        {
            transform.gameObject.SetActive(true);
        }

        spriteRenderer.enabled = true;
        isHidden = false;
    }

    private void Hide()
    {
        foreach (Transform transform in transform)
        {
            transform.gameObject.SetActive(false);
        }

        spriteRenderer.enabled = false;
        isHidden = true;
    }


    private void Awake()
    {
        camera = Camera.main.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Hide();
    }
}
