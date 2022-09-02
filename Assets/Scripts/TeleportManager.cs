using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportManager : MonoBehaviour
{
    public XRController teleportationRay;
    public float pressTreshold;
    public InputHelpers.Button teleportButton;

    void Update()
    {
        if(teleportationRay)
        {
            InputHelpers.IsPressed(teleportationRay.inputDevice, teleportButton, out bool isPressed, pressTreshold);
            teleportationRay.gameObject.SetActive(isPressed);
        }
    }
}
