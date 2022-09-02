using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresenceRight : MonoBehaviour
{
    private InputDevice targetdevice;
    public InputDeviceCharacteristics InputDeviceCharacteristics;

    public GameObject description;
    public GameObject menu;

    public XRController teleportationRay;
    public XRController interactionRay;

    public GameObject Box;

    private bool isGrippingAnObject = false;
    private GameObject grippedObject;
    private string grippedObjectTag;

    public GameObject selectedObject = null;
    private bool isPrimaryButtonPressed = false;

    public GameObject VrRig;
    public GameObject parentless;

    public bool hasRotatedAnObject = false;

    public EventSystem eventSystem;

    //private GameObject predictedTransform;

    public float distance;
    public float previousDistance = 0f;

    private Quaternion rotationOfVrRigAndCamera;

    private XRRayInteractor rayInteractor;
    private XRInteractorLineVisual lineVisual;

    public bool noMenu;
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics, devices);

        rayInteractor = interactionRay.GetComponent<XRRayInteractor>();
        lineVisual = interactionRay.GetComponent<XRInteractorLineVisual>();
        //will get only right controller
        if (devices.Count > 0)
        {
            targetdevice = devices[0];
        }
    }

    private void FixedUpdate()
    {
        if ((isGrippingAnObject && grippedObject) && (grippedObjectTag == "Furniture" || grippedObjectTag == "Decoration"))
        {
            if (targetdevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
            {
                targetdevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 angularVelocity);
                targetdevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);
                RotateAround(grippedObject.transform, this.transform.position, rotation, velocity, angularVelocity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        lineVisual.enabled = true;
        if (targetdevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryvalue) && primaryvalue)
        {
            if(!isPrimaryButtonPressed)
            {
                if (rayInteractor.GetCurrentRaycastHit(out RaycastHit hit))
                {
                    noMenu = true;
                    foreach(Transform menuItem in menu.transform)
                    {
                        if(menuItem.gameObject.activeSelf)
                        {
                            noMenu = false;
                        }
                    }

                    if (noMenu == true)
                    {
                        switch (hit.transform.tag)
                        {
                            case "Furniture":
                                description.SetActive(true);
                                selectedObject = hit.transform.gameObject;
                                break;

                            case "Decoration":
                                description.SetActive(true);
                                selectedObject = hit.transform.gameObject;
                                break;

                            case "Wall":
                                description.SetActive(true);
                                selectedObject = hit.transform.gameObject;
                                break;

                            case "Floor":
                                description.SetActive(true);
                                selectedObject = hit.transform.gameObject;
                                break;

                        }
                    }
                }
                isPrimaryButtonPressed = primaryvalue;
            }
        }
        else
        {
            isPrimaryButtonPressed = primaryvalue;
        }

        if (targetdevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue))
        {
            if ((isGrippingAnObject && grippedObject) && (grippedObjectTag == "Furniture" || grippedObjectTag == "Decoration"))
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
                if (primary2DAxisValue.y > 0.5f)
                {
                    grippedObject.transform.GetComponent<Rigidbody>().velocity = rotationOfVrRigAndCamera * new Vector3(0, 0, (primary2DAxisValue.y));
                }
                if (primary2DAxisValue.y < -0.5f)
                {
                    grippedObject.transform.GetComponent<Rigidbody>().velocity = rotationOfVrRigAndCamera * new Vector3(0, 0, (primary2DAxisValue.y));
                }
                if (primary2DAxisValue.y > -0.5f && primary2DAxisValue.y < 0.5f)
                {
                    grippedObject.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
            else
            {
                hasRotatedAnObject = false;
            }
        }

        if (targetdevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
        {
            teleportationRay.gameObject.SetActive(true);
        }
        else
        {
            teleportationRay.gameObject.SetActive(false);
        }

        if(rayInteractor.GetCurrentRaycastHit(out RaycastHit RayHit))
        {
            if(noMenu == true)
            {
                if (RayHit.transform.tag == "Floor" || RayHit.transform.tag == "Wall")
                {
                    lineVisual.enabled = false;
                }
            }
        }
    }

    public void SetGrippedObject(GameObject ray)
    {
        if (ray.GetComponent<XRRayInteractor>().GetCurrentRaycastHit(out RaycastHit hit))
        {
            if(hit.transform.tag == "Furniture" || hit.transform.tag == "Decoration")
            {
                grippedObjectTag = hit.transform.tag;
                Debug.Log("Selected");
                isGrippingAnObject = true;
                grippedObject = hit.transform.gameObject;
                interactionRay.gameObject.GetComponent<XRInteractorLineVisual>().enabled = false;
                VrRig.GetComponent<SnapTurnProvider>().enabled = false;
            }
        }
    }

    public void ReleaseGrippedObject()
    {
        grippedObjectTag = null;
        Debug.Log("Released");
        isGrippingAnObject = false;
        interactionRay.gameObject.GetComponent<XRInteractorLineVisual>().enabled = true;
        if(grippedObject && grippedObject.tag == "Furniture")
        {
            grippedObject.transform.position = new Vector3(grippedObject.transform.position.x, 0f, grippedObject.transform.position.z);
        }
        grippedObject = null;
        VrRig.GetComponent<SnapTurnProvider>().enabled = true;
    }

    public void DeleteTargetedObject()
    {
        Destroy(selectedObject);
        description.SetActive(false);
    }

    public void FreezeTargetedObject()
    {
        if (selectedObject.GetComponent<Rigidbody>())
        {
            if (selectedObject.GetComponent<Rigidbody>().isKinematic)
            {
                selectedObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                selectedObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void RotateAround(Transform transform, Vector3 controllerPosition, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        distance = Vector3.Distance(controllerPosition, transform.position);
        rotationOfVrRigAndCamera = Quaternion.Normalize(new Quaternion(0, VrRig.transform.rotation.y, 0, VrRig.transform.rotation.w) * new Quaternion(rotation.x, rotation.y, 0, rotation.w));
        transform.GetComponent<Rigidbody>().velocity += rotationOfVrRigAndCamera * new Vector3(angularVelocity.y * distance, -angularVelocity.x * distance, velocity.z * distance);
    }
}
