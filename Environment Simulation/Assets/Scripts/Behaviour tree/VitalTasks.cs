using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
public class VitalTasks : MonoBehaviour
{

    private VitalFunctions vitalFunctions;
    private Perceptor perceptor;
    private Genes MyGenes;

    public void Start()
    {
        vitalFunctions = GetComponent<VitalFunctions>();
        perceptor = GetComponent<Perceptor>();
    }

    [Task]
    public void DoINeedFood()
    {
        Task.current.Fail();
        //Vital functions -> currentEnergy
    }
    [Task]
    public void DoINeedWater()
    {
        Task.current.Fail();
        //Vital functions -> currentHidration
    }
    [Task]
    public void AmIOldEnough()
    {
        Task.current.Succeed();
        //Vital functions -> currentAge
    }
    [Task]
    public void AmIPregnant()
    {
        Task.current.Succeed();
        //Vital functions -> 
    }
    [Task]
    public void EatFood(Transform foodPos)
    {
        Task.current.Fail();
        //Vital functions -> EatFood
        vitalFunctions.EatFood(foodPos);
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
            pm.vitalFunctions.GetPregnant(MyGenes);
        }
        
    }


}
