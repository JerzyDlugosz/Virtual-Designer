using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutOfBoundsLogic : MonoBehaviour
{

    private GameObject BlackOutScreen;
    private Color blackOutScreenColor;

    private void Start()
    {
        BlackOutScreen = GameObject.Find("BlackOutScreen"); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            Color blackOutScreenColor = BlackOutScreen.GetComponentInChildren<Image>().color;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MainCamera")
        {

            float distance;
            if(this.transform.rotation.y == 180 || this.transform.rotation.y == 0)
            {
                distance = Mathf.Abs(this.transform.position.z - other.transform.position.z);
            }
            else
            {
                distance = Mathf.Abs(this.transform.position.x - other.transform.position.x);
            }
            if(distance > 1f)
            {
                distance = 1f;
            }
            blackOutScreenColor = new Color(blackOutScreenColor.r, blackOutScreenColor.g, blackOutScreenColor.b, (1 - distance));
            BlackOutScreen.GetComponentInChildren<Image>().color = blackOutScreenColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            BlackOutScreen.GetComponentInChildren<Image>().color = new Color(blackOutScreenColor.r, blackOutScreenColor.g, blackOutScreenColor.b, 0);
        }
    }

}
