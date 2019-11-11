using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Perceptor : MonoBehaviour
{
    private new SphereCollider collider;

    private List<GameObject> perceptedObjects = new List<GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        perceptedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        perceptedObjects.Remove(other.gameObject);
    }


    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;

        collider.radius = GetComponentInParent<Genes>().perceptionRadius;
    }
}
