using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercieveTasks : MonoBehaviour
{

    [Task]
    public void CheckForEnemies()
    {
        //Perceptor -> GetEnemies
    }

    [Task]
    public void FindFood()
    {
        //Perceptor -> GetClosestFood
    }

    [Task]
    public void FindWater()
    {
        //Animal movement -> GoToWater
    }

    [Task]
    public void FindAPartner()
    {
        //Perceptor -> GetBestPartnerToHaveSexWith
    }


}
