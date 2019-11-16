using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Genes))]
public class AnimalMovement : MonoBehaviour
{
	[SerializeField] private float jumpHeight = 1f;
	[SerializeField] private float jumpTime = 1f;
	[SerializeField] private AnimationCurve jumpCurve;

	[Header("References")]
	[SerializeField] private Transform[] enemies;
	[SerializeField] private GameObject terrainGenerator;

	private bool _isJumping = false;
	private Transform _model;

	private NavMeshAgent _agent;
	private Genes _genes;
	private TerrainData _terrainData;


	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_genes = GetComponent<Genes>();
		_model = GetComponentInChildren<MeshRenderer>().transform;
		//_terrainData = terrainGenerator.GetComponent<TerrainGenerator>().TerrainData;
	}


	void FixedUpdate()
    {
		/*if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit))
			{
				_agent.SetDestination(hit.point);
			}

		}*/

		/*if(_agent.velocity.magnitude == 0)
		{
			MoveRandom();
		}*/

		//FleeFrom(enemy.position);

		if(_agent.velocity.magnitude > 0  && !_isJumping)
		{			
			StartCoroutine(jumpAnimation());
		}
    }

	private IEnumerator jumpAnimation()
	{
		_isJumping = true;
		float t = 0;

		while(t < 1)
		{
			Vector3 pos = _model.localPosition;
			pos.y = jumpCurve.Evaluate(t) * jumpHeight;

			_model.localPosition = pos;

			t += Time.fixedDeltaTime / jumpTime;
			yield return new WaitForFixedUpdate();
		}

		_isJumping = false;
	}
	
	public void FleeFrom(Vector3 danger)
	{
		/*Vector3 dir = (transform.position - danger).normalized;

		Vector3 runTo = transform.position + (transform.position - danger)  * _genes.speed;

		_agent.SetDestination(runTo);*/
	}

	public void GoTo(Vector3 position)
	{
		_agent.SetDestination(position);
	}

	public void MoveRandom()
	{
		Vector3 randomPosition = transform.position + Random.onUnitSphere * _agent.speed;
		randomPosition.y = transform.position.y;

		_agent.SetDestination(randomPosition);		
	}

	private void OnDrawGizmos()
	{
		/*Vector3 meanDirection = Vector3.zero;

		for (int i = 0; i < enemies.Length; i++)
		{
			Vector3 enemyPos2d = enemies[i].position;
			enemyPos2d.y = transform.position.y;
			Vector3 dir = (transform.position - enemyPos2d).normalized;

			meanDirection += dir / enemies.Length;
		}

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + meanDirection * 3.5f);

		float terrainSize = terrainGenerator.GetComponent<TerrainGenerator>().TerrainData.size * 0.5f;

		//Vector3 tile = Vector3.Scale(meanDirection, (new Vector3(terrainSize * 0.5f + transform.position.x + 0.5f, transform.position.y, terrainSize * 0.5f + transform.position.z + 0.5f)));
	

		float x = transform.position.x + meanDirection.x * 10f;
		float y = transform.position.y;
		float z = transform.position.z + meanDirection.z * 10f;

		Gizmos.DrawSphere(new Vector3(x, y, z), 0.25f);
		Vector2 intersection = Vector2.zero;
		Vector2 pos2d = new Vector2(transform.position.x, transform.position.z);

		Vector2 objective = new Vector2(x, z);

		if(x > )
		{
			if(z > terrainSize)
			{
				
			}
			else
			{

			}
		}
		else
		{
			if (z > terrainSize)
			{
				intersection = GetIntersectionPointCoordinates(pos2d, objective, new Vector2(-terrainSize, terrainSize), new Vector2(terrainSize, terrainSize)); 
			}
			else
			{

			}
		}

		Vector3 tile = new Vector3(x, y, z);
		//Debug.Log(tile);

		Gizmos.DrawSphere(tile, 0.25f);*/

	}

	public Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out bool found)
	{
		float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

		if (tmp == 0)
		{
			// No solution!
			found = false;
			return Vector2.zero;
		}

		float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

		found = true;

		return new Vector2(
			B1.x + (B2.x - B1.x) * mu,
			B1.y + (B2.y - B1.y) * mu
		);
	}
}
