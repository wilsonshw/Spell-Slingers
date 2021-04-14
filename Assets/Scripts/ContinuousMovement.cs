using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class ContinuousMovement : MonoBehaviour
{
    public float speed = 1;
    public XRNode inputSource;
    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource); //left controller (see inspector)
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis); //analog stick
    }

    private void FixedUpdate()
    {
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0); //head facing angle, about y-axis (vertical)
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y); //multiplying quaternion by the vector rotates the vector by that much angle
        character.Move(direction * Time.fixedDeltaTime * speed);
    }
}
