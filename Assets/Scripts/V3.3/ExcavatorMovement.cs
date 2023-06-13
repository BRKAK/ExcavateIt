using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/*
 * *********
 *         *
 *!WARNING!*
 *         *
 * *********
 * Do not delete any of the commented codes segments!
 * 
 * 
 */
public class ExcavatorMovement : MonoBehaviour
{
    [SerializeField] GameObject rightTrack, leftTrack, excavatorPivot, body, boom, stick, bucket,
        bucketJoint, leftJoystick, rightJoystick, leftPedal, rightPedal , rightJoystickBase, 
        leftJoystickBase, throttle, throttlePivot , key;
    [SerializeField] float speed = 1f, hydrolicPressure = 5f;

    [SerializeField] GameObject leftRayInteractor, rightRayInteractor;

    public UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable throttleXRGrab, keyXRGrab;//This is the script attached to throttle object.
    //It is needed because the actual input has to be overridden.

    private Animator left, right;
    private const float speedMultiplier = 0.765f, rotationMultiplier = 10f;
    private Quaternion leftJoystickSensitivity, rightJoystickSensitivity, leftPedalSensitivity, rightPedalSensitivity;

    private InputDevice rightController, leftController;
    private List<InputDevice> rightHandDevices, leftHandDevices;
    private float angleBeforeSelection, throttleSensitivity = 0f, throttleVal = 0.5f, throttleRotation = 0f;

    GasSoundScript gasSound;
    rpmHeatFuel rpmHeatFuel;
    bool onceSoundFlag = true;

    bool isEngineIgnited = false, 
        isSwingLocked = true, 
        isSafetyLockLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        //Animation intialization.
        left = leftTrack.GetComponent<Animator>();
        right = rightTrack.GetComponent<Animator>();

        //VR Headset and Controllers' setup.
        rightHandDevices = new List<InputDevice>();
        leftHandDevices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, rightHandDevices);
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, leftHandDevices);

        foreach (var item in rightHandDevices)
        {
            //Debug.Log("RIGHT:"+item.name + item.characteristics);
        }
        foreach (var item in leftHandDevices)
        {
            //Debug.Log("LEFT:" + item.name + item.characteristics);
        }
        
        if (rightHandDevices.Count > 0 && leftHandDevices.Count > 0)
        {
            rightController = rightHandDevices[0];
            leftController = leftHandDevices[0];
        }
        
        gasSound = FindObjectOfType<GasSoundScript>();
        rpmHeatFuel = FindObjectOfType<rpmHeatFuel>();
    }

    bool throttleIncreasedPrev = false;

    // Update is called once per frame
    void Update()
    {

        if(isEngineIgnited && !isSafetyLockLocked){
            
            leftJoystick.GetComponent<Rigidbody>().detectCollisions = true;
            rightJoystick.GetComponent<Rigidbody>().detectCollisions = true;
                /**THIS IS FOR DEBUGGING PURPOSES ON PC NO NEED FOR STANDALONE**/
            if (rightHandDevices.Count == 0 || leftHandDevices.Count == 0) //To check whether there is a well-functioning device or not. 
            {
                Start();
            }
            /**End**/


            //Debug.Log("TAG: " + rightJoystick.tag);
            rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rightControllerRotationVal); //3D Rotational Input from the VR Controller.
            leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion leftControllerRotationVal);//3D Rotational Input from the VR Controller.


            rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightControllerJoystickVal); //2D Axis Joystick Input from the VR Controller.
            //Debug.Log("Right thumb: " + rightControllerJoystickVal);
            rightPedalSensitivity = Quaternion.Euler(rightControllerJoystickVal.y * 20, 0, 0); 
            rightPedal.transform.localRotation = rightPedalSensitivity;

            leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftControllerJoystickVal); //2D Axis Joystick Input from the VR Controller.
            //Debug.Log("Left thumb: " + leftControllerJoystickVal);
            leftPedalSensitivity = Quaternion.Euler(leftControllerJoystickVal.y * 20, 0, 0);
            leftPedal.transform.localRotation = leftPedalSensitivity;

            throttleControl(rightControllerRotationVal, leftControllerRotationVal);

            
            if(leftControllerJoystickVal.y!=0){
                leftTrackMovement();
            }  
                
            if (rightControllerJoystickVal.y != 0){
                rightTrackMovement();
            }  

            if(rightControllerJoystickVal.y != 0 || leftControllerJoystickVal.y!=0){
                if(onceSoundFlag){
                    Debug.Log("go gas");
                    gasSound.playGasSoundSFX();
                    onceSoundFlag = false;
                    
                }   
                rpmHeatFuel.changeHeat(10f); 
                rpmHeatFuel.changeFuel(-5F);
            }else if(rightControllerJoystickVal.y != 0 && leftControllerJoystickVal.y!=0){
                if(onceSoundFlag){
                    Debug.Log("go gas");
                    gasSound.playGasSoundSFX();
                    onceSoundFlag = false;
                    
                }  
                rpmHeatFuel.changeHeat(10f);
                rpmHeatFuel.changeFuel(-5F);
            }else{
                if(!onceSoundFlag){
                    gasSound.stopGasSoundSFX();
                    Debug.Log("stop gas");
                    onceSoundFlag = true;
                   
                }
                rpmHeatFuel.changeHeat(-250f);
            }

            //Checking which object is grabbed by the controller
            if (leftJoystick.tag == "selected") 
            {
                rightRayInteractor.SetActive(false);
                leftRayInteractor.SetActive(false);
                leftJoystickSensitivity = Quaternion.Euler(leftControllerRotationVal.x, leftControllerRotationVal.z, 0);
                if(!isSwingLocked){
                    SwingExcavator();
                }
                RotateStick();
            }
            else if (leftJoystick.tag == "Untagged") //Smooth translation to initial point when the joystick is relased.
            {
                leftJoystick.transform.rotation = Quaternion.RotateTowards(leftJoystick.transform.rotation, Quaternion.Euler(-90, leftJoystickBase.transform.rotation.eulerAngles.y, 0), 250 * Time.deltaTime);
                leftJoystickSensitivity = Quaternion.Euler(0, 0, 0);
                rightRayInteractor.SetActive(true);
                leftRayInteractor.SetActive(true);
            }

            if (rightJoystick.tag == "selected")
            {
                rightRayInteractor.SetActive(false);
                leftRayInteractor.SetActive(false);
                rightJoystickSensitivity = Quaternion.Euler(rightControllerRotationVal.x, rightControllerRotationVal.z, 0);
                RotateBoom();
                RotateBucket();
            }
            else if(rightJoystick.tag == "Untagged") //Smooth translation to initial point when the joystick is relased.
            {
                rightJoystick.transform.rotation = Quaternion.RotateTowards(rightJoystick.transform.rotation, Quaternion.Euler(-90, rightJoystickBase.transform.rotation.eulerAngles.y, 0), 250 * Time.deltaTime);
                rightJoystickSensitivity = Quaternion.Euler(0, 0, 0);
                rightRayInteractor.SetActive(true);
                leftRayInteractor.SetActive(true);
            }
            SetUpInputSensitivity(); // virtual joystick sensitivity configurations
            
            if(key.tag == "selected")
            {
                throttleXRGrab.transform.localRotation = Quaternion.Euler(0, throttleXRGrab.transform.rotation.eulerAngles.z + throttle.transform.rotation.eulerAngles.z, 0);
            }
        }else{
            leftJoystick.GetComponent<Rigidbody>().detectCollisions = false;
            rightJoystick.GetComponent<Rigidbody>().detectCollisions = false;
        }
    }

    float setUpThrottleMultiplier(float b, float t)//rotational value of throttle
    {
        //b>t 1-b>180,t<180  2-b>180,t>180  3-b<180,t<180
        if (b > t)
        {
            if (b > 180)
            {
                if (t < 180)
                    return 360 - b + t;
                return (b - t) * -1;
            }
            return (b - t) * -1;
            
        }
        //b<t 1-b<180,t>180 2-b>180,t>180  3-b<180,t<180
        else
        {
            if (b < 180)
            {
                if (t > 180)
                    return (360 - t + b) * -1;
                return t - b;
            }
            return t - b;
        }
    }

    void SetUpInputSensitivity() //input sensitivity from joysticks
    {
        if(rightJoystick.transform.localRotation.eulerAngles.x > 359.8f || rightJoystick.transform.localRotation.eulerAngles.x < 0.2f)
        {
            if(rightJoystick.transform.localRotation.eulerAngles.x > 359.8f)
            {
                rightJoystickSensitivity = Quaternion.Euler(rightJoystick.transform.localRotation.eulerAngles.x * -10, rightJoystick.transform.localRotation.eulerAngles.y * -10, rightJoystick.transform.localRotation.eulerAngles.z * -10);
            }
            else
            {
                rightJoystickSensitivity = Quaternion.Euler(rightJoystick.transform.localRotation.eulerAngles.x * 10, rightJoystick.transform.localRotation.eulerAngles.y * 10, rightJoystick.transform.localRotation.eulerAngles.z * 10);
            }
            //Debug.Log(rightJoystickSensitivity.x);
        }
        if (leftJoystick.transform.localRotation.eulerAngles.x > 359.8f || leftJoystick.transform.localRotation.eulerAngles.x < 0.2f)
        {
            if (leftJoystick.transform.localRotation.eulerAngles.x > 359.8f)
            {
                leftJoystickSensitivity = Quaternion.Euler(leftJoystick.transform.localRotation.eulerAngles.x * -10, leftJoystick.transform.localRotation.eulerAngles.y * -10, leftJoystick.transform.localRotation.eulerAngles.z * -10);
            }
            else
            {
                rightJoystickSensitivity = Quaternion.Euler(leftJoystick.transform.localRotation.eulerAngles.x * 10, leftJoystick.transform.localRotation.eulerAngles.y * 10, leftJoystick.transform.localRotation.eulerAngles.z * 10);
            }
            //Debug.Log(rightJoystickSensitivity.x);
        }
    }

    void RotateBucket() //Bucket rotation according to the virtual input from joysticks
    {
        //Debug.Log("bucket.transform.localRotation.x" + bucket.transform.localRotation.x);
        if (bucket.transform.localRotation.x < -0.15) //Rotational limitations
        {
            bucket.transform.Rotate(hydrolicPressure * Time.deltaTime * rightJoystickSensitivity.y * 100 * rotationMultiplier * throttleVal, 0, 0);
            bucketJoint.transform.Rotate(hydrolicPressure * Time.deltaTime * rightJoystickSensitivity.y * 100 * rotationMultiplier * throttleVal, 0, 0);
        }
        else
        {
            bucket.transform.Rotate(-1, 0, 0);
            bucketJoint.transform.Rotate(-1, 0, 0);
        }
        if (bucket.transform.localRotation.x > -0.88)
        {
            bucket.transform.Rotate(hydrolicPressure * Time.deltaTime * rightJoystickSensitivity.y * 100 * rotationMultiplier * throttleVal, 0, 0);
            bucketJoint.transform.Rotate(hydrolicPressure * Time.deltaTime * rightJoystickSensitivity.y * 100 * rotationMultiplier * throttleVal, 0, 0);
        }
        else
        {
            bucket.transform.Rotate(1, 0, 0);
            bucketJoint.transform.Rotate(1, 0, 0);
        }
        ResetObjectPositions();
    }

    void ResetObjectPositions() //Due to XR Built-in scripts objects that are being grabbed have a problem on movement. This is a patch for grabable objects' positions.
    {
        leftJoystick.transform.position = leftJoystickBase.transform.position + new Vector3(0, 0, 0/*.0002f*/);
        rightJoystick.transform.position = rightJoystickBase.transform.position + new Vector3(0, 0, 0/*.0002f*/);
        leftJoystick.transform.rotation = leftJoystickBase.transform.rotation;
        rightJoystick.transform.rotation = rightJoystickBase.transform.rotation;
        throttle.transform.SetPositionAndRotation(throttlePivot.transform.position, throttle.transform.rotation);
    }

    void SwingExcavator() //Rotation of the Excavator's body according to the virtual input from joysticks.
    {

        ResetObjectPositions();
        body.transform.Rotate(0, hydrolicPressure * Time.deltaTime * leftJoystickSensitivity.y * -200 * rotationMultiplier * throttleVal, 0);
    }

    void RotateBoom() //Rotation of Boom according to the virtual input from joysticks.
    {
        if (boom.transform.localRotation.x > 0.5)
        {
            boom.transform.Rotate(hydrolicPressure * Time.deltaTime * rightJoystickSensitivity.x * 200 * throttleVal, 0, 0);
        }
        else
        {
            boom.transform.Rotate(1, 0, 0);
        }
        if (boom.transform.localRotation.x < 0.88)
        {
            boom.transform.Rotate(hydrolicPressure * Time.deltaTime * rightJoystickSensitivity.x * 200 * throttleVal, 0, 0);
        }
        else
        {
            boom.transform.Rotate(-1, 0, 0);
        }
        //Debug.Log(boom.transform.localRotation.x);
        ResetObjectPositions();
    }

    void RotateStick() //Rotation of Stick according to the virtual input from joysticks.
    {
        //Debug.Log("Stick Angles EULER: " + stick.transform.eulerAngles);
        //Debug.Log("Stick Angles STANDARD: " + stick.transform.rotation.x);
        if (stick.transform.localRotation.x > -0.99)
        {
            stick.transform.Rotate(hydrolicPressure * Time.deltaTime * leftJoystickSensitivity.x * 200 * rotationMultiplier * -1 * throttleVal, 0, 0);
        }
        else
        {
            stick.transform.Rotate(1, 0, 0);
        }
        if (stick.transform.localRotation.x < -0.41)
        {
            stick.transform.Rotate(hydrolicPressure * Time.deltaTime * leftJoystickSensitivity.x * 200 * rotationMultiplier * -1 * throttleVal, 0, 0);
        }
        else
        {
            stick.transform.Rotate(-1, 0, 0);
        }
        ResetObjectPositions();
    }

    void leftTrackMovement() //Left track movement and animation with the input from the joysticks on vr controller.
    {
        ResetObjectPositions();
        //Debug.Log("leftPedal: " + leftPedalSensitivity.x * 5);
        if(leftPedalSensitivity.x > 0) //Animation speed configuration according to the virtual pedals.
        {
            left.Play("forward");
            left.speed = leftPedalSensitivity.x *  speed * 2;
        }
        else
        {
            left.Play("backward");
            left.speed = leftPedalSensitivity.x * speed * -2;
        }

        //Movement of the excavator according to the pivot point.
        excavatorPivot.transform.Translate(0, 0, speed * speedMultiplier * Time.deltaTime * leftPedalSensitivity.x * 5);
        excavatorPivot.transform.Rotate(0, speed * Time.deltaTime * rotationMultiplier * leftPedalSensitivity.x * 5, 0);
    
    }

    void rightTrackMovement() //Right track movement and animation with the input from the joysticks on vr controller.
    {
        ResetObjectPositions();
        if(rightPedalSensitivity.x > 0) //Animation speed configuration according to the virtual pedals.
        {
            right.Play("forward");
            right.speed = speed * rightPedalSensitivity.x * 2;
        }
        else {
            right.Play("backward");
            right.speed = speed * rightPedalSensitivity.x * -2;
        }

        //Movement of the excavator according to the pivot point.
        excavatorPivot.transform.Translate(0, 0, speed * speedMultiplier * Time.deltaTime * rightPedalSensitivity.x * 5);
        excavatorPivot.transform.Rotate(0, speed * Time.deltaTime * rotationMultiplier * -5 * rightPedalSensitivity.x, 0);
    
    }

    void throttleControl(Quaternion rightControllerRotationVal, Quaternion leftControllerRotationVal){
        
        //1.1
            if(throttle.tag == "selected")
            {
                rightRayInteractor.SetActive(false);
                leftRayInteractor.SetActive(false);

                ResetObjectPositions();
                
                throttleXRGrab.transform.localRotation = Quaternion.Euler(0, 0, throttleXRGrab.transform.rotation.eulerAngles.z + throttle.transform.rotation.eulerAngles.z);
                
                throttleSensitivity = Quaternion.Euler(0,0, rightControllerRotationVal.eulerAngles.z - body.transform.rotation.eulerAngles.y).eulerAngles.z;
                /*throttleSensitivity = rightControllerRotationVal.eulerAngles.z - angleBeforeSelection;
                Debug.Log("Controller Rot Z: "+rightControllerRotationVal.z);
                if (throttleSensitivity >= 300)
                {
                    throttleSensitivity = (360 - throttleSensitivity) / 60;
                    throttleIsNegative = false;
                }
                else if(throttleSensitivity <= 60)
                {
                    throttleSensitivity = throttleSensitivity / -60;
                    throttleIsNegative = true;
                }
                else
                {
                    if (throttleIsNegative)
                    {
                        throttleSensitivity = -1;
                    }
                    else
                    {
                        throttleSensitivity = 1;
                    }
                }*/
                throttleRotation = (throttleSensitivity - angleBeforeSelection) * -1;
                
                throttle.transform.Rotate(new Vector3(-90, throttle.transform.localRotation.eulerAngles.y, throttleRotation));

                //b>t 1-b>180,t<180  2-b>180,t>180  3-b<180,t<180
                //b<t 1-b<180,t>180 2-b>180,t>180  3-b<180,t<180
                throttleRotation = setUpThrottleMultiplier(body.transform.rotation.eulerAngles.y, throttle.transform.localRotation.eulerAngles.y);

                Debug.Log("Local Rotation: " + throttle.transform.localRotation.eulerAngles.y);
                Debug.Log("Body Local Rotation: " + body.transform.rotation.eulerAngles.y);
                Debug.Log("Throttle Rotation: " + throttleRotation);

                /*if(body.transform.rotation.eulerAngles.y > 180f)
                {
                    throttleRotation = setUpThrottleMultiplier(body.transform.rotation.eulerAngles.y, throttle.transform.rotation.eulerAngles.y);
                    //throttleRotation = (throttle.transform.localRotation.eulerAngles.y - (360 - body.transform.rotation.eulerAngles.y));
                    throttleRotation = throttle.transform.localRotation.eulerAngles.y - body.transform.rotation.eulerAngles.y;
                    Debug.Log("Body rotation is over 180, throttle value: " + throttleRotation);
                }
                else
                {
                    if(throttle.transform.localRotation.eulerAngles.y > 180)
                    {
                        throttleRotation = setUpThrottleMultiplier(body.transform.rotation.eulerAngles.y, throttle.transform.rotation.eulerAngles.y)
                        Debug.Log("Geldi: " + throttleRotation);
                    }
                }*/
                Debug.Log("Throttle Rotation After if: " + throttleRotation);
                Debug.Log(throttleRotation >= -30 && throttleRotation < 0);
                if (throttleRotation <= 30 && throttleRotation > 0)
                {
                    Debug.Log("0 - 30: " + throttleRotation);
                    throttleVal = (throttleRotation + 30) / 30;
                    throttleIncreasedPrev = true;
                }
                else if(throttleRotation >= -30 && throttleRotation < 0)
                {
                    Debug.Log("-30 - 0: " + throttleRotation);
                    throttleVal = (throttleRotation + 30) / 30;
                    throttleIncreasedPrev = false;
                }
                else
                {
                    if (throttleIncreasedPrev)
                    {
                        throttleVal = 2;
                        throttleRotation = 30;

                    }
                    else
                    {
                        throttleVal = 0.1f;
                        throttleRotation = -30;

                    }
                }
                Debug.Log("Throttle Val Scaled: " + throttleVal);
                
                rpmHeatFuel.changePower(throttleVal);
                //Debug.Break();
                //throttle.GetComponent<SphereCollider>().enabled = true;
            }
            else if(throttle.tag == "Untagged")
            {
                rightRayInteractor.SetActive(true);
                leftRayInteractor.SetActive(true);
                //Debug.Log("Before Selection: " + angleBeforeSelection);
                angleBeforeSelection = rightControllerRotationVal.eulerAngles.z;
                
            }
            //1.1

        
    }

    public void changeStatus(string type, bool val){
        if(type == "ignite"){
            isEngineIgnited = val;
        }else if(type == "swingLock"){
            isSwingLocked = val;
        }else if(type == "safetyLock"){
            isSafetyLockLocked = val;
        }
    }
    
}
    
