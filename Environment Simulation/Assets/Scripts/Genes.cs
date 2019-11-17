using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(VitalFunctions), typeof(AnimalMovement))]
public class Genes : MonoBehaviour
{
    public float lifeExpectancy = 9;

    public float maxEnergy;
    public float maxHydration;
    public float speed;
    public float childCountMean;
    public float perceptionRadius;
    public float gestationPeriodLength;
    public Vector2 reproductiveAgeRange;

    public float SexAppeal => (maxEnergy + maxHydration + speed + perceptionRadius) / 4;

    

    [SerializeField] private AnimationCurve statCurve = null;

    private VitalFunctions vitalFunctions;
    private NavMeshAgent navMeshAgent;
    private Perceptor perceptor;

   
    private float AgeStatFactor { get => statCurve.Evaluate(vitalFunctions.CurrentAge / lifeExpectancy); }
        
    private void FixedUpdate()
    {
        float ageFactor = AgeStatFactor;

        navMeshAgent.speed = speed * ageFactor;
        perceptor.PerceptionRadius = perceptionRadius * ageFactor;
    }

    private void Awake()
    {
        vitalFunctions = GetComponent<VitalFunctions>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        perceptor = GetComponentInChildren<Perceptor>();
    }
    public float[] getArrayGenes()
    {
        //Valor de los genes en array
        float[] arrayGenes = new float[]{lifeExpectancy, maxEnergy, maxHydration, speed, childCountMean,
                        perceptionRadius, gestationPeriodLength, sexAppeal}; ;

        return arrayGenes;
    }
    public float averageGenes()
    {
        //media
        float sum = lifeExpectancy + maxEnergy + maxHydration + speed + childCountMean +
                        perceptionRadius + gestationPeriodLength + sexAppeal;
        float average = sum / 7;

        return average;
    }
}
