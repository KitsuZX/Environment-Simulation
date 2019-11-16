using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Perceptor : MonoBehaviour
{
    private const string RABBIT_TAG = "Rabbit";
    private const string FOX_TAG = "Fox";
    private const string BUSH_TAG = "Bush";


    [SerializeField] private PerceiveeType rabbitPerception = PerceiveeType.Irrelevant;
    [SerializeField] private PerceiveeType foxPerception = PerceiveeType.Irrelevant;
    [SerializeField] private PerceiveeType bushPerception = PerceiveeType.Irrelevant;

    public bool IsInDanger { get => perceivedDangers.Count > 0; }
    public bool SeesFood { get => perceivedFood.Count > 0; }
    public bool SeesPartner { get => perceivedMates.Count > 0; }


    private new SphereCollider collider;
    private new Transform transform;

    //Los objetos percibidos se guardan en un diccionario. La clave es el GameObject para poder identificarlo cuando se salga.
    //El valor es la información que necesitamos sobre este tipo de objeto percibido.
    private Dictionary<GameObject, Transform> perceivedDangers = new Dictionary<GameObject, Transform>();
    private Dictionary<GameObject, Transform> perceivedFood = new Dictionary<GameObject, Transform>();
    private Dictionary<GameObject, PerceivedMate> perceivedMates = new Dictionary<GameObject, PerceivedMate>();


    public PerceiveeType GetPerceiveeType(GameObject gameObject)
    {
        switch (gameObject.tag)
        {
            case RABBIT_TAG:
                return rabbitPerception;
            case FOX_TAG:
                return foxPerception;
            case BUSH_TAG:
                return bushPerception;
            default:
                return PerceiveeType.Irrelevant;
        }
    }

    public Transform GetClosestFood()
    {
        Vector3 myPos = transform.position;

        Transform closest = null
;
        float closestDistance = float.MaxValue;
        float distance;

        foreach (Transform transform in perceivedFood.Values)
        {
            distance = (myPos - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closest = transform;
            }
        }

        return closest;
    }

    //Devuelve IEnumerable porque así no hace falta copiar a un array, se pasa directamente lo que contiene el diccionario.
    public IEnumerable<Transform> GetDangers()
    {
        return perceivedDangers.Values;
    }

    public PerceivedMate GetSexiestMate()
    {
        PerceivedMate sexiestMate = new PerceivedMate();
        float highestSexAppeal = float.MinValue;
        float sexAppeal;

        foreach (PerceivedMate mate in perceivedMates.Values)
        {
            sexAppeal = mate.genes.sexAppeal;
            if (sexAppeal > highestSexAppeal)
            {
                highestSexAppeal = sexAppeal;
                sexiestMate = mate;
            }
        }

        return sexiestMate;
    }


    #region Registration & Unregistration
    private void OnTriggerEnter(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        AddToPerceived(other.gameObject, GetPerceiveeType(other.gameObject));
    }

    private void AddToPerceived(GameObject perceivee, PerceiveeType perceiveeType)
    {
        switch (perceiveeType)
        {
            case PerceiveeType.Food:
                perceivedFood.Add(perceivee, perceivee.transform);
                break;
            case PerceiveeType.Danger:
                perceivedDangers.Add(perceivee, perceivee.transform);
                break;
            case PerceiveeType.Mate:
                //TODO: Comprobar si es del sexo opuesto. Añadir sólo si lo es.
                perceivedMates.Add(perceivee, new PerceivedMate
                {
                    transform = perceivee.transform,
                    genes = perceivee.GetComponent<Genes>()
                });
                break;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        RemoveFromPerceived(other.gameObject, GetPerceiveeType(other.gameObject));

        print(this);
    }

    private void RemoveFromPerceived(GameObject perceivee, PerceiveeType perceiveeType)
    {
        switch (perceiveeType)
        {
            case PerceiveeType.Food:
                perceivedFood.Remove(perceivee);
                break;
            case PerceiveeType.Danger:
                perceivedDangers.Remove(perceivee);
                break;
            case PerceiveeType.Mate:
                perceivedMates.Remove(perceivee);
                break;
        }
    }
    #endregion


    private void Start()
    {
        transform = GetComponent<Transform>();

        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = GetComponentInParent<Genes>().perceptionRadius;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }


    public override string ToString()
    {
        string s = $"Danger: {perceivedDangers.Count}, " +
            $"Food: {perceivedFood.Count}, " +
            $"Mates: {perceivedMates.Count}";
        
        return s;
    }

    #region Types
    public enum PerceiveeType
    {
        Food,
        Danger,
        Mate,
        Irrelevant
    }

    public struct PerceivedMate
    {
        public Transform transform;
        //TODO: esto cuando el componente exista
        //public VitalFunctions vitalFunctions;
        public Genes genes;
    }
    #endregion
}
