using Panda;
using UnityEngine;

[RequireComponent(typeof(AnimalMovement))]
public class MovementTasks : MonoBehaviour
{
    private AnimalMovement animalMovement;
    private Perceptor perceptor;
    private BehaviourCommunicator communicator;

    [Task]
    public void RunAway()
    {
        animalMovement.FleeFrom(perceptor.GetDangers());
        Task.current.Succeed();
    }

    [Task]
    public void GoToFood()
    {
        bool reached = animalMovement.GoTo(perceptor.GetClosestFood().Position);
        Task.current.Complete(reached);
    }

    [Task]
    public void GoToWater()
    {
        Task.current.Complete(animalMovement.GoToNearestWaterSource());
    }

    [Task]
    public void GoToPartner()
    {
        bool reached = animalMovement.GoTo(perceptor.GetSexiestMate().transform.position);
        Task.current.Complete(reached);
    }

    [Task]
    public void MoveRandomly()
    {
        animalMovement.MoveRandom();
        Task.current.Succeed();
    }


    private void Awake()
    {
        animalMovement = GetComponent<AnimalMovement>();
        perceptor = GetComponentInChildren<Perceptor>();
        communicator = GetComponentInChildren<BehaviourCommunicator>();
    }

}
