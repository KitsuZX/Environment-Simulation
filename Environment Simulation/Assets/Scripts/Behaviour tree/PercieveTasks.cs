using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649
[RequireComponent(typeof(AnimalMovement))]
public class PercieveTasks : MonoBehaviour
{
    [SerializeField] private Sprite searchForFoodSprite;
    [SerializeField] private Sprite searchForAPartnerSprite;

    private Perceptor perceptor;
    private AnimalMovement animalMovement;
    private BehaviourCommunicator communicator;


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

            communicator.SetSprite(searchForFoodSprite);
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

            communicator.SetSprite(searchForAPartnerSprite);
        }
    }


    private void Awake()
    {
        perceptor = GetComponentInChildren<Perceptor>();
        animalMovement = GetComponent<AnimalMovement>();
        communicator = GetComponentInChildren<BehaviourCommunicator>();
    }
}
