using UnityEngine;
using System.Collections;


public class SteamVRManager : MonoBehaviour
{
  public GameObject m_controllerLeft; // GameObject which must contain the SteamVR controller left
  public GameObject m_controllerRight; // GameObject which must contain the SteamVR controller right

  public static SteamVR_Controller.Device m_deviceLeft;
  public static SteamVR_Controller.Device m_deviceRight;

  void FixedUpdate()
  {
    if (m_deviceLeft == null || m_deviceRight == null)
    {
      int leftIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
      m_deviceLeft = SteamVR_Controller.Input(leftIndex);
      m_controllerLeft.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)leftIndex;

      int rightIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
      m_deviceRight = SteamVR_Controller.Input(rightIndex);
      m_controllerRight.GetComponent<SteamVR_TrackedObject>().index = (SteamVR_TrackedObject.EIndex)rightIndex;
    }
  }

  /// <summary>
  /// returns the left SteamVR Device
  /// </summary>
  //public SteamVR_Controller.Device m_deviceLeft
  //{
  //  get
  //  {
  //    return SteamVR_Controller.Input((int)m_controllerLeft.GetComponent<SteamVR_TrackedObject>().index);
  //  }
  //}

  ///// <summary>
  ///// returns the right SteamVR Device
  ///// </summary>
  //public SteamVR_Controller.Device m_deviceRight
  //{
  //  get
  //  {
  //    return SteamVR_Controller.Input((int)m_controllerRight.GetComponent<SteamVR_TrackedObject>().index);
  //  }
  //}

  /// <summary>
  /// static method to get a reference to the SteamVRManager
  /// </summary>
  /// <returns></returns>
  public static SteamVRManager GetInstance()
  {
    return GameObject.Find(StringManager.Names.SteamVRManager).GetComponent<SteamVRManager>();
  }
}
