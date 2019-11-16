using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float maxFoodAmount = 100f;
    [SerializeField] private float foodGainedPerSecond = 10f;
    [SerializeField] private float actualFoodAmount;
       

    private void Awake()
    {
        actualFoodAmount = maxFoodAmount;     
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
    /// <summary>
    /// Returns the actual amount of food the animal gets from the bush
    /// </summary>
    /// <param name="amountAskedFor">Amount of food the animal is asking</param>
    /// <returns></returns>
    public float GetFoodFromBush(float amountAskedFor)
    {
        float foodRetrieved = actualFoodAmount - amountAskedFor;
        foodRetrieved = Mathf.Clamp(foodRetrieved, 0, amountAskedFor);

        actualFoodAmount = foodRetrieved;

        return foodRetrieved;        
    }


}
