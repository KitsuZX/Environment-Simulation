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
        //Vital functions -> currentEnergy
    }
    [Task]
    public void DoINeedWater()
    {
        //Vital functions -> currentHidration
    }
    [Task]
    public void AmIOldEnough()
    {
        //Vital functions -> currentAge
    }
    [Task]
    public void AmIPregnant()
    {
        //Vital functions -> 
    }
    [Task]
    public void EatFood(Transform foodPos)
    {
        //Vital functions -> EatFood
        vitalFunctions.EatFood(foodPos);
    }
    [Task]
    public void DrinkWater()
    {
        //Vital functions -> DrinkWater
    }
    [Task]
    public void Breed()
    {
        //Vital functions -> Breed
        Perceptor.PerceivedMate pm = perceptor.GetSexiestMate();

        if (!vitalFunctions.isFemale) //Si yo soy chico y la otra es chica, la otra se queda preñada de mi, lets go
        {
            pm.vitalFunctions.GetPregnant(MyGenes);
        }
        
    }


}
