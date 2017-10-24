using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

// TODO:
// Add raycast between controller and interaction item while isInteracting
// if it does not hit the controller then stop interaction

public enum Device
{
  left,
  right
}

public class WandController : MonoBehaviour
{
  private Valve.VR.EVRButtonId m_interactionButton = EVRButtonId.k_EButton_Grip;
  public InteractableItem m_interactionItem; // shows with which item we are currently interacting with
  public Device m_device;
  private HashSet<InteractableItem> m_objectsHoveringOver = new HashSet<InteractableItem>(); // contains all InteractableItems which have collided with the controller

  private bool m_isButtonDown = false;

  void Update()
  {
    UpdateButtonState();

    //if (SteamVRManager.m_deviceRight.GetPressDown(m_interactionButton) || SteamVRManager.m_deviceRight.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
    //{
    //  // is there already an interactionItem?
    //  if (m_interactionItem)
    //  {
    //    // finish interaction
    //    m_interactionItem.EndInteraction(this);
    //    m_interactionItem = null;
    //  }
    //  else
    //  {
    //    // get new interaction item
    //    UpdateNearestInteractionItem();

    //    // any item found we can interact with?
    //    if (m_interactionItem)
    //    {
    //      // start interaction
    //      m_interactionItem.BeginInteraction(this);
    //    }
    //  }
    //}

    //MoveAndRotateControllerJustForTest();
  }

  /// <summary>
  /// Sets the nearest interactable item from the hashmap
  /// </summary>
  private void UpdateNearestInteractionItem()
  {
    float minDistance = float.MaxValue;
    float distance;

    InteractableItem closestItem = null;

    foreach (InteractableItem item in m_objectsHoveringOver)
    {
      if (item == null)
      {
        m_objectsHoveringOver.Remove(item);
      }

      distance = (item.transform.position - transform.position).sqrMagnitude;

      if (distance < minDistance)
      {
        minDistance = distance;
        closestItem = item;
      }
    }

    m_interactionItem = closestItem;
  }

  /// <summary>
  /// Adds a interactable item to the hashmap if collision happens between itself and the controller
  /// </summary>
  private void OnTriggerEnter(Collider collider)
  {
    InteractableItem item = collider.GetComponent<InteractableItem>();
    if (item)
    {
      m_objectsHoveringOver.Add(item);
      if (m_interactionItem != item)
      {
        item.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
      }
      
      //Debug.Log("Adding gameobject: " + item.gameObject.name);
    }
  }

  /// <summary>
  /// Removes a interactable item to the hashmap if collision is left between itself and the controller
  /// </summary>
  private void OnTriggerExit(Collider collider)
  {
    InteractableItem item = collider.GetComponent<InteractableItem>();
    if (item)
    {
      //if (item == m_interactionItem)
      //{
      //  if (m_interactionItem.IsInteracting() == false)
      //  {
      //    m_objectsHoveringOver.Remove(item);
      //  }
      //}
      m_objectsHoveringOver.Remove(item);

      if (m_interactionItem != item)
      {
        item.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
      }
      
      //Debug.Log("Removing gameobject: " + item.gameObject.name);
    }
  }

  public void StopInteraction()
  {
    if (m_interactionItem)
    {
      m_objectsHoveringOver.Remove(m_interactionItem);
      m_interactionItem.EndInteraction(this);
      m_interactionItem = null;
    }
  }

  void UpdateButtonState()
  {
    bool currentState = false;

    SteamVR_Controller.Device device = SteamVR_Controller.Input((int)GetComponent<SteamVR_TrackedObject>().index);
    currentState = device.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger);

    //if (m_device == Device.left)
    //{
    //  currentState = SteamVRManager.m_deviceLeft.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger);
    //}
    //else
    //{
    //  currentState = SteamVRManager.m_deviceRight.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger);
    //}

    if (currentState != m_isButtonDown)
    {
      // state
      m_isButtonDown = currentState;
      Test();
    }
  }

  public void RemoveItem(InteractableItem a_interactableItem)
  {
    m_objectsHoveringOver.Remove(a_interactableItem);
  }

  void Test()
  {
    // is there already an interactionItem?
    if (m_interactionItem)
    {
      // finish interaction
      m_interactionItem.EndInteraction(this);
      m_interactionItem = null;
    }
    else
    {
      // get new interaction item
      UpdateNearestInteractionItem();

      // any item found we can interact with?
      if (m_interactionItem)
      {
        // start interaction
        m_interactionItem.BeginInteraction(this);
      }
    }
  }
}
