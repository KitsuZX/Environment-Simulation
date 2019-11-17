using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
	const string meshHolderName = "Terrain Mesh";

	public bool autoUpdate = true;

	public bool centralize = true;
	public int worldSize = 20;
	public float waterDepth = .2f;
	public float edgeDepth = .2f;

	public NoiseSettings terrainNoise;
	public Material mat;

	public Biome water;
	public Biome sand;
	public Biome grass;

	[Header("Info")]
	public int numTiles;
	public int numLandTiles;
	public int numWaterTiles;
	public float waterPercent;

    public TerrainData TerrainData{get; private set;}

	MeshFilter meshFilter;
	MeshRenderer meshRenderer;
	Mesh mesh;

	bool needsUpdate;

    private void Awake()
    {
        TerrainData = Generate();
    }

    private void Update()
	{
		if(needsUpdate && autoUpdate)
		{
			needsUpdate = false;
			TerrainData = Generate();
		}
		else
		{
			if (!Application.isPlaying)
			{
				UpdateColors();
			}
		}
	}

	public TerrainData Generate()
	{
		CreateMeshComponents();

		//Generamos el mapa de alturas
		int numTilesPerLine = Mathf.CeilToInt(worldSize);
		float min = (centralize) ? -numTilesPerLine / 2f : 0f;
		float[,] map = HeightMapGenerator.GenerateHeightMap(terrainNoise, numTilesPerLine);

		var vertices = new List<Vector3>();
		var triangles = new List<int>();
		var uvs = new List<Vector2>();
		var normals = new List<Vector3>();

		//Cosas para no tener que escribir las cosas 10000 veces
		var biomes = new Biome[] { water, sand, grass };
		Vector3[] upVectorX4 = { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
		Coord[] nswe = { Coord.up, Coord.down, Coord.left, Coord.right };
		int[][] sideVertIndexByDir = { new int[] { 0, 1 }, new int[] { 3, 2 }, new int[] { 2, 0 }, new int[] { 1, 3 } };
		Vector3[] sideNormalsByDir = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

		//Terrain data
		var terrainData = new TerrainData(numTilesPerLine);
		numLandTiles = 0;
		numWaterTiles = 0;

		//Crear los quads
		for (int y = 0; y < numTilesPerLine; y++)
		{
			for (int x = 0; x < numTilesPerLine; x++)
			{
				//Subimos al shader como uvs el indice del bioma y la distancia al bioma más cercano
				Vector2 uv = GetBiomeInfo(map[x, y], biomes);
				uvs.AddRange(new Vector2[] { uv, uv, uv, uv });

				bool isWaterTile = uv.x == 0f;
				bool isLandTile = !isWaterTile;

				if (isWaterTile)
				{
					numWaterTiles++;
				}
				else
				{
					numLandTiles++;
				}

				//Vertices
				int vertexIndex = vertices.Count;
				float height = (isWaterTile) ? -waterDepth : 0;
				Vector3 nw = new Vector3(min + x, height, min + y + 1);
				Vector3 ne = nw + Vector3.right;
				Vector3 sw = nw - Vector3.forward;
				Vector3 se = sw + Vector3.right;
				Vector3[] tileVertices = { nw, ne, sw, se };

				vertices.AddRange(tileVertices);
				normals.AddRange(upVectorX4);

				//Triagulos
				triangles.Add(vertexIndex);
				triangles.Add(vertexIndex + 1);
				triangles.Add(vertexIndex + 2);
				triangles.Add(vertexIndex + 1);
				triangles.Add(vertexIndex + 3);
				triangles.Add(vertexIndex + 2);

				//Cierra el hueco entre los tiles de agua y los de tierra y crea los bordes en los ejes
				bool isEdgeTile = x == 0 || x == numTilesPerLine - 1 || y == 0 || y == numTilesPerLine - 1;
				if(isLandTile || isEdgeTile)
				{
					for (int i = 0; i < nswe.Length; i++)
					{
						int neigbhourX = x + nswe[i].x;
						int neihbhourY = y + nswe[i].y;

						bool neighbourIsOutOfBounds = neigbhourX < 0 || neigbhourX >= numTilesPerLine || neihbhourY < 0 || neihbhourY >= numTilesPerLine;
						bool neighbourIsWater = false;

						if (!neighbourIsOutOfBounds)
						{
							float neighbourHeight = map[neigbhourX, neihbhourY];
							neighbourIsWater = neighbourHeight <= biomes[0].height;
							if (neighbourIsWater)
							{
								terrainData.shore[neigbhourX, neihbhourY] = true;
								terrainData.coastal[x, y] = true;
							}
						}

						if(neighbourIsOutOfBounds || (isLandTile && neighbourIsWater))
						{
							float depth = waterDepth;
							if (neighbourIsOutOfBounds)
							{
								depth = (isWaterTile) ? edgeDepth : edgeDepth + waterDepth;
							}
							vertexIndex = vertices.Count;

							int edgeVertIntA = sideVertIndexByDir[i][0];
							int edgeVertIntB = sideVertIndexByDir[i][1];

							vertices.Add(tileVertices[edgeVertIntA]);					
							vertices.Add(tileVertices[edgeVertIntA] + Vector3.down * depth);					
							vertices.Add(tileVertices[edgeVertIntB]);					
							vertices.Add(tileVertices[edgeVertIntB] + Vector3.down * depth);

							uvs.AddRange(new Vector2[] { uv, uv, uv, uv });
							int[] sideTriIndices =
							{
								vertexIndex,
								vertexIndex + 1,
								vertexIndex + 2,
								vertexIndex + 1,
								vertexIndex + 3,
								vertexIndex +2
							};

							triangles.AddRange(sideTriIndices);
							normals.AddRange(new Vector3[] { sideNormalsByDir[i], sideNormalsByDir[i], sideNormalsByDir[i], sideNormalsByDir[i] });
						}
					}
				}

				terrainData.tileCentres[x, y] = nw + new Vector3(0.5f, 0, -0.5f);
				terrainData.walkable[x, y] = isLandTile;
			}
		}

		mesh.SetVertices(vertices);
		mesh.SetTriangles(triangles, 0, true);
		mesh.SetUVs(0, uvs);
		mesh.SetNormals(normals);

		meshRenderer.sharedMaterial = mat;
		UpdateColors();

		numTiles = numLandTiles + numWaterTiles;
		waterPercent = numWaterTiles / (float)numTiles;
		return terrainData;
	}

	private void UpdateColors()
	{		
		if(mat != null)
		{
			Color[] startColors = { water.startCol, sand.startCol, grass.startCol };
			Color[] endColors = { water.endCol, sand.endCol, grass.endCol };
			mat.SetColorArray("_StartColors", startColors);
			mat.SetColorArray("_EndColors", endColors);
		}
	}

	/// <summary>
	/// Devuelve el bioma en el que está un vertice y la distancia a la que está de otro bioma para calcular el color
	/// </summary>
	/// <param name="height">Altura a la que está el vertice que hay que analizar</param>
	/// <param name="biomes">Array de biomas</param>
	/// <returns>Vector 2 que contiene el bioma en el que se encuentra y el indice del blend del color</returns>
	private Vector2 GetBiomeInfo(float height, Biome[] biomes)	{
		
		int biomeIndex = 0;
		float biomeStartHeight = 0;

		for (int i = 0; i < biomes.Length; i++)
		{
			if (height <= biomes[i].height)
			{
				biomeIndex = i;
				break;
			}

			biomeStartHeight = biomes[i].height;
		}

		Biome biome = biomes[biomeIndex];
		float sampleT = Mathf.InverseLerp(biomeStartHeight, biome.height, height);
		sampleT = (int)(sampleT * biome.numSteps) / (float)Mathf.Max(biome.numSteps, 1);

		//UV stores x: biomeIndex and y: val between 0 and 1 for how colse to prev/next biome
		Vector2 uv = new Vector2(biomeIndex, sampleT);
		return uv;
	}
	
	/// <summary>
	/// Si no existe, crea un objeto en la escena que es el que contendrá el mesh, y le añade un meshFilter y un meshRenderer y crea el Mesh 
	/// </summary>
	private void CreateMeshComponents()
	{
		GameObject holder = null;

		if(meshFilter == null)
		{
			if (GameObject.Find(meshHolderName))
			{
				holder = GameObject.Find(meshHolderName);
			}
			else
			{
				holder = new GameObject(meshHolderName);
				holder.AddComponent<MeshRenderer>();
				holder.AddComponent<MeshFilter>();
			}

			meshFilter = holder.GetComponent<MeshFilter>();
			meshRenderer = holder.GetComponent<MeshRenderer>();
		}

		if(meshFilter.sharedMesh == null)
		{
			mesh = new Mesh();
			mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
			meshFilter.sharedMesh = mesh;
		}
		else
		{
			mesh = meshFilter.sharedMesh;
			mesh.Clear();
		}

		meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
	}

	private void OnValidate()
	{
		needsUpdate = true;	
	}

    [System.Serializable]
	public class Biome
	{
		[Range(0, 1)]
		public float height;
		public Color startCol;
		public Color endCol;
		public int numSteps;
	}	
}
