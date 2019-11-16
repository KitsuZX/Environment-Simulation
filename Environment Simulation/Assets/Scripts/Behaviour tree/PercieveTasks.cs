using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimalMovement))]
public class PercieveTasks : MonoBehaviour
{
    private Perceptor perceptor;
    private AnimalMovement animalMovement;

    [Task]
    public bool IsInDanger => perceptor.IsInDanger;


    [Task]
    public void SearchForFood()
    {
        if (perceptor.SeesFood) Task.current.Succeed();
        else
        {
            animalMovement.MoveRandom();
            Task.current.Fail();
        }
    }

    [Task]
    public void SearchForAPartner()
    {
        if (perceptor.SeesPartner) Task.current.Succeed();
        else
        {
            animalMovement.MoveRandom();
            Task.current.Fail();
        }
    }


    private void Awake()
    {
        perceptor = GetComponentInChildren<Perceptor>();
        animalMovement = GetComponent<AnimalMovement>();
    }
}
