using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresenceLeft : MonoBehaviour
{
    private InputDevice targetdevice;
    public InputDeviceCharacteristics InputDeviceCharacteristics;
    private List<GameObject> collidedObjects;
    public GameObject leftHandMenu;
    private GameObject itemMenu;

    public bool hasRotatedAnObject = false;
    private bool buttonIsPressed = false;
    private bool exitButtonIsPressed = false;
    private bool isGrippingAnObject = false;
    private GameObject grippedObject;
    public GameObject interactionRay;
    public GameObject VrRig;
    private bool alreadyReset = false;
    private int exitTime = 0;

    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics, devices);
        collidedObjects = new List<GameObject>();
        //will get only right controller
        itemMenu = leftHandMenu.transform.GetChild(0).gameObject;

        if (devices.Count > 0)
        {
            targetdevice = devices[0];
        }
    }

    void FixedUpdate()
    {
        if (targetdevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryvalue) && secondaryvalue)
        {
            exitTime++;
            if (!exitButtonIsPressed)
            {
                exitButtonIsPressed = true;
            }
            if(exitTime > 300)
            {
                if (!alreadyReset)
                {
                    List<GameObject> presistentObjectList = new List<GameObject>();
                    presistentObjectList.Add(GameObject.Find("GameManager"));
                    presistentObjectList.Add(GameObject.Find("EventSystem"));
                    presistentObjectList.Add(GameObject.Find("Interaction Manager"));
                    presistentObjectList.Add(GameObject.Find("Vr Rig"));

                    foreach (GameObject item in presistentObjectList)
                    {
                        SceneManager.MoveGameObjectToScene(item, SceneManager.GetSceneByName("PersistentScene"));
                    }

                    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName("PersistentScene"));
                    SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
                    VrRig.transform.position = new Vector3(0, 0, 0);
                    alreadyReset = true;
                }
            }
        }
        else if (exitButtonIsPressed)
        {
            exitButtonIsPressed = false;
            alreadyReset = false;
            exitTime = 0;
        }
    }

    void Update()
    {
        if (targetdevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryvalue) && secondaryvalue)
        {
            if (!buttonIsPressed)
            {
                bool noMenu = false;

                foreach (Transform menu in leftHandMenu.transform)
                {
                    if (menu.gameObject.activeSelf)
                    {
                        menu.gameObject.SetActive(false);
                        noMenu = true;
                        GameObject.Find("Right Hand").GetComponent<HandPresenceRight>().noMenu = true;
                    }
                }
                foreach (Transform child in this.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetChild(0))
                {
                    Destroy(child.gameObject);
                }

                if (noMenu == false)
                {
                    itemMenu.SetActive(true);
                }

                buttonIsPressed = true;
            }            
        }
        else if(buttonIsPressed)
        {
            buttonIsPressed = false;
        }


        if (targetdevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue))
        {
            if (isGrippingAnObject)
            {
                if (targetdevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerValue) && triggerValue)
                {
                    if (primary2DAxisValue.x > 0.5f && !hasRotatedAnObject)
                    {
                        grippedObject.transform.GetComponent<Rigidbody>().MoveRotation(grippedObject.transform.rotation * Quaternion.Euler(0, 15, 0));
                        hasRotatedAnObject = true;
                    }
                    if (primary2DAxisValue.x < -0.5f && !hasRotatedAnObject)
                    {
                        grippedObject.transform.GetComponent<Rigidbody>().MoveRotation(grippedObject.transform.rotation * Quaternion.Euler(0, -15, 0));
                        hasRotatedAnObject = true;
                    }
                    if (primary2DAxisValue.x < 0.1f && primary2DAxisValue.x > -0.1f)
                    {
                        hasRotatedAnObject = false;
                    }
                }
                else
                {
                    if (primary2DAxisValue.y > 0.5f)
                    {
                        grippedObject.transform.position += new Vector3(0, 0, 0.01f);
                    }
                    if (primary2DAxisValue.y < -0.5f)
                    {
                        grippedObject.transform.position += new Vector3(0, 0, -0.01f);
                    }
                    if (primary2DAxisValue.x > 0.5f)
                    {
                        grippedObject.transform.position += new Vector3(0.01f, 0, 0);
                    }
                    if (primary2DAxisValue.x < -0.5f)
                    {
                        grippedObject.transform.position += new Vector3(-0.01f, 0, 0);
                    }
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        collidedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        collidedObjects.Remove(other.gameObject);
    }

    public void SetGrippedObject(GameObject ray)
    {
        if (ray.GetComponent<XRRayInteractor>().GetCurrentRaycastHit(out RaycastHit hit))
        {
            Debug.Log("Selected");
            isGrippingAnObject = true;
            grippedObject = hit.transform.gameObject;
            interactionRay.gameObject.GetComponent<XRInteractorLineVisual>().enabled = false;
        }
    }

    public void ReleaseGrippedObject()
    {
        Debug.Log("Released");
        isGrippingAnObject = false;
        interactionRay.gameObject.GetComponent<XRInteractorLineVisual>().enabled = true;
        if (grippedObject && grippedObject.tag == "Furniture")
        {
            grippedObject.transform.position = new Vector3(grippedObject.transform.position.x, 0f, grippedObject.transform.position.z);
        }
        grippedObject = null;
    }
}
