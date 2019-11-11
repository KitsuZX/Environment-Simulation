using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHopping : MonoBehaviour
{
    public float hopDistance = 2f;
    public AnimationCurve curve;

    public float verticalHopDistance = 1f;
    public float hopRestingTime = 0.5f;
    public float hopTime = 0.5f;

    private bool jumpAllowed = true;

	[Header("Info")]
	public Vector2 _objective;
	public bool _objectiveMarked = false;

    private void FixedUpdate()
    {
		if (jumpAllowed)
		{
			jumpAllowed = false;

			Vector2 targetPosition = Vector2.zero;
			Vector2 pos = new Vector2(transform.position.x, transform.position.z);

			if (!_objectiveMarked)
			{
				targetPosition = ChooseRandomPointToHop();
				transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.y));
			}
			else
			{ 
				transform.LookAt(new Vector3(_objective.x, transform.position.y, _objective.y));

				if((_objective - pos).magnitude > hopDistance)
				{
					float angle = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

					targetPosition = new Vector2(pos.x + hopDistance * Mathf.Sin(angle),												
												 pos.y + hopDistance * Mathf.Cos(angle));
					
				}
				else
				{
					targetPosition = _objective;
				}
			}

			StartCoroutine(HopToPosition(targetPosition));

		
			if((pos - _objective).magnitude < 0.5f)
			{
				_objectiveMarked = false;
			}
		}
    }

    private IEnumerator HopToPosition(Vector2 targetPosition)
    {              
        float t = 0;
	
        Vector2 beginPosition = new Vector2(transform.position.x, transform.position.z);

        while(t < 1)
        {
            Vector2 pos = Vector2.Lerp(beginPosition, targetPosition, t);      

            transform.position = new Vector3(pos.x, curve.Evaluate(t), pos.y);          

            t += Time.fixedDeltaTime / hopTime;
            yield return new WaitForFixedUpdate();
        }

        Invoke("allowJumping", hopRestingTime);         
    }

    private void allowJumping()
    {
        jumpAllowed = true;
    }

    private Vector2 ChooseRandomPointToHop()
    {
        float randomAngle = Random.Range(0f, 360f);

        float x = transform.position.x + hopDistance * Mathf.Sin(randomAngle);
        float z = transform.position.z + hopDistance * Mathf.Cos(randomAngle);

        Vector2 targetPoint = new Vector2(x, z);         

        return targetPoint;
    }
	
	public void MarkObjective(/*Vector3 destination*/)
	{
		_objective = new Vector2(7.5f, -7.5f);
		_objectiveMarked = true;
	}

}
