using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Graph : MonoBehaviour
{
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private LineRenderer rabbitsLine;
    [SerializeField] private LineRenderer foxesLine;

    public static Graph Instance { get; private set; }

    private List<int> rabbits = new List<int>();
    private List<int> foxes = new List<int>();

	private float maxContainerX;
	private float minContainerX;
	private float maxContainerY;
	private float minContainerY;

	private float timeSinceStartup;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

		Vector3[] v = new Vector3[4];
		graphContainer.GetWorldCorners(v);
		minContainerX = v[0].x;
		minContainerY = v[0].y;
		maxContainerY = v[2].y;
		maxContainerX = v[2].x;


		//List<int> valueList = new List<int>() { 0, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33 };      

	}

    private void Update()
    {
		timeSinceStartup = Time.realtimeSinceStartup;

		ShowGraph(rabbits, rabbitsLine);
        ShowGraph(foxes, foxesLine);
    }

    private void ShowGraph(List<int> valueList, LineRenderer line)
    {
        if (valueList.Count == 0) return;

        float graphHeight = graphContainer.anchorMax.y;

        line.positionCount = valueList.Count;

        Vector2 lastPos = Vector2.zero;

        for (int i = 0; i < valueList.Count; i++)
        {		
			float valueXNorm = (float) i / (float) 120f;
			float xPos = Mathf.Lerp(minContainerX, maxContainerX, valueXNorm);

			float valueYNorm = valueList[i] / (float)40f;
			float yPos = Mathf.Lerp(minContainerY, maxContainerY, valueYNorm);

			Vector2 actualPos = new Vector2(xPos, yPos);

			line.SetPosition(i, new Vector3(actualPos.x, actualPos.y, graphContainer.position.z - 1f));
        }
    }    

    public void AddRabbit(int value)
    {
        rabbits.Add(value);
    }

	public void AddFox(int value)
	{
		foxes.Add(value);
	}

}
