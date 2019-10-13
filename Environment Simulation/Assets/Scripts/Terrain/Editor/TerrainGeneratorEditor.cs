using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
	TerrainGenerator terrainGen;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Refresh"))
		{
			terrainGen.Generate();
		}
	}

	private void OnEnable()
	{
		terrainGen = (TerrainGenerator)target;
	}
}

