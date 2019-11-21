using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ecosystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI foxesText;
    [SerializeField] private TextMeshProUGUI rabbitsText;

    [HideInInspector] public List<GameObject> foxes = new List<GameObject>();
    [HideInInspector] public List<GameObject> rabbits = new List<GameObject>();

    public int FoxesCount => foxes.Count;
    public int RabbitsCount => rabbits.Count;

    public void RemoveAnimal(GameObject animal)
    {
        if (animal.CompareTag("Fox"))
        {
            foxes.Remove(animal);
        }
        else
        {
            rabbits.Remove(animal);
        }

        UpdateTexts();
    }

    public void AddAnimal(GameObject animal)
    {
        if (animal.CompareTag("Fox"))
        {
            foxes.Add(animal);
        }
        else
        {
            rabbits.Add(animal);
        }

        UpdateTexts();
    }

    private void UpdateTexts()
    {
        foxesText.text = "Foxes: \n" + FoxesCount.ToString();
        rabbitsText.text = "Rabbit: \n" + RabbitsCount.ToString();
    }

    private void Update()
    {
        Graph.Instance.AddValue(RabbitsCount);
    }

}
