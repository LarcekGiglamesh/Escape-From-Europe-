using UnityEngine;
using System.Collections;
using Valve.VR;

public class PlayerMovement_Sebastian : MonoBehaviour
{
  public float m_acceleration = 0.3f;
  public float m_maxAcceleration = 2.0f;
  public GameObject m_cameraEye = null;
  public float m_moveOnXOffset = 0.2f;
  public GameObject m_controllerLeft = null;
  public GameObject m_controllerRight = null;

  private Rigidbody m_rigidbody;
  private Vector3 m_movement = Vector3.zero;

  void Start()
  {
    m_rigidbody = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    KeyboardMovement();
    TouchpadMovement();
  }

  void TouchpadMovement()
  {
    m_movement = Vector3.zero;

    if (SteamVRManager.m_deviceLeft.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) || SteamVRManager.m_deviceRight.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
    {
      Vector2 axis1 = SteamVRManager.m_deviceRight.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
      Vector2 axis2 = SteamVRManager.m_deviceLeft.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);

      Vector2 axis = axis1 + axis2;

      Vector3 movementZ = Vector3.zero;

      // if y > 0.1f
      if (axis.y >= 0.25f) // 55% cap
      {
        // only forward movement
        movementZ = m_cameraEye.transform.forward * (-axis.y * 100.0f);
        movementZ.y = 0.0f;
        m_movement += movementZ * -1.0f;  // * -1.0f? I dont know...
      }
      else
      {
        // Y-Axis on Touchpad is responsible for movement in z-direction
        movementZ = m_cameraEye.transform.forward * (-axis.y * 100.0f);
        movementZ.y = 0.0f;
        //movementZ.Normalize();
        m_movement += movementZ * -1.0f;  // * -1.0f? I dont know...

        // X-Axis on Touchpad is responsible for movement in x-direction
        if (Mathf.Abs(axis.x) > m_moveOnXOffset)
        {
          if (axis.x > 0)
          {
            axis.x -= m_moveOnXOffset;
          }
          else
          {
            axis.x += m_moveOnXOffset;
          }

          Vector3 movementX = m_cameraEye.transform.right * (-axis.x * 100.0f);
          movementX.y = 0.0f;
          // x-axis movement only 50%
          movementX *= 1.65f;
          //movementX.Normalize();

          m_movement += movementX * -1.0f; // * -1.0f? I dont know...
        }
      }

      MoveThroughPhysics();
    }
  }

  void MoveThroughPhysics()
  {
    // frame-independant movement
    Vector3 playerMovement = m_movement * m_acceleration * Time.fixedDeltaTime;
    // move camera (player)
    //m_rigidbody.MovePosition(this.transform.position + playerMovement);
    m_rigidbody.velocity += playerMovement;

    if (m_rigidbody.velocity.magnitude > m_maxAcceleration)
    {
      m_rigidbody.velocity = m_rigidbody.velocity.normalized * 2.0f;
    }

    // move controllers
    //Vector3 controllerMovement = (-1.0f * m_movement) * m_speed * Time.fixedDeltaTime;

    //m_controllerLeft.transform.Translate(new Vector3(1, 0, 0));
    //m_controllerRight.transform.Translate(controllerMovement);

    //m_controllerLeft.GetComponent<Rigidbody>().MovePosition(m_controllerLeft.transform.position + m_movement);
    //m_controllerRight.GetComponent<Rigidbody>().MovePosition(m_controllerRight.transform.position + m_movement);
  }

  void KeyboardMovement()
  {
    Transform tCameraEye = m_cameraEye.transform;

    if (Input.GetKey(KeyCode.W))
    {
      this.transform.Translate(tCameraEye.forward * Time.deltaTime * m_acceleration, Space.World);
    }
    else if (Input.GetKey(KeyCode.S))
    {
      this.transform.Translate(-1 * tCameraEye.forward * Time.deltaTime * m_acceleration, Space.World);
    }
  }

}

