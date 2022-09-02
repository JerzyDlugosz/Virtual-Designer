using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLoadSceneFromGameManager : MonoBehaviour
{
    public void GetLoadScene()
    {
        if(GameObject.Find("3D Visualization"))
        {
            SceneList HouseName = GameObject.Find("3D Visualization").GetComponent<HouseChangingController>().HouseName;
            GameManagerScript.instance.LoadGame(HouseName);
        }
    }

}
