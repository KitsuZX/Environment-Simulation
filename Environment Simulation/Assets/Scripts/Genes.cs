using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VitalFunctions))]
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
    public float sexAppeal;

    [SerializeField] private AnimationCurve statCurve = null;

    private VitalFunctions vitalFunctions;
    private Perceptor perceptor;

   
    private float AgeStatFactor { get => statCurve.Evaluate(vitalFunctions.CurrentAge / lifeExpectancy); }
        
    private void FixedUpdate()
    {
        float ageFactor = AgeStatFactor;
        //TODO: UpdateMovementSpeed
        perceptor.PerceptionRadius = perceptionRadius * ageFactor;
    }

    private void Awake()
    {
        vitalFunctions = GetComponent<VitalFunctions>();
        perceptor = GetComponentInChildren<Perceptor>();
    }
}
