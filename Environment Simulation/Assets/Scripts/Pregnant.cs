using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Genes), typeof(VitalFunctions))]
public class Pregnant : MonoBehaviour
{
    private static Vector3 COMMUNICATION_OFFSET = new Vector3(0, 0.7f, 0);
    private const float COMMUNICATION_SCALE = 0.7f;

    private float timePregnant;
    private int childCount;
    private float gestationPeriodLength;
    private GenesData motherGenesData;
    private GenesData fatherGenesData;

    private bool pregnancyStarted = false;

    private SpriteRenderer pregnantIcon;


    private void FixedUpdate()
    {
        if (pregnancyStarted)
        {
            timePregnant += Time.fixedDeltaTime / 60;
        }
        if(timePregnant >= gestationPeriodLength)
        {
            GiveBirth();
            Destroy(this);
        }
    }

    public void StartPregnancy(GenesData fatherGenes)
    {
        this.fatherGenesData = fatherGenes;

        pregnancyStarted = true;
        timePregnant = 0;
        childCount = Mathf.RoundToInt(motherGenesData.childCountMean);
        gestationPeriodLength = motherGenesData.gestationPeriodLength;
    }
    
    public void GiveBirth()
    {
        //Se calculan los genes base de cada hijo
        float meanmaxEnergy = mean(motherGenesData.maxEnergy, fatherGenesData.maxEnergy);
        float meanmaxHydration = mean(motherGenesData.maxHydration, fatherGenesData.maxHydration);
        float meanspeed = mean(motherGenesData.speed, fatherGenesData.speed);
        float meanchildCountMean = mean(motherGenesData.childCountMean, fatherGenesData.childCountMean);
        float meanperceptionRadius = mean(motherGenesData.perceptionRadius, fatherGenesData.perceptionRadius);
        float meangestationPeriodLength = mean(motherGenesData.gestationPeriodLength, fatherGenesData.gestationPeriodLength);

        //Asignación de elementos 
        for (int i = 0;i < childCount; i++) //Para cada hijo se calcula el valor base + un random de ese stat
        {
            GenesData childGenesData = new GenesData();
            childGenesData.maxEnergy = meanmaxEnergy + randomVariation(meanmaxEnergy, childGenesData.variance);
            childGenesData.maxHydration = meanmaxHydration + randomVariation(meanmaxHydration, childGenesData.variance);
            childGenesData.speed = meanspeed + randomVariation(meanspeed, childGenesData.variance);
            childGenesData.childCountMean = meanchildCountMean + randomVariation(meanchildCountMean, childGenesData.variance);
            childGenesData.perceptionRadius = meanperceptionRadius + randomVariation(meanperceptionRadius, childGenesData.variance);
            childGenesData.gestationPeriodLength = meangestationPeriodLength + randomVariation(meangestationPeriodLength, childGenesData.variance);

            InstantiateNewAnimal(childGenesData);
        }
        
    }
    public void InstantiateNewAnimal(GenesData childGenes)
    {
        GameObject son = Instantiate(gameObject);
        son.GetComponent<Genes>().genesData = childGenes;
        son.GetComponent<VitalFunctions>().CurrentAge = gestationPeriodLength;
        Destroy(son.GetComponent<Pregnant>());
    }

    public float mean(float mother,float father)
    {
        return (mother + father) / 2;
    }
    
    public float randomVariation(float stat,float variance)
    {
        float random = Random.Range(-variance * stat, variance * stat);
        return random;
    }


    private void Awake()
    {
        motherGenesData = GetComponent<Genes>().genesData;

        GameObject pregnantSignGO = new GameObject();
        pregnantSignGO.name = "Pregnant Sign";

        BehaviourCommunicator communicator = GetComponentInChildren<BehaviourCommunicator>();
        pregnantSignGO.transform.parent = communicator.transform;
        pregnantSignGO.transform.localPosition = COMMUNICATION_OFFSET;
        pregnantSignGO.transform.localScale = new Vector3(COMMUNICATION_SCALE,COMMUNICATION_SCALE, COMMUNICATION_SCALE);

        pregnantIcon = pregnantSignGO.AddComponent<SpriteRenderer>();
        pregnantIcon.sprite = GetComponent<VitalFunctions>().PregnantCommunicationSprite;
        pregnantIcon.sortingOrder = communicator.GetComponent<SpriteRenderer>().sortingOrder + 1;

    }

    private void OnDestroy()
    {
        foreach (Transform child in pregnantIcon.transform.parent)
        {
            if (child.name == "Pregnant Sign") Destroy(child.gameObject);
        } 
    }
}
