using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Gizmos = Popcron.Gizmos;

[RequireComponent(typeof(VitalFunctions))]
public class AIDebugger : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    [Header("Selection")]
    public float coneHeight = 2;
    public Mesh selectedMesh;
    public Material selectedMeshMaterial;
    public Material targetPointMaterial;

    [Header("Gizmo colors")]
    public Color mateColor;
    public Color availableFoodColor;

    [Header("Sliders")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider energySlider;
    [SerializeField] private Slider thirstSlider;

    private bool isSelected = false;

    private Perceptor perceptor;
    private BehaviourCommunicator communicator;
    private Sexuality sexuality;
    private AnimalMovement animalMovement;
    private VitalFunctions vitalFunctions;
    private GenesData genesData;

    #region Selection events
    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        communicator.forceActive = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        communicator.forceActive = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
    #endregion

    private void Update()
    {
        canvas.SetActive(isSelected);

        if (isSelected)
        {
            VisualizeSelected();
            VisualizePerception();
            VisualizeMovement();

            energySlider.value = vitalFunctions.CurrentEnergy;
            thirstSlider.value = vitalFunctions.CurrentHydration;
        }
    }


    private void VisualizeSelected()
    {
        Graphics.DrawMesh(selectedMesh, transform.position + new Vector3(0, coneHeight, 0), Quaternion.Euler(90, 0, 0), selectedMeshMaterial, LayerMask.NameToLayer("Default"));
    }

    private void VisualizePerception()
    {
        Vector3 position = transform.position;

        //Draw the perception radius
        Gizmos.Sphere(position, perceptor.PerceptionRadius, Color.green, false);

        //Draw potential mates
        ICollection<Perceptor.PerceivedMate> mates = perceptor.GetAllPerceivedMates();
        Perceptor.PerceivedMate chosenMate = sexuality.chosenPartner;
        foreach (var perceivedMate in mates)
        {
            Gizmos.Line(position, perceivedMate.transform.position, mateColor, perceivedMate.sexuality != chosenMate.sexuality);
        }

        //Draw food
        ICollection<IEatable> foods = perceptor.GetAllPerceivedFood();
        IEatable closestFood = perceptor.GetClosestFood();
        foreach (var eatable in foods)
        {
            Gizmos.Line(position, eatable.Position, eatable.IsAvailableToEat ? availableFoodColor : Color.black, eatable != closestFood);
        }

        //Draw danger
        ICollection<Transform> dangers = perceptor.GetDangers();
        foreach (var danger in dangers)
        {
            Gizmos.Line(position, danger.position, Color.red, false);
        }
    }

    private void VisualizeMovement()
    {
        Graphics.DrawMesh(selectedMesh, animalMovement.TargetPoint + new Vector3(0, 1, 0), Quaternion.Euler(90, 0, 0), targetPointMaterial, LayerMask.NameToLayer("Default"));
    }

    private void Awake()
    {
        perceptor = GetComponentInChildren<Perceptor>();
        communicator = GetComponentInChildren<BehaviourCommunicator>();
        sexuality = GetComponent<Sexuality>();
        animalMovement = GetComponent<AnimalMovement>();
        vitalFunctions = GetComponent<VitalFunctions>();
        genesData = GetComponent<Genes>().genesData;
    }

    private void Start()
    {
        energySlider.maxValue = genesData.maxEnergy;
        energySlider.minValue = 0;

        thirstSlider.maxValue = genesData.maxHydration;
        thirstSlider.minValue = 0;
    }
}
