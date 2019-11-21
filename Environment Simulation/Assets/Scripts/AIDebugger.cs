using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gizmos = Popcron.Gizmos;

[RequireComponent(typeof(VitalFunctions))]
public class AIDebugger : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public Mesh selectedMesh;
    public Material selectedMeshMaterial;
    public Color mateColor;

    private bool isSelected = false;

    private Perceptor perceptor;
    private BehaviourCommunicator communicator;
    private Sexuality sexuality;

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
        if (isSelected)
        {
            VisualizeSelected();
            VisualizePerception();
        }
    }


    private void VisualizeSelected()
    {
        Graphics.DrawMesh(selectedMesh, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(90, 0, 0), selectedMeshMaterial, LayerMask.NameToLayer("Default"));
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
    }

    private void Awake()
    {
        perceptor = GetComponentInChildren<Perceptor>();
        communicator = GetComponentInChildren<BehaviourCommunicator>();
        sexuality = GetComponent<Sexuality>();
    }
}
