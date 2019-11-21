using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalFunctions : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float needThresholdPortion = 0.5f;
    [SerializeField] private float hydrationLostPerSecond = 0.1f;
    public Sprite PregnantCommunicationSprite => _pregnantCommunicationSprite;
    [SerializeField] private Sprite _pregnantCommunicationSprite;
    [SerializeField] private AnimationCurve curveGrowUp;

    public float CurrentAge { get; set; }
    public bool IsMale { get; private set; }
    
    public bool IsHungry => 1 - (CurrentEnergy / genes.genesData.maxEnergy) > needThresholdPortion;
    public bool IsThirsty => 1 - (CurrentHydration/ genes.genesData.maxHydration) > needThresholdPortion;
    public bool IsOldEnoughForSex => CurrentAge > genes.reproductiveAgeRange.x && CurrentAge < genes.reproductiveAgeRange.y;
    public bool IsPregnant => pregnancy;

    public float CurrentEnergy { get; private set; }
    public float CurrentHydration { get; private set; }
    private Genes genes;
    private Pregnant pregnancy;

    [SerializeField] private EnergyFactors energyFactors;

    public void EatFood(IEatable food)
    {
        //SALE EL LOGO DE COMIENDO///
        /////////////////////////////
        float maxEnergyToGet = genes.genesData.maxEnergy - CurrentEnergy;

        float energyEarned = food.Eat(maxEnergyToGet);
        CurrentEnergy += energyEarned;
    }

    public void DrinkWater()
    {
        //SALE EL LOGO DE BEBIENDO///
        ////////////////////////////
        float maxWaterToGet = genes.genesData.maxHydration - CurrentHydration;
        CurrentHydration += maxWaterToGet;
    }

    public void Impregnate(GenesData fatherGenesData)
    {
        //SALE EL LOGO DE FOLLANDO///
        ////////////////////////////
        pregnancy = gameObject.AddComponent<Pregnant>();
        pregnancy.StartPregnancy(fatherGenesData);
    }

    [ContextMenu("Dejar preñá")]
    public void DebugImpregnate()
    {
        Impregnate(new GenesData());
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        growUp(dt);

        CurrentEnergy -= (genes.genesData.speed * genes.genesData.speed) * energyFactors.speedFactor * dt;
        CurrentEnergy -= genes.genesData.perceptionRadius * energyFactors.perceptionRadiusFactor * dt;
        if (IsPregnant) CurrentEnergy -= pregnancy.ChildCount * energyFactors.pregnantEnergyLost * dt;

        CurrentHydration -= hydrationLostPerSecond * dt;

        if (CurrentEnergy < 0 || CurrentHydration < 0)
        {
            Destroy(gameObject);
            FindObjectOfType<Ecosystem>().RemoveAnimal(gameObject);
        }
    }

    private void growUp(float dt)
    {
        CurrentAge += dt / 60;
        float size = curveGrowUp.Evaluate(CurrentAge / genes.lifeExpectancy);
        transform.localScale = new Vector3(size, size, size);
    }

    private void Awake()
    {
        genes = GetComponent<Genes>();
        //CurrentAge = 0;
        IsMale = Random.value < 0.5f;
       
    }
    private void Start() 
    {
        CurrentEnergy = genes.genesData.maxEnergy;
        CurrentHydration = genes.genesData.maxHydration;
    }
}
