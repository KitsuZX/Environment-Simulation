using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Graph : MonoBehaviour
{
    [SerializeField] private RectTransform graphContainer;
    private LineRenderer line;

    public static Graph Instance { get; private set; }

    private List<int> values = new List<int>();

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

        List<int> valueList = new List<int>() { 0, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33 };
        line = GetComponentInChildren<LineRenderer>();
        ShowGraph(valueList);
    }

    private void Update()
    {
        ShowGraph(values);
    }

    private void ShowGraph(List<int> valueList)
    {
        if (valueList.Count == 0) return;

        float graphHeight = 90;
        float xSize = graphContainer.sizeDelta.x * 0.5f;
        int yMax = 50;
        float graphAplitude = 0.01f;

        line.positionCount = valueList.Count;

        Vector2 lastPos = Vector2.zero;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = i * graphAplitude - xSize;
            float yPos = (valueList[i] /(float) yMax) * graphHeight;

            Vector2 actualPos = new Vector2(xPos, yPos);

            line.SetPosition(i, new Vector3(actualPos.x, actualPos.y, graphContainer.position.z - 1f));
        }


    }    

    public void AddValue(int value)
    {
        values.Add(value);
    }

}
