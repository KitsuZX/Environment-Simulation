using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

#pragma warning disable 649
[RequireComponent(typeof(VitalFunctions), typeof(Genes))]
public class VitalTasks : MonoBehaviour
{
    [SerializeField] private Sprite eatFoodSprite;
    [SerializeField] private Sprite drinkWaterSprite;
    [SerializeField] private Sprite breedSprite;


    private VitalFunctions vitalFunctions;
    private Perceptor perceptor;
    private Genes genes;
    private BehaviourCommunicator communicator;

    [Task]
    public bool IsHungry => vitalFunctions.IsHungry;

    [Task]
    public bool IsThirsty => vitalFunctions.IsThirsty;

    [Task]
    public bool IsOldEnoughForSex => vitalFunctions.IsOldEnoughForSex;

    [Task]
    public bool IsPregnant => vitalFunctions.IsPregnant;
    

    [Task]
    public void EatFood()
    {
        IEatable closestFood = perceptor.GetClosestFood();
        vitalFunctions.EatFood(closestFood);
        Task.current.Succeed();

        communicator.SetSprite(eatFoodSprite);
    }

    [Task]
    public void DrinkWater()
    {
        vitalFunctions.DrinkWater();
        Task.current.Succeed();

        communicator.SetSprite(drinkWaterSprite);
    }

    [Task]
    public void Breed()
    { 
        Perceptor.PerceivedMate pm = perceptor.GetSexiestMate();

        if (!vitalFunctions.IsFemale) //Si yo soy chico y la otra es chica, la otra se queda preñada de mi, lets go
        {
            pm.vitalFunctions.GetPregnant(genes.genesData);
        }

        Task.current.Succeed();

        communicator.SetSprite(breedSprite);
    }


    private void Awake()
    {
        vitalFunctions = GetComponent<VitalFunctions>();
        genes = GetComponent<Genes>();
        perceptor = GetComponentInChildren<Perceptor>();
        communicator = GetComponentInChildren<BehaviourCommunicator>();
    }
}
