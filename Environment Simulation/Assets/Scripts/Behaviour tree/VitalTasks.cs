using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

[RequireComponent(typeof(VitalFunctions), typeof(Genes))]
public class VitalTasks : MonoBehaviour
{

    [Task]
    public bool DoINeedFood => vitalFunctions.IsHungry;

    [Task]
    public bool DoINeedWater => vitalFunctions.IsThirsty;

    [Task]
    public bool AmIOldEnough => vitalFunctions.CurrentAge > genes.reproductiveAgeRange.x && vitalFunctions.CurrentAge < genes.reproductiveAgeRange.y;


    private VitalFunctions vitalFunctions;
    private Perceptor perceptor;
    private Genes genes;

    private void Awake()
    {
        vitalFunctions = GetComponent<VitalFunctions>();
        genes = GetComponent<Genes>();
        perceptor = GetComponentInChildren<Perceptor>();
    }

    [Task]
    public void AmIPregnant()
    {
        Task.current.Fail();
        //Vital functions -> 
    }
    [Task]
    public void EatFood()
    {
        Task.current.Fail();
        Transform closestFood = perceptor.GetClosestFood();
        //Vital functions -> EatFood
        vitalFunctions.EatFood(closestFood);
    }
    [Task]
    public void DrinkWater()
    {
        Task.current.Fail();
        //Vital functions -> DrinkWater
    }
    [Task]
    public void Breed()
    {
        Task.current.Fail();
        //Vital functions -> Breed
        Perceptor.PerceivedMate pm = perceptor.GetSexiestMate();

        if (!vitalFunctions.isFemale) //Si yo soy chico y la otra es chica, la otra se queda preñada de mi, lets go
        {
            pm.vitalFunctions.GetPregnant(genes);
        }
        
    }


}
