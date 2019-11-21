using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

[RequireComponent(typeof(PandaBehaviour))]
public class Sexuality : MonoBehaviour
{
    public bool IsLookingToBreed => (breedingTree != null) ? breedingTree.status == Status.Succeeded || breedingTree.status == Status.Running : false;
    public Perceptor.PerceivedMate chosenPartner;

    private PandaTree breedingTree;


    private void Start()
    {
        breedingTree = GetComponent<PandaBehaviour>().GetTree("Need to breed");
    }
}
