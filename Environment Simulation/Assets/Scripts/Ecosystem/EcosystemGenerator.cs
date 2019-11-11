using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

#pragma warning disable 649
[RequireComponent(typeof(TerrainGenerator))]
public class EcosystemGenerator : MonoBehaviour
{    
    [Header("Trees")]
    [Range(0, 1)]
    [SerializeField] private float treeThreshold = 0.05f;
    [SerializeField] private float scaleVariation = 0.2f;    
    
    [Header("Bushes")]
    [Range(0, 1)]
    [SerializeField] private float bushThreshold = 0.05f;

    [Header("Animals")]
    [SerializeField] private int rabbitCount = 10;
    [SerializeField] private int foxCount = 10;

    [Header("References")]
    [SerializeField] private GameObject rabbitPrefab;
    [SerializeField] private GameObject foxPrefab;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private Transform treesParent;
    [SerializeField] private GameObject bushPrefab;
    [SerializeField] private Transform bushesParent;

    private TerrainData terrainData;

    private void Start()
    {
        TerrainGenerator generator = GetComponent<TerrainGenerator>();
        Assert.IsNotNull(generator);
        terrainData = generator.TerrainData;

        GenerateTrees();
        GenerateBushes();
        GenerateAnimals();
    }

    private void GenerateTrees()
    {
        for (int y = 0; y < terrainData.size; y++)
        {
            for (int x = 0; x < terrainData.size; x++)
            {
                if(terrainData.walkable[x, y] && !terrainData.shore[x, y])
                {
                    float treeRrand = Random.Range(0f, 1f);
                    if(treeRrand <= treeThreshold)
                    {
                        GameObject tree = Instantiate(treePrefab, terrainData.tileCentres[x, y], Quaternion.identity);

                        float randomScaleVariation = Random.Range(-scaleVariation, scaleVariation);                     
                        tree.transform.localScale += new Vector3(randomScaleVariation, randomScaleVariation, randomScaleVariation);
                        tree.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
                        tree.transform.SetParent(treesParent, true);

                        terrainData.walkable[x, y] = false;
                    }
                }
            }
        }
    }

    private void GenerateBushes()
    {
        for (int y = 0; y < terrainData.size; y++)
        {
            for (int x = 0; x < terrainData.size; x++)
            {
                if (terrainData.walkable[x, y] && !terrainData.shore[x, y])
                {
                    float bushRand = Random.Range(0f, 1f);
                    if (bushRand <= bushThreshold)
                    {
                        GameObject bush = Instantiate(bushPrefab, terrainData.tileCentres[x, y], Quaternion.identity);
                        bush.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
                        bush.transform.SetParent(bushesParent, true);

                        terrainData.walkable[x, y] = false;
                    }
                }
            }
        }
    }

    private void GenerateAnimals()
    {
        int x;
        int y;

        Vector3 offset = new Vector3(0, 0.5f, 0);
        for (int i = 0; i < rabbitCount; i++)
        {
            do
            {
                x = Random.Range(0, terrainData.size);
                y = Random.Range(0, terrainData.size);
            } while (!terrainData.walkable[x, y]);


            Instantiate(rabbitPrefab, terrainData.tileCentres[x, y] + offset, Quaternion.identity);
        }

        for (int i = 0; i < foxCount; i++)
        {
            do
            {
                x = Random.Range(0, terrainData.size);
                y = Random.Range(0, terrainData.size);
            } while (!terrainData.walkable[x, y]);


            Instantiate(foxPrefab, terrainData.tileCentres[x, y] + offset, Quaternion.identity);
        }
    }
}
