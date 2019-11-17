using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData
{
    public int size;
    public Vector3[,] tileCentres;
    public bool[,] walkable;
    public bool[,] shore;
	public bool[,] coastal;

    public TerrainData(int size)
    {
        this.size = size;

        tileCentres = new Vector3[size, size];
        walkable = new bool[size, size];
        shore = new bool[size, size];
		coastal = new bool[size, size];
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

	public Vector3 GetNearestCoastalTile(Vector3 position)
	{
		Vector3 nearestSourceWater = Vector3.zero;
		Vector2 originTile = new Vector2(Mathf.Round(position.x) + 0.5f, Mathf.Round(position.z) + 0.5f);

		Vector2 pos = new Vector2(position.x, position.z);		

		float maxDistance = float.MaxValue;

		for (int y = 0; y < size; y++)
		{
			for (int x = 0; x < size; x++)
			{
				if (coastal[x, y])
				{
					Vector2 tile = new Vector2(tileCentres[x, y].x, tileCentres[x, y].z);

					float distance = Vector2.Distance(pos, tile);
					if (distance < maxDistance)
					{
						maxDistance = distance;
						nearestSourceWater = tileCentres[x, y];
					}
				}
			}
		}

		nearestSourceWater.y = position.y;

		return nearestSourceWater;
	}

	public Vector2 WorldBorderToTile(Vector2 position)
	{
		float x = Mathf.Floor(position.x) + ((position.x > 0) ? 0.5f - 1 : -0.5f + 1);
		float y = Mathf.Floor(position.y) + ((position.y > 0) ? 0.5f - 1: -0.5f + 1);

		int inX = Mathf.Abs(Mathf.FloorToInt(x + size * 0.5f));
		int inY = Mathf.Abs(Mathf.FloorToInt(y + size * 0.5f));

		return new Vector2(inX, inY);
	}	
}