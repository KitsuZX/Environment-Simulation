using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

#pragma warning disable 649
[RequireComponent(typeof(VitalFunctions), typeof(Genes))]
public class VitalTasks : MonoBehaviour
{
    [SerializeField] private float taskDuration = 2;

    private VitalFunctions vitalFunctions;
    private Perceptor perceptor;
    private Genes genes;
    private BehaviourCommunicator communicator;

    [Task]
    public bool IsHungry => vitalFunctions.IsHungry;

    [Task]
    public bool IsThirsty => vitalFunctions.IsThirsty;

    [Task]
    public bool IsOldEnoughForSex => vitalFunctions.IsOldEnoughForSex;

    [Task]
    public bool IsPregnant => vitalFunctions.IsPregnant;

    [Task]
    public bool IsMale => vitalFunctions.IsMale;
    

    [Task]
    public void EatFood()
    {
        Task task = Task.current;
        WaitTaskContext context;

        //Get the context, initializing if necessary
        if (Task.current.isStarting)
        {
            context = new WaitTaskContext(perceptor.GetClosestFood(), taskDuration);
        }
        else
        {
            context = (WaitTaskContext) Task.current.item;
        }
            
        //Interrupt if food is no longer available
        if (!(context.context is IEatable food) || !food.IsAvailableToEat)
        {
            task.Fail();
            return;
        }
        
        //Advance the wait
        context.timeLeft -= Time.deltaTime;
        task.item = context;
        if (Task.isInspected) task.debugInfo = context.FormattedTimeLeft;

        //Check for success
        if (context.timeLeft <= 0)
        {
            vitalFunctions.EatFood(food);
            Task.current.Succeed();
        }
    }

    [Task]
    public void DrinkWater()
    {
        Task task = Task.current;
        WaitTaskContext context;

        //Get the context, initializing if necessary
        if (Task.current.isStarting)
        {
            context = new WaitTaskContext(null, taskDuration);
        }
        else
        {
            context = (WaitTaskContext)Task.current.item;
        }

        //Advance the wait
        context.timeLeft -= Time.deltaTime;
        task.item = context;
        if (Task.isInspected) task.debugInfo = context.FormattedTimeLeft;

        //Check for success
        if (context.timeLeft <= 0)
        {
            vitalFunctions.DrinkWater();
            Task.current.Succeed();
        }
    }

    [Task]
    public void Breed()
    {
        Task task = Task.current;
        WaitTaskContext context;

        //Get the context, initializing if necessary
        if (Task.current.isStarting)
        {
            context = new WaitTaskContext(perceptor.GetSexiestMate(), taskDuration);
        }
        else
        {
            context = (WaitTaskContext)Task.current.item;
        }

        //Interrupt if the mate dies
        if (!(context.context is Perceptor.PerceivedMate mate) || !mate.vitalFunctions)
        {
            task.Fail();
            return;
        }

        //Advance the wait
        context.timeLeft -= Time.deltaTime;
        task.item = context;
        if (Task.isInspected) task.debugInfo = context.FormattedTimeLeft;

        //Check for success
        if (context.timeLeft <= 0)
        {
            if (vitalFunctions.IsMale) mate.vitalFunctions.Impregnate(genes.genesData);

            Task.current.Succeed();
        }
    }


    private void Awake()
    {
        vitalFunctions = GetComponent<VitalFunctions>();
        genes = GetComponent<Genes>();
        perceptor = GetComponentInChildren<Perceptor>();
        communicator = GetComponentInChildren<BehaviourCommunicator>();
    }

    private struct WaitTaskContext
    {
        public object context;
        public float timeLeft;

        public string FormattedTimeLeft => string.Format("t-{0:0.000}", timeLeft);

        public WaitTaskContext(object context, float timeLeft)
        {
            this.context = context;
            this.timeLeft = timeLeft;
        }
    }
}
