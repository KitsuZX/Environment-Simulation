using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalFunctions : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float needThresholdPortion = 0.5f;
    [SerializeField] private float energyLostPerSecond = 0.1f;
    [SerializeField] private float hydrationLostPerSecond = 0.1f;
    public Sprite PregnantCommunicationSprite => _pregnantCommunicationSprite;
    [SerializeField] private Sprite _pregnantCommunicationSprite;
    [SerializeField] private AnimationCurve curveGrowUp;

    public Vector3 minimumScale;
    public Vector3 maxScale;

    public float CurrentAge { get; private set; }
    public bool IsMale { get; private set; }
    
    public bool IsHungry => 1 - (currentEnergy / genes.genesData.maxEnergy) > needThresholdPortion;
    public bool IsThirsty => 1 - (currentHydration/ genes.genesData.maxHydration) > needThresholdPortion;
    public bool IsOldEnoughForSex => CurrentAge > genes.reproductiveAgeRange.x && CurrentAge < genes.reproductiveAgeRange.y;
    public bool IsPregnant => pregnancy;

    [System.NonSerialized] public Perceptor.PerceivedMate chosenPartner;

    private float currentEnergy;
    private float currentHydration;
    private Genes genes;
    private Pregnant pregnancy;

    public void EatFood(IEatable food)
    {
        //SALE EL LOGO DE COMIENDO///
        /////////////////////////////
        float maxEnergyToGet = genes.genesData.maxEnergy - currentEnergy;

        float energyEarned = food.Eat(maxEnergyToGet);
        currentEnergy += energyEarned;
    }

    public void DrinkWater()
    {
        //SALE EL LOGO DE BEBIENDO///
        ////////////////////////////
        float maxWaterToGet = genes.genesData.maxHydration - currentHydration;
        currentHydration += maxWaterToGet;
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
        currentEnergy -= energyLostPerSecond * dt;
        currentHydration -= hydrationLostPerSecond * dt;

        if (currentEnergy < 0 || currentHydration < 0)
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
        CurrentAge = 0;

        IsMale = Random.value < 0.5f;
    }
    private void Start() 
    {
        currentEnergy = genes.genesData.maxEnergy;
        currentHydration = genes.genesData.maxHydration;
    }
}
