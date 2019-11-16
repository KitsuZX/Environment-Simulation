using Panda;
using UnityEngine;

[RequireComponent(typeof(AnimalMovement))]
public class MovementTasks : MonoBehaviour
{
    private AnimalMovement animalMovement;
    private Perceptor perceptor;

    [Task]
    public void RunAway()
    {
        Task.current.Succeed();
        //TODO: this
    }
    [Task]
    public void GoToFood()
    {
        bool reached = animalMovement.GoTo(perceptor.GetClosestFood().position);
        Task.current.Complete(reached);
    }
    [Task]
    public void GoToWater()
    {
        Task.current.Fail();
        //TODO: Saber dónde está el agua
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
    }

}
