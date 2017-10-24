using UnityEngine;
using System.Collections;

public class ServerEventTrigger : MonoBehaviour
{
  public GameObject m_collisionStart;
  public GameObject m_server;
  public bool isCoolingElement = false;
  public ServerSlotType m_type = ServerSlotType.none;

  void Start()
  {
    CustomCollisionDetection cc = GetComponent<CustomCollisionDetection>();
    cc.ListenOnCollision("", Enter, Exit);
  }

  void Enter(GameObject a_other)
  {
    GameObject go = m_collisionStart.GetComponent<IServerCollision>().GetCollisionObject();
    if (go == null)
    {
      return;
    }


    if (go.tag == StringManager.Tags.ServerCoolingElement)
    {
      go.transform.parent.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    InteractableItem interactableItem = go.transform.parent.gameObject.GetComponent<InteractableItem>();
    if (interactableItem && interactableItem.m_isDamaged)
    {
      m_server.GetComponent<Server>().PlaySound(ServerSlotState.Remove);
      Debug.Log("Damaged object has been inserted.");
      return;
    }

    if (go.tag == StringManager.Tags.ServerCpu)
    {
      go.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true; // this is required so the object won't move itself out
    }

    if (a_other.gameObject == m_collisionStart.GetComponent<IServerCollision>().GetCollisionObject())
    {
      // object snapping successful, we can tell the server that a slot has been filled
      Debug.Log("inserted");
      if (isCoolingElement)
      {
        // make door not kinematic anymore
        //Debug.Log(this.transform.parent.parent.gameObject.name + " ==> Setting isKinematic to false");
        //this.transform.parent.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false;        
      }
        
      m_server.gameObject.GetComponent<Server>().UpdateServerSlot(ServerSlotState.Insert, m_type);
      m_server.GetComponent<Server>().PlaySound(ServerSlotState.Insert);
    }
  }

  void Exit(GameObject a_other)
  {
    if (a_other.gameObject == m_collisionStart.GetComponent<IServerCollision>().GetCollisionObject())
    {

      GameObject go = m_collisionStart.GetComponent<IServerCollision>().GetCollisionObject();

      InteractableItem interactableItem = go.transform.parent.gameObject.GetComponent<InteractableItem>();
      if (interactableItem && interactableItem.m_isDamaged)
      {
        m_server.GetComponent<Server>().PlaySound(ServerSlotState.Remove);
        Debug.Log("Damaged object has been inserted.");
        return;
      }


      if (go.tag == StringManager.Tags.ServerCpu)
      {
        go.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false; // this is required so the object won't move itself out
      }

      // object snapping successful, we can tell the server that a slot has been filled
      Debug.Log("removed");
      m_server.gameObject.GetComponent<Server>().UpdateServerSlot(ServerSlotState.Remove, m_type);
      m_server.GetComponent<Server>().PlaySound(ServerSlotState.Remove);
      a_other.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false; // this is required so the object won't move itself out
    }
  }
}
