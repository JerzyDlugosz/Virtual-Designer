using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillInfoBox : MonoBehaviour
{
    void Awake()
    {
        transform.Find("Name").GetComponent<Text>().text = transform.parent.name;
    }

}
