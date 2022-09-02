using HSVPicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    private Material material;
    private GameObject selectedObject;
    private List<int> indexesOfMaterial;
    public void Start()
    {
        material = this.GetComponentInParent<MaterialOfSlider>().material;
        selectedObject = this.GetComponentInParent<MaterialOfSlider>().selectedObject;
        indexesOfMaterial = this.GetComponentInParent<MaterialOfSlider>().indexesOfMaterial;
    }

    public void Changecolor(ColorPicker colorPicker)
    {
        if (selectedObject)
        {
            Color color = colorPicker.CurrentColor;

            foreach (int index in indexesOfMaterial)
            {
                if (selectedObject.transform.tag == "Furniture" || selectedObject.transform.tag == "Decoration")
                {
                    selectedObject.transform.GetChild(index).GetComponent<Renderer>().material.color = color;
                }
                else
                {
                    selectedObject.transform.GetComponent<Renderer>().material.color = color;
                }
            }
        }
    }
}
