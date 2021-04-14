using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class LocomotionController : MonoBehaviour
{
    public XRController leftTeleRay;
    public XRController rightTeleRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if(leftTeleRay)
        {
            leftTeleRay.gameObject.SetActive(CheckIfActivated(leftTeleRay));
        }

        if (rightTeleRay)
        {
            rightTeleRay.gameObject.SetActive(CheckIfActivated(rightTeleRay));
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold); //if trigger pressed beyond threshold, return true
        return isActivated;
    }
}
