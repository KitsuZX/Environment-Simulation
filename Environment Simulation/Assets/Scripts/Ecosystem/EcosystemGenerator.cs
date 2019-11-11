using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

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

    [Header("References")]
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
}
