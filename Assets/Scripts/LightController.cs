using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public List<GameObject> lightsources;
    bool active = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GameController")
        {
            Debug.Log("Enter");
            active = !active;
            foreach(GameObject lightsource in lightsources)
            {
                lightsource.SetActive(active);
            }
        }
    }
}
