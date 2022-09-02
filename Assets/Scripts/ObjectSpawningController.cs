using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectSpawningController : MonoBehaviour
{
    private string path;
    private GameObject player;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private GameObject hand;
    public ItemMenuTabs Tabs;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        hand = GameObject.FindGameObjectWithTag("GameController");
        path = this.gameObject.GetComponent<ItemPrefabStorage>().PrefabPath;
        Tabs = GameObject.Find("TabsPanel").GetComponent<ItemMenuTabs>();
    }

    public void SpawnObject()
    {
        string pathToAsset = "none";

        if(Path.GetExtension(path) == ".fbx")
        {
            pathToAsset = path.Substring(0, path.Length - 4);
        }
        else if (Path.GetExtension(path) == ".prefab")
        {
            pathToAsset = path.Substring(0, path.Length - 7);
        }
        pathToAsset = pathToAsset.Replace('\\', '/');
        if (Resources.Load(pathToAsset))
        {
            Vector3 eulerAngles = new Vector3(0f, player.transform.eulerAngles.y - player.transform.eulerAngles.y % 15, 0f);
            spawnRotation = Quaternion.Euler(eulerAngles);
            spawnPosition = new Vector3(hand.transform.position.x, 0f, hand.transform.position.z);
            GameObject gameObj = (GameObject)Instantiate(Resources.Load(pathToAsset), spawnPosition, spawnRotation);

            if (gameObj.transform.childCount > 0)
            {
                foreach (Transform child in gameObj.transform)
                {
                    if (Tabs.tabName == "Furniture")
                    {
                        child.gameObject.tag = "Furniture";
                        child.gameObject.layer = 9;
                    }
                    if (Tabs.tabName == "Decorations")
                    {
                        child.gameObject.tag = "Decoration";
                        child.gameObject.layer = 13;
                    }
                    child.gameObject.AddComponent<MeshCollider>().convex = true;
                }
            }

            if (Tabs.tabName == "Furniture")
            {
                gameObj.gameObject.tag = "Furniture";
                gameObj.gameObject.layer = 9;
            }
            if (Tabs.tabName == "Decorations")
            {
                gameObj.gameObject.tag = "Decoration";
                gameObj.gameObject.layer = 13;
                gameObj.AddComponent<MeshCollider>().convex = true;
            }

            gameObj.transform.Translate(Vector3.forward);
            Rigidbody rb = gameObj.AddComponent<Rigidbody>();

            if (Tabs.tabName == "Furniture")
            {
                rb.isKinematic = true;
            }
            if (Tabs.tabName == "Decorations")
            {
                rb.isKinematic = false;
            }
            rb.freezeRotation = true;
            gameObj.AddComponent<XRGrabInteractable>();
            gameObj.GetComponent<XRGrabInteractable>().trackRotation = false;
            gameObj.GetComponent<XRGrabInteractable>().trackPosition = false;
            gameObj.GetComponent<XRGrabInteractable>().movementType = XRBaseInteractable.MovementType.VelocityTracking;

        }
        else
        {
            Debug.LogError($"Couldn't load the resource, asset path: {pathToAsset}");
        }
    }
}
