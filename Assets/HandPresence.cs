using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresence : MonoBehaviour
{
    private InputDevice targetDevice;
    private List<InputDevice> devices;
    // Start is called before the first frame update
    void Start()
    {
        devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        foreach(var item in devices)
        {
            //Debug.Log(item.name + item.characteristics);
        }
        if(devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (devices.Count == 0)
        {
            Start();
        }

        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonVal);
        if (primaryButtonVal)
        {
            //Debug.Log("Primary Button Is Pressed.");
        }
        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerVal);
        if (triggerVal > 0.1f)
        {
            //Debug.Log("Trigger pressed " + triggerVal);
        }
        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisVal);
        if(primary2DAxisVal != Vector2.zero)
        {
            //Debug.Log("Primary touchpad " + primary2DAxisVal);
        }
        targetDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rightControllerRotationVal);
        //Debug.Log("Rotation X: " + rightControllerRotationVal.x + " Rotation Y: " + rightControllerRotationVal.y + " Rotation Z: " + rightControllerRotationVal.z + " Rotation W: " + rightControllerRotationVal.w);
        //Debug.Log(" Rotation : " + rightControllerRotationVal.eulerAngles);
    }
}
