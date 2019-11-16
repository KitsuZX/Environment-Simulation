using Panda;
using UnityEngine;

[RequireComponent(typeof(AnimalMovement))]
public class MovementTasks : MonoBehaviour
{
    private AnimalMovement animalMovement;

    [Task]
    public void RunAway()
    {
        Task.current.Succeed();
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
        Task.current.Succeed();
        animalMovement.MoveRandom();
    }

    private void Awake()
    {
        animalMovement = GetComponent<AnimalMovement>();
    }

}
