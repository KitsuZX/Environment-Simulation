using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercieveTasks : MonoBehaviour
{
    private Perceptor perceptor;

    [Task]
    public bool IsInDanger => perceptor.IsInDanger;


    [Task]
    public void SearchForFood()
    {
        if (perceptor.SeesFood) Task.current.Succeed();
        else
        {
            Task.current.Fail();
            //TODO: MoveRandomly()
        }
    }

    [Task]
    public void SearchForAPartner()
    {
        if (perceptor.SeesPartner) Task.current.Succeed();
        else
        {
            Task.current.Fail();
            //TODO: MoveRandomly()
        }
    }


    private void Awake()
    {
        perceptor = GetComponentInChildren<Perceptor>();
    }
}
