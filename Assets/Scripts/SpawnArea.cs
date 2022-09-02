using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    private GameObject Player;
    void Start()
    {
        Player = GameObject.Find("Vr Rig");
        Player.transform.position = this.transform.position;
    }

}
