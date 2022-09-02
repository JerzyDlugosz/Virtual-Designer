using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialList
{
    public MaterialList()
    {
        indexOfMaterial = new List<int>();
    }

    public string nameOfMaterial { get; set; }
    public List<int> indexOfMaterial { get; set; }
    public Transform transform { get; set; }
}
