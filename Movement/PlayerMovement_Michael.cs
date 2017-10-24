using UnityEngine;
using System.Collections;
// http://i-cdn.phonearena.com/images/articles/226599-image/The-two-controllers-that-come-with-the-device-out-of-the-box.jpg


public class PlayerMovement_Michael : MonoBehaviour
{

    public int m_DeviceIdLeftController = 3;
    public int m_DeviceIdRightController = 4;
    public float m_WalkingSpeed = 7.0f;
    public float m_RunningSpeed = 10.0f;

    public GameObject m_VrCameraMain; // real position of camera
    public GameObject m_VrCameraEye; // required to get correct look-forward vector
    //private GameObject m_ControllerLeft;
    //private GameObject m_ControllerRight;
    private float m_FixedHeight = 2.0f;
    SteamVR_Controller.Device m_DeviceLeft;
    SteamVR_Controller.Device m_DeviceRight;
    //float range = 0.5f;

    public float m_RotateBackwardsInThisSeconds = 2.0f;
    private bool m_HoldingDownwards = false;
    private float m_TurnAroundTimer = 2.0f;

    [Tooltip("Border Value for all Directions. Used for Walking/Running, Strafing, Possibly Turning Around 180°.")]
    public float m_MaximumThresholdWalking = 0.7f; 

    void Start()
    {
        // get device ids
        m_DeviceLeft = SteamVR_Controller.Input(m_DeviceIdLeftController);
        m_DeviceRight = SteamVR_Controller.Input(m_DeviceIdRightController);
        m_FixedHeight = m_VrCameraMain.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        #region Left Controller Touchpad
        if (m_DeviceLeft.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("m_DeviceLeft : axis");

            //Read the touchpad values
            Vector2 axis = m_DeviceLeft.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

            //Debug.Log("Check axis.x = " + axis.x);
            if (axis.x <= m_MaximumThresholdWalking && axis.x >= -m_MaximumThresholdWalking)
            {
                axis.x = 0.0f;
                // Debug.Log("Detect Forward Movement axis.y = " + axis.y);
                if (axis.y >= 0.01f && axis.y <= m_MaximumThresholdWalking)
                {
                    Vector3 walk = m_VrCameraMain.transform.position + (m_VrCameraEye.transform.forward * m_WalkingSpeed);
                    walk.y = m_FixedHeight;
                    m_VrCameraMain.transform.position = walk;
                    
                    // Undo Rotation Timer
                    m_HoldingDownwards = false;
                    m_TurnAroundTimer = 0.0f;
                    return;
                }
                else if (axis.y > m_MaximumThresholdWalking)
                {
                    Vector3 walk = m_VrCameraMain.transform.position + (m_VrCameraEye.transform.forward * m_RunningSpeed);
                    walk.y = m_FixedHeight;
                    m_VrCameraMain.transform.position = walk;

                    // Undo Rotation Timer
                    m_HoldingDownwards = false;
                    m_TurnAroundTimer = 0.0f;
                    return;
                }

                // Turn around
                //if (m_HoldingDownwards)
                //{
                //    m_TurnAroundTimer -= Time.deltaTime;
                //    if(m_TurnAroundTimer <= 0.0f)
                //    {
                //        Quaternion rotate = m_VrCameraEye.transform.rotation;
                //        rotate.y += 180.0f;
                //        m_VrCameraEye.transform.rotation = rotate;
                //        m_HoldingDownwards = false;
                //        m_TurnAroundTimer = 0.0f;
                //    }
                //}
                //else if(axis.y < -m_MaximumThresholdWalking && !m_HoldingDownwards)
                //{
                //    m_HoldingDownwards = true;
                //    m_TurnAroundTimer = m_RotateBackwardsInThisSeconds;
                //}
            }

            /*
            // Evade to Side
            if (axis.y <= m_MaximumThresholdWalking && axis.y >= -m_MaximumThresholdWalking)
            {
                axis.y = 0.0f;
                if (axis.x < -m_MaximumThresholdWalking)
                {
                    // Strafe Left
                    Vector3 walk = m_VrCameraMain.transform.position - (m_VrCameraEye.transform.right * m_RunningSpeed);
                    walk.y = m_FixedHeight;
                    m_VrCameraMain.transform.position = walk;
                }

                if (axis.x > m_MaximumThresholdWalking)
                {
                    // Strafe Right
                    Vector3 walk = m_VrCameraMain.transform.position + (m_VrCameraEye.transform.right * m_RunningSpeed);
                    walk.y = m_FixedHeight;
                    m_VrCameraMain.transform.position = walk;
                }
            }
            */
        }
        #endregion

        #region Right Controller Touchpad
        if (m_DeviceRight.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            //Read the touchpad values
            // Vector2 tmp = m_DeviceRight.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            //Debug.Log("Magnitude: " + tmp.magnitude);
            //Debug.Log(tmp);
        }
        #endregion

    }
}
