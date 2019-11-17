using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Genes))]
public class AnimalMovement : MonoBehaviour
{
    [SerializeField] private float thresholdDistance = 1f;
	[SerializeField] private float jumpHeight = 1f;
	[SerializeField] private float jumpTime = 1f;
	[SerializeField] private float timeBetweenRandomlyChoosingTarget = 8f;
	[SerializeField] private AnimationCurve jumpCurve = null;

	private bool _isJumping = false;
	private Transform _model;

	private NavMeshAgent _agent;
	private Genes _genes;
	private TerrainGenerator _terrainGenerator;

	private Vector3 randomTarget;

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_genes = GetComponent<Genes>();
		_model = GetComponentInChildren<MeshRenderer>().transform;
        _terrainGenerator = FindObjectOfType<TerrainGenerator>();
	}

	private void Start()
	{
		InvokeRepeating("UpdateRandomTarget", 0f, timeBetweenRandomlyChoosingTarget);
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

		//FleeFrom(enemies);

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
	
	public void FleeFrom(ICollection<Transform> dangers)
	{
		Vector3 meanDirection = Vector3.zero;

		foreach (Transform dangerSource in dangers)
		{
			Vector3 enemyPos2d = dangerSource.position;
			enemyPos2d.y = transform.position.y;
			Vector3 dir = transform.position - enemyPos2d;

			meanDirection += dir / dangers.Count;
		}

		meanDirection = meanDirection.normalized;

		_agent.SetDestination(GetFurthesPointInDirection(meanDirection));

	}

	public bool GoTo(Vector3 position)
	{
		_agent.SetDestination(position);
        return ((position - transform.position).sqrMagnitude < thresholdDistance * thresholdDistance);
    }

	public void MoveRandom()
	{
		GoTo(randomTarget);		
	}


	private Vector3 GetFurthesPointInDirection(Vector3 direction)
	{
		float terrainSize = _terrainGenerator.TerrainData.size * 0.5f;

		float x = transform.position.x + direction.x * terrainSize * 2;
		float y = transform.position.y;
		float z = transform.position.z + direction.z * terrainSize * 2;

		Vector2 intersection = Vector2.zero;
		Vector2 pos2d = new Vector2(transform.position.x, transform.position.z);

		Vector2 objective = new Vector2(x, z);

		Vector2 upRightCorner = new Vector2(terrainSize, terrainSize);
		Vector2 downRightCorner = new Vector2(terrainSize, -terrainSize);
		Vector2 upLeftCorner = new Vector2(-terrainSize, terrainSize);
		Vector2 downLeftCorner = new Vector2(-terrainSize, -terrainSize);

		bool b;

		if (x > terrainSize)
		{
			if (z > terrainSize)
			{
				if (Mathf.Abs(x) > Mathf.Abs(z))
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upRightCorner, downRightCorner, out b);
				}
				else
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upLeftCorner, upRightCorner, out b);
				}

			}
			else if (z < -terrainSize)
			{
				if (Mathf.Abs(x) > Mathf.Abs(z))
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upRightCorner, downRightCorner, out b);
				}
				else
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, downLeftCorner, downRightCorner, out b);
				}
			}
			else
			{
				intersection = GetIntersectionPointCoordinates(pos2d, objective, upRightCorner, downRightCorner, out b);
			}

		}
		else if (x < -terrainSize)
		{
			if (z > terrainSize)
			{
				if (Mathf.Abs(x) > Mathf.Abs(z))
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upLeftCorner, downLeftCorner, out b);
				}
				else
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upLeftCorner, upRightCorner, out b);
				}

			}
			else if (z < -terrainSize)
			{
				if (Mathf.Abs(x) > Mathf.Abs(z))
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, downLeftCorner, upLeftCorner, out b);
				}
				else
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, downLeftCorner, downRightCorner, out b);
				}
			}
			else
			{
				intersection = GetIntersectionPointCoordinates(pos2d, objective, upLeftCorner, downLeftCorner, out b);
			}
		}
		else if (z > terrainSize)
		{
			if (x > terrainSize)
			{
				if (Mathf.Abs(x) > Mathf.Abs(z))
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upRightCorner, downRightCorner, out b);
				}
				else
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upLeftCorner, upRightCorner, out b);
				}

			}
			else if (x < -terrainSize)
			{
				if (Mathf.Abs(x) > Mathf.Abs(z))
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upLeftCorner, downLeftCorner, out b);
				}
				else
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upRightCorner, upLeftCorner, out b);
				}
			}
			else
			{
				intersection = GetIntersectionPointCoordinates(pos2d, objective, upLeftCorner, upRightCorner, out b);
			}
		}
		else if (z < -terrainSize)
		{
			if (x > terrainSize)
			{
				if (Mathf.Abs(x) > Mathf.Abs(z))
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upRightCorner, downRightCorner, out b);
				}
				else
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, downRightCorner, downLeftCorner, out b);
				}

			}
			else if (x < -terrainSize)
			{
				if (Mathf.Abs(x) > Mathf.Abs(z))
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, upLeftCorner, downLeftCorner, out b);
				}
				else
				{
					intersection = GetIntersectionPointCoordinates(pos2d, objective, downRightCorner, downLeftCorner, out b);
				}
			}
			else
			{
				intersection = GetIntersectionPointCoordinates(pos2d, objective, downRightCorner, downLeftCorner, out b);
			}
		}

		return new Vector3(intersection.x, y, intersection.y);
	}

	private void OnDrawGizmos()
	{
		/*Vector3 meanDirection = Vector3.zero;

		for (int i = 0; i < enemies.Length; i++)
		{
			Vector3 enemyPos2d = enemies[i].position;
			enemyPos2d.y = transform.position.y;
			Vector3 dir = transform.position - enemyPos2d;

			meanDirection += dir / enemies.Length;
		}

		meanDirection = meanDirection.normalized;

		Vector3 tile = GetFurthesPointInDirection(meanDirection);

		Gizmos.color = Color.blue;
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

	private void UpdateRandomTarget()
	{
		randomTarget = _terrainGenerator.TerrainData.GetRandomWalkableTile();
//		print(randomTarget);
	}
}