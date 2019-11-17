using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Genes)), AddComponentMenu("Hidden")]
public class Pregnant : MonoBehaviour
{
    private float timePregnant;
    private int childCount;
    private float gestationPeriodLength;
    private Genes motherGenes;
    private Genes fatherGenes;

    private bool pregnancyStarted = false;


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

    

    public void StartPregnancy(Genes fatherGenes)
    {
        this.fatherGenes = fatherGenes;

        pregnancyStarted = true;
        timePregnant = 0;
        childCount = Mathf.RoundToInt(motherGenes.childCountMean);
        gestationPeriodLength = Mathf.RoundToInt(motherGenes.gestationPeriodLength);
    }
    
    public void GiveBirth()
    {

    }


    private void Awake()
    {
        motherGenes = GetComponent<Genes>();
    }
}
