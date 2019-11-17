using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEatable
{
    bool IsAvailableToEat { get; }
    Vector3 Position { get; }

    /// <summary>
    /// Returns the actual amount of energy received from the food.
    /// </summary
    float Eat(float requestedEnergy);
}
