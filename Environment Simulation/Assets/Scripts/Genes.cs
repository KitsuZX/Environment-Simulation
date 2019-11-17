using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(VitalFunctions), typeof(AnimalMovement))]
public class Genes : MonoBehaviour
{
    public float lifeExpectancy = 9;
   

    public float SexAppeal => (genesData.maxEnergy + genesData.maxHydration + genesData.speed + genesData.perceptionRadius) / 4;
    public Vector2 reproductiveAgeRange;

    [SerializeField] private AnimationCurve statCurve = null;

    private VitalFunctions vitalFunctions;
    private NavMeshAgent navMeshAgent;
    private Perceptor perceptor;
    public GenesData genesData;
   
    private float AgeStatFactor { get => statCurve.Evaluate(vitalFunctions.CurrentAge / lifeExpectancy); }
        
    private void FixedUpdate()
    {
        float ageFactor = AgeStatFactor;

        navMeshAgent.speed = genesData.speed * ageFactor;
        perceptor.PerceptionRadius = genesData.perceptionRadius * ageFactor;
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
        float[] arrayGenes = new float[]{ genesData.maxEnergy, genesData.maxHydration, genesData.speed, genesData.childCountMean,
                        genesData.perceptionRadius, genesData.gestationPeriodLength}; ;

        return arrayGenes;
    }

}
