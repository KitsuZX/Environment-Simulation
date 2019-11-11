using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{

    [SerializeField] private float maxFoodAmount = 5f;
    private float actualFoodAmount;

    private void Awake()
    {
        maxFoodAmount = actualFoodAmount;
    }


}
