using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
public class VitalTasks : MonoBehaviour
{

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
    public void EatFood()
    {
        Task.current.Fail();
        //Vital functions -> EatFood
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
    }


}
