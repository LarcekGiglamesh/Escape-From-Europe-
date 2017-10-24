using UnityEngine;
using System.Collections;
using System;

public class ServerCPU : MonoBehaviour, IServerCollision
{
  public GameObject m_lookAt = null;
  private GameObject m_cpuObject = null;

  void Start()
  {
    Transform server = this.transform.parent.parent;

    CustomCollisionDetection ccd = GetComponent<CustomCollisionDetection>();
    ccd.ListenOnCollision(StringManager.Tags.ServerCpu, CustomOnTriggerEnter, CustomOnTriggerExit);
  }

  public void CustomOnTriggerEnter(GameObject a_other)
  {
    m_cpuObject = a_other;
    Run(a_other);
  }

  void CustomOnTriggerExit(GameObject a_other)
  {
    // enable rigidbody movement stuff
    Rigidbody rigidbody = a_other.transform.parent.GetComponent<Rigidbody>();

    if (rigidbody)
    {
      rigidbody.constraints = RigidbodyConstraints.None;
      rigidbody.velocity = Vector3.zero;
    }

    m_cpuObject = null;
  }

  void Run(GameObject a_hdd)
  {
    Transform interactableObject = a_hdd.transform.parent; // this is the gameobject which needs rotation, ...

    // get server rotation (which is the parent from this object)
    Transform server = this.transform.parent.parent;

    InteractableItem interactableItem = interactableObject.GetComponent<InteractableItem>();

    Vector3 newPos = this.transform.GetChild(0).position;
    interactableItem.transform.position = newPos;

    interactableItem.transform.LookAt(2 * interactableItem.transform.position - m_lookAt.transform.position);

    // adjust rigidbody movement
    Rigidbody rigidbody = interactableItem.GetComponent<Rigidbody>();
    rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

  }

  public GameObject GetCollisionObject()
  {
    return m_cpuObject;
  }
}
