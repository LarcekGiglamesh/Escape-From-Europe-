using UnityEngine;
using System.Collections;
using Valve.VR;

public class ServerNotebook2Boot : MonoBehaviour
{
  public bool m_isHovering = false;
  public GameObject m_infoScreen = null;

  void Start()
  {
    CustomCollisionDetection ccd = GetComponent<CustomCollisionDetection>();
    ccd.ListenOnCollision(StringManager.Tags.Controller, Enter, Exit);
  }

  void Update()
  {
    if (m_isHovering)
    {
      if (SteamVRManager.m_deviceLeft.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger) || SteamVRManager.m_deviceRight.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
      {
        Debug.Log("KEY PRESSED");
        m_infoScreen.GetComponent<IServerNotebook>().BootSystem();
      }
    }
  }

  void Enter(GameObject a_object)
  {
    m_isHovering = true;
  }

  void Exit(GameObject a_exit)
  {
    m_isHovering = false;
  }
}
