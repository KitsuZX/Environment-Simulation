using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Genes), typeof(VitalFunctions))]
public class Pregnant : MonoBehaviour
{
    private static Vector3 COMMUNICATION_OFFSET = new Vector3(0, 0.7f, 0);
    private const float COMMUNICATION_SCALE = 0.7f;

    private float timePregnant;
    private int childCount;
    private float gestationPeriodLength;
    private Genes motherGenes;
    private Genes fatherGenes;

    private bool pregnancyStarted = false;

    private SpriteRenderer pregnantIcon;


    private void FixedUpdate()
    {
        if (pregnancyStarted)
        {
            timePregnant += Time.fixedDeltaTime / 60;
        }
        if(timePregnant >= gestationPeriodLength)
        {
            GiveBirth();
            Destroy(this);
        }
    }

    

    public void StartPregnancy(Genes fatherGenes)
    {
        this.fatherGenes = fatherGenes;

        pregnancyStarted = true;
        timePregnant = 0;
        childCount = Mathf.RoundToInt(motherGenes.childCountMean);
        gestationPeriodLength = motherGenes.gestationPeriodLength;
    }
    
    public void GiveBirth()
    {

    }


    private void Awake()
    {
        motherGenes = GetComponent<Genes>();

        GameObject pregnantSignGO = new GameObject();
        pregnantSignGO.name = "Pregnant Sign";

        BehaviourCommunicator communicator = GetComponentInChildren<BehaviourCommunicator>();
        pregnantSignGO.transform.parent = communicator.transform;
        pregnantSignGO.transform.localPosition = COMMUNICATION_OFFSET;
        pregnantSignGO.transform.localScale = new Vector3(COMMUNICATION_SCALE,COMMUNICATION_SCALE, COMMUNICATION_SCALE);

        pregnantIcon = pregnantSignGO.AddComponent<SpriteRenderer>();
        pregnantIcon.sprite = GetComponent<VitalFunctions>().PregnantCommunicationSprite;
        pregnantIcon.sortingOrder = communicator.GetComponent<SpriteRenderer>().sortingOrder + 1;

    }

    private void OnDestroy()
    {
        Destroy(pregnantIcon.gameObject);
    }
}
