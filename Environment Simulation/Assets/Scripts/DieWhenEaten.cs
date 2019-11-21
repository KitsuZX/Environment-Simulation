using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieWhenEaten : MonoBehaviour, IEatable
{
    public bool IsAvailableToEat => this && true;
    public Vector3 Position => transform.position;


    public float Eat(float requestedEnergy)
    {
        //TODO: Cuánta energía devuelve un animal? Todo lo que se pide? Un número ajustable en el inspector?

        Destroy(gameObject);

        FindObjectOfType<Ecosystem>().RemoveAnimal(gameObject);

        return requestedEnergy;
    }
}
