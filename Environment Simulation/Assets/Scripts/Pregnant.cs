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
    private Ecosystem ecosystem;


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
        for (int i = 0;i < childCount; i++)
        {
            GenesData genesData = GenesData.Mix(motherGenesData, fatherGenesData);
            genesData.ApplyRandomVariance();
            InstantiateNewAnimal(genesData);
        }
    }

    public void InstantiateNewAnimal(GenesData childGenes)
    {
        GameObject son = Instantiate(gameObject);
        ecosystem.AddAnimal(son);
        son.GetComponent<Genes>().genesData = childGenes;
        son.GetComponent<VitalFunctions>().CurrentAge = gestationPeriodLength;
        Destroy(son.GetComponent<Pregnant>());
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

        ecosystem = FindObjectOfType<Ecosystem>();

    }

    private void OnDestroy()
    {
        foreach (Transform child in pregnantIcon.transform.parent)
        {
            if (child.name == "Pregnant Sign") Destroy(child.gameObject);
        } 
    }
}
