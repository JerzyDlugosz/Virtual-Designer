using HSVPicker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MaterialsManager : MonoBehaviour
{
    public GameObject materialChanger;
    private HandPresenceRight rightHand;
    private List<MaterialList> materialLists;
    public GameObject itemButton;
    private string sizeOfHouse;

    public void PopulateMaterialMenu(HandPresenceRight rightController)
    {
        if (rightController)
        {
            rightHand = rightController;

            FillList(rightHand);

            if (rightHand.selectedObject.transform.tag == "Furniture" || rightHand.selectedObject.transform.tag == "Decoration")
            {
                foreach (MaterialList materialList in materialLists)
                {
                    GameObject item = Instantiate(materialChanger, this.transform, false);
                    item.GetComponent<MaterialOfSlider>().selectedObject = rightController.selectedObject;
                    item.GetComponent<MaterialOfSlider>().material = rightController.selectedObject.transform.GetChild(materialList.indexOfMaterial.First()).GetComponent<Renderer>().material;
                    item.transform.GetChild(0).GetComponent<Text>().text = materialList.nameOfMaterial;
                    item.GetComponent<MaterialOfSlider>().indexesOfMaterial = materialList.indexOfMaterial;

                    ColorPicker colorPicker = item.transform.GetChild(1).GetChild(0).GetComponent<ColorPicker>();
                    colorPicker.CurrentColor = rightController.selectedObject.transform.GetChild(materialList.indexOfMaterial.First()).GetComponent<Renderer>().material.color;

                }
            }
            else
            {
                foreach (MaterialList materialList in materialLists)
                {
                    GameObject item = Instantiate(materialChanger, this.transform, false);
                    item.GetComponent<MaterialOfSlider>().selectedObject = rightController.selectedObject;
                    item.GetComponent<MaterialOfSlider>().material = rightController.selectedObject.GetComponent<Renderer>().material;
                    item.transform.GetChild(0).GetComponent<Text>().text = materialList.nameOfMaterial.Replace(" (Instance)", "");
                    item.GetComponent<MaterialOfSlider>().indexesOfMaterial = materialList.indexOfMaterial;
                    Debug.Log(materialList.nameOfMaterial);
                    string str = materialList.nameOfMaterial.Replace(" (Instance)", "");
                    if(str.LastIndexOf("-") != -1)
                    {
                        str = str.Substring(0, str.LastIndexOf("-") - 1);
                    }
                    int i = 0;
                    if(SceneManager.GetActiveScene().name == "House-Large" || SceneManager.GetActiveScene().name == "Preset-House-Large")
                    {
                        sizeOfHouse = "Large";
                    }
                    if (SceneManager.GetActiveScene().name == "House-Medium" || SceneManager.GetActiveScene().name == "Preset-House-Medium")
                    {
                        sizeOfHouse = "Medium";
                    }
                    if (SceneManager.GetActiveScene().name == "House-Small" || SceneManager.GetActiveScene().name == "Preset-House-Small")
                    {
                        sizeOfHouse = "Small";
                    }

                    foreach (string asset in Directory.GetFiles(Application.dataPath + "/Resources/Rooms/" + sizeOfHouse + "/WallMaterials"))
                    {
                        string fileName = asset.Substring(asset.LastIndexOf("\\") + 1);
                        if (Path.GetExtension(fileName) == ".mat")
                        {

                            string aplicationPath = Application.dataPath + "/Resources/Rooms/" + sizeOfHouse + "/WallMaterials";
                            fileName = fileName.Substring(0, fileName.Length - 4);
                            if (fileName.Contains(str))
                            {

                                string ResourcesPath = asset.Replace(Application.dataPath + "/Resources/", "");
   
                                if (sizeOfHouse == "Large")
                                {
                                    CreateButtonForSpecificHhouse("Large", ResourcesPath, item, i);
                                    i++;
                                }
                                if (sizeOfHouse == "Medium")
                                {
                                    CreateButtonForSpecificHhouse("Medium", ResourcesPath, item, i);
                                    i++;
                                }
                                if (sizeOfHouse == "Small")
                                {
                                    CreateButtonForSpecificHhouse("Small", ResourcesPath, item, i);
                                    i++;
                                }
                            }
                        }
                    }
                    ColorPicker colorPicker = item.transform.GetChild(1).GetChild(0).GetComponent<ColorPicker>();
                    colorPicker.CurrentColor = rightController.selectedObject.GetComponent<Renderer>().material.color;
                }
            }
        }
        else
        {
            Debug.LogError("Right controller cannot be found!");
        }
    }

    public void CreateButtonForSpecificHhouse(string houseName, string path, GameObject item, int i)
    {
        if (path.Contains(houseName))
        {
            if (Path.GetExtension(path) == ".mat")
            {
                Debug.Log(path);
                string materialPath = path.Replace(".mat", "");
                materialPath = materialPath.Replace("Assets/Resources/", "");
                Material material = Resources.Load<Material>(materialPath);
                ButtonBuilder("Texture_" + i, item, material);
                i++;
            }
        }
    }



    public void onCloseMenu()
    {
        rightHand = null;
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void FillList(HandPresenceRight rightHand)
    {
        materialLists = new List<MaterialList>();
        int i = 0;
        List<string> compactNames = new List<string>();

        if(rightHand.selectedObject.transform.tag == "Furniture" || rightHand.selectedObject.transform.tag == "Decoration")
        {
            foreach (Transform child in rightHand.selectedObject.transform)
            {
                if (child.GetComponent<Renderer>())
                {
                    if (compactNames.Contains(child.GetComponent<Renderer>().material.name))
                    {
                        foreach (MaterialList list in materialLists)
                        {
                            if (list.nameOfMaterial == child.GetComponent<Renderer>().material.name)
                            {
                                list.indexOfMaterial.Add(i);
                            }
                        }
                    }
                    else
                    {
                        MaterialList materialList = new MaterialList();
                        materialList.nameOfMaterial = child.GetComponent<Renderer>().material.name;
                        materialList.indexOfMaterial.Add(i);
                        materialLists.Add(materialList);
                        compactNames.Add(child.GetComponent<Renderer>().material.name);
                    }
                    i++;
                }
                else
                {
                    Destroy(child.gameObject);
                    Debug.LogWarning($"child {child.name} has no renderer, Deleting that object");
                }
            }
        }
        else
        {
            Transform collider = rightHand.selectedObject.transform;
            if (collider.GetComponent<Renderer>())
            {
                if (compactNames.Contains(collider.GetComponent<Renderer>().material.name))
                {
                    foreach (MaterialList list in materialLists)
                    {
                        if (list.nameOfMaterial == collider.GetComponent<Renderer>().material.name)
                        {
                            list.indexOfMaterial.Add(i);
                        }
                    }
                }
                else
                {
                    MaterialList materialList = new MaterialList();
                    materialList.nameOfMaterial = collider.GetComponent<Renderer>().material.name;
                    materialList.indexOfMaterial.Add(i);
                    materialLists.Add(materialList);
                    compactNames.Add(collider.GetComponent<Renderer>().material.name);
                }
            }
        }
    }
    public void ChangeMaterial(Material material)
    {
        if (rightHand.selectedObject.GetComponent<Renderer>())
        {
            rightHand.selectedObject.GetComponent<Renderer>().material = material;
        }
    }

    public void ButtonBuilder(string name, GameObject item, Material material)
    {
        GameObject button = Instantiate(itemButton, item.transform.GetChild(2).GetChild(1), false);
        button.name = name;
        button.GetComponent<Button>().onClick.AddListener(delegate { ChangeMaterial(material); });
    }

}
