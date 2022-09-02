using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemMenuTabs : MonoBehaviour
{
    public GameObject tabs;
    public GameObject itemsHolder;

    public GameObject itemPrefab;
    public GameObject itemButton;

    public List<string> directories;
    private List<Sprite> sprites;
    public string tabName;

    public void FillTabs(string nameOfTab)
    {
        directories = new List<string>();
        sprites = new List<Sprite>();
        tabName = nameOfTab;
        if (itemsHolder.transform.childCount > 0)
        {
            foreach (Transform child in itemsHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        if (this.transform.GetChild(1).childCount > 0)
        {
            foreach (Transform child in this.transform.GetChild(1))
            {
                Destroy(child.gameObject);
            }
        }
        foreach (string directory in Directory.GetDirectories(Application.dataPath + "/Resources/Objects/" + nameOfTab))
        {
            string str = Application.dataPath + "/Resources/Objects/" + nameOfTab + "\\";
            string nameOfDirectory = directory.Replace(str, "");
            Debug.Log(nameOfDirectory);
            directories.Add(nameOfDirectory);
            Sprite sprite = Resources.Load<Sprite>("Objects/" + nameOfTab + "/" + nameOfDirectory);
            sprites.Add(sprite);

        }
        if(directories.Count >= 3)
        {
            ButtonBuilder("0", directories[0], sprites[0]);
            ButtonBuilder("1", directories[1], sprites[1]);
            ButtonBuilder("2", directories[2], sprites[2]);
        }
        else if (directories.Count == 2)
        {
            ButtonBuilder("0", directories[0], sprites[0]);
            ButtonBuilder("1", directories[1], sprites[1]);

        }
        else if (directories.Count == 1)
        {
            ButtonBuilder("0", directories[0], sprites[0]);
        }
        else
        {
            Debug.Log("No directiories in " + Application.dataPath + "/Resources/Objects/" + nameOfTab);
        }
    }

    public void FillOptions(string name)
    {
        if(itemsHolder.transform.childCount > 0)
        {
            foreach(Transform child in itemsHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (string directory in System.IO.Directory.GetDirectories(Application.dataPath + "/Resources/Objects/" + tabName +"/" + name))
        {
            foreach (string file in System.IO.Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file) == ".fbx")
                {
                    string fileName = file.Substring(file.LastIndexOf("\\") + 1);
                    GameObject Item = Instantiate(itemPrefab, itemsHolder.transform, false);
                    Item.name = fileName.Substring(0, fileName.Length - 4);
                    string str = Application.dataPath + "/Resources/";
                    Item.GetComponent<ItemPrefabStorage>().PrefabPath = file.Replace(str, "");

                    Sprite sprite = Resources.Load<Sprite>("Objects/" + tabName + "/" + name + "/" + Item.name + "/" + "img");
                    Item.GetComponent<Image>().sprite = sprite;
                }
                else if (Path.GetExtension(file) == ".prefab")
                {
                    string fileName = file.Substring(file.LastIndexOf("\\") + 1);
                    GameObject Item = Instantiate(itemPrefab, itemsHolder.transform, false);
                    Item.name = fileName.Substring(0, fileName.Length - 7);
                    string str = Application.dataPath + "/Resources/";
                    Item.GetComponent<ItemPrefabStorage>().PrefabPath = file.Replace(str, "");

                    Sprite sprite = Resources.Load<Sprite>("Objects/" + tabName + "/" + name + "/" + Item.name + "/" + "img");
                    Item.GetComponent<Image>().sprite = sprite;
                }
            }
        }
    }

    public void PreviousTab()
    {
        if (directories.Count > 3)
        {
            int name = int.Parse(tabs.transform.GetChild(0).name);
            int storageName = 0;
            storageName = name;
            name--;
            Destroy(tabs.transform.GetChild(0).gameObject);
            Destroy(tabs.transform.GetChild(1).gameObject);
            Destroy(tabs.transform.GetChild(2).gameObject);

            if (name < 0)
            {
                name = directories.Count - 1;
            }
            ButtonBuilder(name.ToString(), directories[name], sprites[name]);
            name = storageName;
            if (name >= directories.Count)
            {
                name -= directories.Count;
            }
            ButtonBuilder((name).ToString(), directories[name], sprites[name]);
            name++;
            if (name >= directories.Count)
            {
                name -= directories.Count;
            }
            ButtonBuilder((name).ToString(), directories[name], sprites[name]);
        }
    }

    public void NextTab()
    {
        if (directories.Count > 3)
        {
            int name = int.Parse(tabs.transform.GetChild(0).name);
            Destroy(tabs.transform.GetChild(0).gameObject);
            name += 3;
            if (name >= directories.Count)
            {
                name -= directories.Count;
            }
            ButtonBuilder(name.ToString(), directories[name], sprites[name]);
        }
    }

    public void ButtonBuilder(string name, string text, Sprite sprite)
    {
        GameObject button = Instantiate(itemButton, tabs.transform, false);
        button.name = name;
        button.GetComponent<Image>().sprite = sprite;
        button.GetComponent<Button>().onClick.AddListener(delegate { FillOptions(text); });
    }
}
