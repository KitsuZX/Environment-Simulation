using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalFunctions : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float needThresholdPortion = 0.5f;

    public float CurrentAge { get; private set; }
    public bool IsMale { get; private set; }

    public bool IsHungry => 1 - (currentEnergy / genes.maxEnergy) > needThresholdPortion;
    public bool IsThirsty => 1 - (currentHydration/ genes.maxHydration) > needThresholdPortion;
    public bool IsOldEnoughForSex => CurrentAge > genes.reproductiveAgeRange.x && CurrentAge < genes.reproductiveAgeRange.y;
    public bool IsPregnant => pregnancy;

    private float currentEnergy;
    private float currentHydration;
    private Genes genes;
    private Pregnant pregnancy;

    [SerializeField] private float energyLostPerSecond = 0.1f;
    [SerializeField] private float hydrationLostPerSecond = 0.1f;

    public Sprite PregnantCommunicationSprite => _pregnantCommunicationSprite;
    [SerializeField] private Sprite _pregnantCommunicationSprite;


    public void EatFood(IEatable food)
    {
        //SALE EL LOGO DE COMIENDO///
        /////////////////////////////
        float maxEnergyToGet = genes.maxEnergy - currentEnergy;

        float energyEarned = food.Eat(maxEnergyToGet);
        currentEnergy += energyEarned;
    }

    public void DrinkWater()
    {
        //SALE EL LOGO DE BEBIENDO///
        ////////////////////////////
        float maxWaterToGet = genes.maxHydration - currentHydration;
        currentHydration += maxWaterToGet;
    }

    public void GetPregnant(Genes fatherGenes)
    {
        //SALE EL LOGO DE FOLLANDO///
        ////////////////////////////
        pregnancy = gameObject.AddComponent<Pregnant>();
        pregnancy.StartPregnancy(fatherGenes);
    }

    [ContextMenu("Dejar preñá")]
    public void DebugGetPregnant()
    {
        GetPregnant(null);
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        CurrentAge += dt / 60;
        currentEnergy -= energyLostPerSecond * dt;
        currentHydration -= hydrationLostPerSecond * dt;

        if (currentEnergy < 0 || currentHydration < 0)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        genes = GetComponent<Genes>();
        CurrentAge = 0;

        IsMale = Random.value < 0.5f;
    }

    private void Start() 
    {
        currentEnergy = genes.maxEnergy;
        currentHydration = genes.maxHydration;
    }
}
