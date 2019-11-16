using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTasks : MonoBehaviour
{

    [Task]
    public void RunAway()
    {
        Task.current.Fail();
        //Animal movement -> FleetFrom
    }
    [Task]
    public void GoToFood()
    {
        //Task.current.Fail();
        //Animal movement -> GoTo
    }
    [Task]
    public void GoToWater()
    {
        Task.current.Fail();
        //Animal movement -> GoToWater
    }
    [Task]
    public void GoToPartner()
    {
        Task.current.Fail();
        //Animal movement -> GoTo
    }
    [Task]
    public void MoveRandomly()
    {
        Task.current.Fail();
        //Animal movement -> MoveRandom
    }

}
