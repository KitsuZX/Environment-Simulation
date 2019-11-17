using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

#pragma warning disable 649
[RequireComponent(typeof(VitalFunctions), typeof(Genes))]
public class VitalTasks : MonoBehaviour
{
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
    public bool IsMale => vitalFunctions.IsMale;
    

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
    public void ImpregnateMate()
    { 
        Perceptor.PerceivedMate pm = perceptor.GetSexiestMate();
        pm.vitalFunctions.GetPregnant(genes.genesData);

        Task.current.Succeed();
    }


    private void Awake()
    {
        vitalFunctions = GetComponent<VitalFunctions>();
        genes = GetComponent<Genes>();
        perceptor = GetComponentInChildren<Perceptor>();
        communicator = GetComponentInChildren<BehaviourCommunicator>();
    }
}
