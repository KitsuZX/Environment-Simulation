using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalFunctions : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float needThresholdPortion = 0.5f;

    public float CurrentAge { get; private set; }

    public bool IsHungry => 1 - (CurrentEnergy / genes.maxEnergy) > needThresholdPortion;
    public bool IsThirsty => 1 - (CurrentHydration/ genes.maxHydration) > needThresholdPortion;
    public bool IsOldEnoughForSex => CurrentAge > genes.reproductiveAgeRange.x && CurrentAge < genes.reproductiveAgeRange.y;
    public bool IsPregnant => pregnancy;

    private float CurrentEnergy;
    private float CurrentHydration;
    private Genes genes;
    private Pregnant pregnancy;

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
   
    public void EatFood(IEatable food)
    {
        //SALE EL LOGO DE COMIENDO///
        /////////////////////////////
        float maxEnergyToGet = genes.maxEnergy - CurrentEnergy;

        float energyEarned = food.Eat(maxEnergyToGet);
        CurrentEnergy += energyEarned;
    }

    public void DrinkWater()
    {
        //SALE EL LOGO DE BEBIENDO///
        ////////////////////////////
        float maxWaterToGet = genes.maxHydration - CurrentHydration;
        CurrentHydration += maxWaterToGet;
    }
  
    public void GetPregnant(Genes fatherGenes)
    {
        //SALE EL LOGO DE FOLLANDO///
        ////////////////////////////
        pregnancy = gameObject.AddComponent<Pregnant>();
        pregnancy.StartPregnancy(fatherGenes);
    }

}
