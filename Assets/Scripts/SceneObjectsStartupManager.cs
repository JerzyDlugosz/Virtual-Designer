using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsStartupManager : MonoBehaviour
{
    public string Name;

    void Start()
    {
        if (Name == "Vr Camera")
        {
            this.GetComponent<Canvas>().worldCamera = GameObject.Find(Name).GetComponent<Camera>();
        }
    }

}
