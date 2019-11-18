using Panda;
using UnityEngine;

#pragma warning disable 649
[RequireComponent(typeof(AnimalMovement), typeof(Sexuality))]
public class MovementTasks : MonoBehaviour
{
    [SerializeField] private Sprite runAwaySprite;
    [SerializeField] private Sprite goToFoodSprite;
    [SerializeField] private Sprite goToWaterSprite;
    [SerializeField] private Sprite goToPartnerSprite;

    private AnimalMovement animalMovement;

    private Sexuality sexuality;
    private Perceptor perceptor;
    private BehaviourCommunicator communicator;

    [Task]
    public void RunAway()
    {
        animalMovement.FleeFrom(perceptor.GetDangers());
        Task.current.Succeed();

        communicator.SetSprite(runAwaySprite);
    }

    [Task]
    public void GoToFood()
    {
        bool reached = animalMovement.GoTo(perceptor.GetClosestFood().Position);
        Task.current.Complete(reached);

        if (!reached) communicator.SetSprite(goToFoodSprite);
    }

    [Task]
    public void GoToWater()
    {
        bool reached = animalMovement.GoToNearestWaterSource();
        Task.current.Complete(reached);

        if (!reached) communicator.SetSprite(goToWaterSprite);
    }

    [Task]
    public void GoToPartner()
    {
        bool reached = animalMovement.GoTo(sexuality.chosenPartner.transform.position);
        Task.current.Complete(reached);

        if (!reached) communicator.SetSprite(goToPartnerSprite);
    }

    [Task]
    public void MoveRandomly()
    {
        animalMovement.MoveRandom();
        Task.current.Succeed();

        communicator.SetSprite(null);
    }


    private void Awake()
    {
        animalMovement = GetComponent<AnimalMovement>();
        perceptor = GetComponentInChildren<Perceptor>();
        communicator = GetComponentInChildren<BehaviourCommunicator>();
        sexuality = GetComponent<Sexuality>();
    }

}
