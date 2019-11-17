using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Genes))]
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
        gestationPeriodLength = motherGenes.gestationPeriodLength;
    }
    
    public Genes GiveBirth(Genes motherGenes)
    {

        Genes childGenes = new Genes();
        float aux = 0;

        //Media de los padres
        float averageParents = (motherGenes.averageGenes() + fatherGenes.averageGenes())/2;

        //Genes de los padres
        float[] arrayMadre = motherGenes.getArrayGenes();
        float[] arrayPadre = fatherGenes.getArrayGenes();

        //Cálculo de Varianzas
        for (int i = 0; i < 7; i++)
        {
            float xi = (arrayMadre[i] + arrayPadre[i]) / 2;
            aux += (Mathf.Pow((xi - averageParents),2) );
        }
        float var = aux / 2;

        //Offset aleatorio entre varianzas
        float rnd = Random.Range(-var, var);

        //Asignación de elementos 
        childGenes.maxEnergy = rnd + ((arrayMadre[0] + arrayPadre[0]) / 2);
        childGenes.maxHydration = rnd + ((arrayMadre[1] + arrayPadre[1])/ 2);
        childGenes.speed = rnd + ((arrayMadre[2] + arrayPadre[2]) / 2);
        childGenes.childCountMean = rnd + ((arrayMadre[3] + arrayPadre[3])/ 2);
        childGenes.perceptionRadius = rnd + ((arrayMadre[4] + arrayPadre[4]) / 2);
        childGenes.gestationPeriodLength = rnd + ((arrayMadre[5] + arrayPadre[5]) / 2);
        childGenes.sexAppeal = rnd + ((arrayMadre[6] + arrayPadre[6]) / 2);

        return childGenes;
    }


    private void Awake()
    {
        motherGenes = GetComponent<Genes>();
    }
}
