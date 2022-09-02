using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseChangingController : MonoBehaviour
{
    public List<GameObject> Houses;
    int HouseNumber = 0;
    public SceneList HouseName;

    public void NextHouse()
    {
        HouseNumber++;
        HouseName++;
        if (HouseNumber > Houses.Count - 1)
        {
            HouseNumber = 0;
            HouseName = (SceneList)3;
        }
        RefreshHouse();
    }

    public void PrevHouse()
    {
        HouseNumber--;
        HouseName--;
        if(HouseNumber < 0)
        {
            HouseNumber = Houses.Count - 1;
            HouseName = (SceneList)8;
        }
        RefreshHouse();
    }

    void RefreshHouse()
    {
        for(int i=0; i < Houses.Count; i++)
        {
            Houses[i].SetActive(false);
            if(i == HouseNumber)
            {
                Houses[i].SetActive(true);
            }

        }
    }
}
