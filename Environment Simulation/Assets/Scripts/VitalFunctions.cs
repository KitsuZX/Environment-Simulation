﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalFunctions : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float needThresholdPortion = 0.5f;

    public float CurrentAge { get; private set; }

    public bool IsHungry { get => 1 - (CurrentEnergy / genes.maxEnergy) > needThresholdPortion; }
    public bool IsThirsty { get => 1 - (CurrentHydration/ genes.maxHydration) > needThresholdPortion; }


    private float CurrentEnergy;
    private float CurrentHydration;
    private Genes genes;

    public bool isFemale;

    private float EnergyLostPerSecond = 0.1f;
    private float HydrationLostPerSecond = 0.1f;

    public void Start() 
    {
        genes = GetComponent<Genes>();
        CurrentAge = 0;
        CurrentEnergy = genes.maxEnergy;
        CurrentHydration = genes.maxHydration;
    }

    public void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        CurrentAge += dt/60;
        CurrentEnergy = Mathf.Max(CurrentEnergy - EnergyLostPerSecond * dt, 0); ;
        CurrentHydration = Mathf.Max(CurrentHydration - HydrationLostPerSecond * dt, 0);
    }
   
    public void EatFood(Transform foodPos)
    {
        //SALE EL LOGO DE COMIENDO///
        /////////////////////////////
        float maxEnergyToGet = genes.maxEnergy - CurrentEnergy;

        //TODO: Que los zorros coman conejos, no fruta
        float energyEarned = foodPos.GetComponent<Bush>().GetFoodFromBush(maxEnergyToGet);
        CurrentEnergy += energyEarned;
    }

    public void DrinkWater()
    {
        //SALE EL LOGO DE BEBIENDO///
        ////////////////////////////
        float maxWaterToGet = genes.maxHydration - CurrentHydration;
        CurrentHydration += maxWaterToGet;
    }
  
    public void GetPregnant(Genes pm)
    {
        //SALE EL LOGO DE FOLLANDO///
        ////////////////////////////
        
        Pregnant pregnant = gameObject.AddComponent<Pregnant>();
        pregnant.StartPregnancy();
        pregnant.AssignParentGenes(pm);

    }

}
