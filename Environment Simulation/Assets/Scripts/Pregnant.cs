using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pregnant : MonoBehaviour
{

    private float timePregnant;
    private int childCount;
    private Genes parentGenes;
    private float gestationPeriodLength;

    private bool pregnancyStarted = false;

    public void Start()
    {
        timePregnant = 0;
        childCount = Mathf.RoundToInt(GetComponent<Genes>().childCountMean);
        gestationPeriodLength = Mathf.RoundToInt(GetComponent<Genes>().gestationPeriodLength);
    }

    public void Update()
    {
        if (pregnancyStarted)
        {
            float dt = Time.fixedDeltaTime;

            timePregnant += dt / 60;
        }
        if(timePregnant >= gestationPeriodLength)
        {
            GiveBirth(GetComponent<Genes>());
            Destroy(this);
        }
    }

    public void AssignParentGenes(Genes parent)
    {
        parentGenes = parent;
    }

    public void StartPregnancy()
    {
        pregnancyStarted = true;
    }
    
    public void GiveBirth(Genes genes)
    {

    }
}
