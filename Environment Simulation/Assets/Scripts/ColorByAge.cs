using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorByAge : MonoBehaviour
{

    VitalFunctions vf;
    Genes genes;

    Color maleColor;
    Color maleOldColor;
    Color femaleColor;
    Color femaleOldColor;

    Material mat;
    string tag;

    public void InitializeColors()
    {
        if (tag.Equals("Rabbit"))
        {
            maleColor = new Color(92f / 255f, 55f / 255f, 16f / 255f);
            maleOldColor = new Color(90f / 255, 90f / 255, 90f / 255);
            femaleColor = new Color(173f / 255f, 103f / 255f, 29f / 255f);
            femaleOldColor = new Color(130f / 255, 130f / 255, 130f / 255);
        }
        else
        {
            maleColor = new Color(191f / 60f, 103f / 255f, 0f / 255f);
            maleOldColor = new Color(104f / 255f, 80f / 255f, 41f / 255);
            femaleColor = new Color(255f / 80f, 55f / 255f, 0f / 255f);
            femaleOldColor = new Color(164f / 255f, 107f / 255f, 50f / 255);
        }
    }

    void Start()
    {
        tag = gameObject.tag;
        vf = GetComponent<VitalFunctions>();
        genes = GetComponent<Genes>();
        InitializeColors();

        if (tag.Equals("Rabbit"))
        {
            mat = GetComponentInChildren<MeshRenderer>().materials[1];
        }
        else
        {
            mat = GetComponentInChildren<MeshRenderer>().materials[0];
        }



    }
    void Update()
    {
        if (vf.IsMale)
        {
            mat.color = Color.Lerp(maleColor, maleOldColor, vf.CurrentAge / genes.lifeExpectancy);
        }
        else
        {
            mat.color = Color.Lerp(femaleColor, femaleOldColor, vf.CurrentAge / genes.lifeExpectancy);
        }
       
    }
}
