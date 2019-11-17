using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBaker : MonoBehaviour
{

	private NavMeshSurface surface;

	private void Awake()
	{
		surface = GetComponent<NavMeshSurface>();
	}

	private void Start()
	{
		surface.BuildNavMesh();
	}
}
