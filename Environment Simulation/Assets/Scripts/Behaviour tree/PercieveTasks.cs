using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649
[RequireComponent(typeof(AnimalMovement), typeof(Sexuality))]
public class PercieveTasks : MonoBehaviour
{
    [SerializeField] private Sprite searchForFoodSprite;
    [SerializeField] private Sprite searchForAPartnerSprite;

    private Perceptor perceptor;
    private AnimalMovement animalMovement;
    private BehaviourCommunicator communicator;
    private Sexuality sexuality;


    [Task]
    public bool IsInDanger => perceptor.IsInDanger;

    [Task]//Lo sé. Juro que está bien. Miramos si mi chosen partner busca follar, y si soy su chosen partner.
    public bool IsMyChosenPartnerIntoMe
    {
        get
        {
            Sexuality partnerSexuality = sexuality.chosenPartner.sexuality;
            if (partnerSexuality)
            {
                return partnerSexuality.IsLookingToBreed && partnerSexuality.chosenPartner.sexuality == sexuality;
            }
            else return false;
        }
    }

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
        Task task = Task.current;
        //Walk randomly until we see a potential mate.
        if (perceptor.SeesPotentialPartner)
        {
            //If there are multiple, choose the sexiest one.
            Perceptor.PerceivedMate sexiestMate = perceptor.GetSexiestMate();
            sexuality.chosenPartner = sexiestMate;
            
            task.Succeed();
        }
        else
        {
            //Kepp looking
            animalMovement.MoveRandom();
            communicator.SetSprite(searchForAPartnerSprite);
            task.Fail();
            return;
        }
    }


    private void Awake()
    {
        perceptor = GetComponentInChildren<Perceptor>();
        animalMovement = GetComponent<AnimalMovement>();
        communicator = GetComponentInChildren<BehaviourCommunicator>();
        sexuality = GetComponent<Sexuality>();
    }
}
