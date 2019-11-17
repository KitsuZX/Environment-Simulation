using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

[RequireComponent(typeof(VitalFunctions), typeof(Genes))]
public class VitalTasks : MonoBehaviour
{

    private VitalFunctions vitalFunctions;
    private Perceptor perceptor;
    private Genes genes;

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
    }

    [Task]
    public void DrinkWater()
    {
        vitalFunctions.DrinkWater();
        Task.current.Succeed();
    }

    [Task]
    public void Breed()
    { 
        Perceptor.PerceivedMate pm = perceptor.GetSexiestMate();

        if (!vitalFunctions.isFemale) //Si yo soy chico y la otra es chica, la otra se queda preñada de mi, lets go
        {
            pm.vitalFunctions.GetPregnant(genes);
        }

        Task.current.Succeed();
    }


    private void Awake()
    {
        vitalFunctions = GetComponent<VitalFunctions>();
        genes = GetComponent<Genes>();
        perceptor = GetComponentInChildren<Perceptor>();
    }
}
