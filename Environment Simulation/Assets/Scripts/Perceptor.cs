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
    public bool SeesFood
    {
        get
        {
            foreach (IEatable eatable in perceivedFood.Values)
            {
                if (eatable.IsAvailableToEat) return true;
            }
            return false;
        }
    }
    public bool SeesPotentialPartner
    {
        get
        {
            foreach (PerceivedMate mate in perceivedMates.Values)
            {
                if (mate.vitalFunctions.IsOldEnoughForSex && !mate.vitalFunctions.IsPregnant) return true;
            }
            return false;
        }
    }

    public float PerceptionRadius { get => collider.radius; set => collider.radius = value; }

    private new SphereCollider collider;
    private new Transform transform;
    private VitalFunctions vitalFunctions;

    //Los objetos percibidos se guardan en un diccionario. La clave es el GameObject para poder identificarlo cuando se salga.
    //El valor es la información que necesitamos sobre este tipo de objeto percibido.
    private Dictionary<GameObject, Transform> perceivedDangers = new Dictionary<GameObject, Transform>();
    private Dictionary<GameObject, IEatable> perceivedFood = new Dictionary<GameObject, IEatable>();
    private Dictionary<GameObject, PerceivedMate> perceivedMates = new Dictionary<GameObject, PerceivedMate>();

    #region Getters
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

    public IEatable GetClosestFood()
    {
        Vector3 myPos = transform.position;

        IEatable closest = null;
;
        float closestDistance = float.MaxValue;
        float distance;

        foreach (IEatable eatable in perceivedFood.Values)
        {
            if (!eatable.IsAvailableToEat) continue;

            distance = (myPos - eatable.Position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closest = eatable;
            }
        }

        return closest;
    }

    //Devuelve ICollection porque así no hace falta copiar a un array, se pasa directamente lo que contiene el diccionario.
    public ICollection<Transform> GetDangers()
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
            if (!mate.vitalFunctions.IsOldEnoughForSex || mate.vitalFunctions.IsPregnant) continue;

            sexAppeal = mate.genes.SexAppeal;
            if (sexAppeal > highestSexAppeal)
            {
                highestSexAppeal = sexAppeal;
                sexiestMate = mate;
            }
        }

        return sexiestMate;
    }
    #endregion

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
                perceivedFood.Add(perceivee, perceivee.GetComponent<IEatable>());
                break;
            case PerceiveeType.Danger:
                perceivedDangers.Add(perceivee, perceivee.transform);
                break;
            case PerceiveeType.Mate:
                VitalFunctions mateVitalFunctions = perceivee.GetComponent<VitalFunctions>();
                if(vitalFunctions.IsMale != mateVitalFunctions.IsMale)
                {
                    perceivedMates.Add(perceivee, new PerceivedMate
                    {
                        transform = perceivee.transform,
                        genes = perceivee.GetComponent<Genes>(),
                        vitalFunctions = mateVitalFunctions,
                        sexuality = perceivee.GetComponent<Sexuality>()
                    });
                }
               
                break;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        RemoveFromPerceived(other.gameObject, GetPerceiveeType(other.gameObject));
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


    private void Awake()
    {
        transform = GetComponent<Transform>();

        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;

        vitalFunctions = GetComponentInParent<VitalFunctions>();

        gameObject.layer = LayerMask.NameToLayer("PerceptorLayer");
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
        public VitalFunctions vitalFunctions;
        public Genes genes;
        public Sexuality sexuality;
    }
    #endregion
}
