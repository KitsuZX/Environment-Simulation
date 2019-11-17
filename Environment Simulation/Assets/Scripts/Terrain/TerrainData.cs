using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData
{
    public int size;
    public Vector3[,] tileCentres;
    public bool[,] walkable;
    public bool[,] shore;

    public TerrainData(int size)
    {
        this.size = size;

        tileCentres = new Vector3[size, size];
        walkable = new bool[size, size];
        shore = new bool[size, size];
    }

	public Vector3 GetRandomWalkableTile()
	{
		int x = 0, y = 0;

		do
		{
			x = Random.Range(0, size);
			y = Random.Range(0, size);
		}
		while (!walkable[x, y]);

		return tileCentres[x, y];
	}

	public Vector3 GetRandomTile()
	{
		int x = Random.Range(0, size);
		int y = Random.Range(0, size);

		return tileCentres[x, y];
	}
}