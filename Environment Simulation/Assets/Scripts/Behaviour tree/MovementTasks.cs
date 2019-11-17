using Panda;
using UnityEngine;

#pragma warning disable 649
[RequireComponent(typeof(AnimalMovement))]
public class MovementTasks : MonoBehaviour
{
    [SerializeField] private Sprite runAwaySprite;
    [SerializeField] private Sprite goToFoodSprite;
    [SerializeField] private Sprite goToWaterSprite;
    [SerializeField] private Sprite goToPartnerSprite;

    [SerializeField] private Sprite breedSprite;
    [SerializeField] private Sprite eatFoodSprite;
    [SerializeField] private Sprite drinkWaterSprite;

    private AnimalMovement animalMovement;
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

        if (reached) communicator.SetSprite(eatFoodSprite);
        else communicator.SetSprite(goToFoodSprite);
    }

    [Task]
    public void GoToWater()
    {
        bool reached = animalMovement.GoToNearestWaterSource();
        Task.current.Complete(reached);

        if (reached) communicator.SetSprite(drinkWaterSprite);
        else communicator.SetSprite(goToWaterSprite);
    }

    [Task]
    public void GoToPartner()
    {
        bool reached = animalMovement.GoTo(perceptor.GetSexiestMate().transform.position);
        Task.current.Complete(reached);

        if (reached) communicator.SetSprite(breedSprite);
        else communicator.SetSprite(goToPartnerSprite);
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
    }

}
