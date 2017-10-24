using UnityEngine;
using System.Collections;

// This script will wait until SteamVR is initialized and active
// After that is done, it will enable the whole Game
// This is required in order to not have any errors regarding SteamVR device index access before they were initialized.

public class InitializeGame : MonoBehaviour
{
  //public GameObject m_initialize = null; // this gameobject will be actiaved when SteamVR is active
  public GameObject m_mainCamera = null;

  public GameObject m_controllerLeft;
  public GameObject m_controllerRight;

  public bool m_Visible = true;
  
  void Update ()
  {
    if (IsLeftControllerActive())
    {
      GameObject.Find("SteamVRManager").GetComponent<SteamVRManager>().enabled = true;

      //Debug.Log("InitializeGame => SteamVR is active. We can run the game now.");
      //m_initialize.SetActive(true);

      // enable all scripts which are attached to SteamVR gameobject, e. g. on the controller, or camera, or whatever.
      SingletonManager.SteamVRManager.m_controllerRight.GetComponent<WandController>().enabled = true;
      SingletonManager.SteamVRManager.m_controllerLeft.GetComponent<WandController>().enabled = true;

      PlayerMovement_Sebastian playerMovement = m_mainCamera.GetComponent<PlayerMovement_Sebastian>();
      playerMovement.enabled = true;



      Debug.Log("InitializeGame => Enabled components on SteamVR which had to be disabled.");
      
      // http://answers.unity3d.com/questions/1177557/hiding-steamvr-vive-controller-models.html
      SteamVR_Utils.Event.Send("hide_render_models", !m_Visible);

      this.gameObject.SetActive(false);
    }
	}

  bool IsLeftControllerActive()
  {
    if (m_controllerLeft.GetComponent<SteamVR_TrackedObject>().index != SteamVR_TrackedObject.EIndex.None)
    {
      return true;
    }

    return false;
  }
}
