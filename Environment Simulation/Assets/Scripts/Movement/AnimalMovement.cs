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

	private bool _isJumping = false;
	private Transform _model;

	private NavMeshAgent _agent;
	private Genes _genes;


	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_genes = GetComponent<Genes>();
		_model = GetComponentInChildren<MeshRenderer>().transform;		
	}

	private void Start()
	{
		_agent.speed = _genes.hopDistance;
	}

	void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit))
			{
				_agent.SetDestination(hit.point);
			}

		}

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
	
	public void FleeFrom(Transform danger)
	{

	}

	public void GoTo(Vector3 position)
	{
		_agent.SetDestination(position);
	}

	public void MoveRandom()
	{

	}
}
