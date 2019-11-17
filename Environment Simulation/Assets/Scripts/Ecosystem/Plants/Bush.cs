using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour, IEatable
{
    [Range(0, 100)]
    [SerializeField] private float maxFoodAmount = 100f;
    [SerializeField] private float foodGainedPerSecond = 10f;
    [SerializeField] private float actualFoodAmount;

    [SerializeField] private float perceptableFoodAmount = 15f;

    public bool IsAvailableToEat => actualFoodAmount > perceptableFoodAmount;
    public Vector3 Position => transform.position;


    private void Awake()
    {
        actualFoodAmount = maxFoodAmount;
    }

    public float Eat(float requestedEnergy)
    {
        float foodRetrieved = actualFoodAmount - requestedEnergy;
        foodRetrieved = Mathf.Clamp(foodRetrieved, 0, requestedEnergy);

        actualFoodAmount = foodRetrieved;

        return foodRetrieved;
    }

    private void FixedUpdate()
    {
        if(actualFoodAmount < maxFoodAmount)
        {
            actualFoodAmount += foodGainedPerSecond * Time.fixedDeltaTime;
            actualFoodAmount = Mathf.Clamp(actualFoodAmount, 0, maxFoodAmount);

            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, actualFoodAmount / maxFoodAmount);
        }
    }
}
