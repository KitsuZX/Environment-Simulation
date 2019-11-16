using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Perceptor : MonoBehaviour
{
    private const string RABBIT_TAG = "Rabbit";
    private const string FOX_TAG = "Fox";
    private const string BUSH_TAG = "Bush";


    [SerializeField] private PerceiveeType rabbitPerception;
    [SerializeField] private PerceiveeType foxPerception;
    [SerializeField] private PerceiveeType bushPerception;


    private new SphereCollider collider;

    //Los objetos percibidos se guardan en un diccionario. La clave es el GameObject para poder identificarlo cuando se salga.
    //El valor es la información que necesitamos sobre este tipo de objeto percibido.
    private Dictionary<GameObject, Transform> perceivedDangers = new Dictionary<GameObject, Transform>();
    private Dictionary<GameObject, Transform> perceivedFood = new Dictionary<GameObject, Transform>();
    private Dictionary<GameObject, PerceivedMate> perceivedMates = new Dictionary<GameObject, PerceivedMate>();


    private PerceiveeType GetPerceiveeType(GameObject gameObject)
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


    private void OnTriggerEnter(Collider other)
    {
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


    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;

        collider.radius = GetComponentInParent<Genes>().perceptionRadius;
    }

    public override string ToString()
    {
        string s = $"Danger: {perceivedDangers.Count}, " +
            $"Food: {perceivedFood.Count}, " +
            $"Mates: {perceivedMates.Count}";
        
        return s;
    }


    public enum PerceiveeType
    {
        Food, 
        Danger,
        Mate,
        Irrelevant
    }

    private struct PerceivedMate
    {
        public Transform transform;
        public Genes genes;
    }
}
