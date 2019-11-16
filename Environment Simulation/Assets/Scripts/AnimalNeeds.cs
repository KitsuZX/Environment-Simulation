using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Genes))]
public class AnimalNeeds : MonoBehaviour
{
    private Genes genes;

    [SerializeField, Range(0, 1)] private float energyDecayPerSecond = 0.1f;
    [SerializeField, Range(0, 1)] private float hidrationDecayPerSecond = 0.1f;

    public float CurrentAge { get; private set; }
    public float CurrentEnergy { get; private set; }
    public float CurrentHidration { get; private set; }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        CurrentAge += dt / 60;
        CurrentEnergy = Mathf.Max(CurrentEnergy - energyDecayPerSecond * dt, 0);
        CurrentHidration = Mathf.Max(CurrentHidration - hidrationDecayPerSecond * dt, 0);
    }

    private void Awake()
    {
        genes = GetComponent<Genes>();

        CurrentAge = 0;
        CurrentEnergy = genes.maxEnergy;
        CurrentHidration = genes.maxHydration;
    }
}
