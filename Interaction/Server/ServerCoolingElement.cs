using UnityEngine;
using System.Collections;

public class ServerCoolingElement : MonoBehaviour, IServerCollision
{
  public GameObject m_lookAt = null;
  private GameObject m_coolingElement = null;
  //private float m_height = 1.280006f;
  //private float m_time = 0.0f;

  void Start()
  {
    Transform server = this.transform.parent.parent;

    CustomCollisionDetection ccd = GetComponent<CustomCollisionDetection>();
    ccd.ListenOnCollision(StringManager.Tags.ServerCoolingElement, Enter, Exit);
  }

  void Enter(GameObject a_other)
  {
    m_coolingElement = a_other.gameObject;
    Run();
  }

  void Exit(GameObject a_other)
  {
    Rigidbody rigidbody = a_other.transform.parent.GetComponent<Rigidbody>();
    if (rigidbody)
      rigidbody.constraints = RigidbodyConstraints.None;

    m_coolingElement = null;
  }
  
  void Run()
  {
    Transform interactableObject = m_coolingElement.transform.parent; // this is the gameobject which needs rotation, ...

    // get server rotation (which is the parent from this object)
    Transform server = this.transform.parent.parent;

    InteractableItem interactableItem = interactableObject.GetComponent<InteractableItem>();

    Vector3 eulerAngles = interactableItem.transform.eulerAngles;

    eulerAngles.x = 270.0f;
    eulerAngles.y = 180.0f; // IF SERVER ROT-Y is 0 THEN 0 ELSE 180
    eulerAngles.z = 0.0f;

    interactableItem.transform.eulerAngles = eulerAngles;


    Vector3 newPos = this.transform.GetChild(0).position;
    newPos.y += 0.5f; // offset
    interactableItem.transform.position = newPos;

    // set serverDoor kinematic
    //this.transform.parent.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;

    // rigidbody movement
    Rigidbody rigidbody = interactableItem.GetComponent<Rigidbody>();
    rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;


    //this.gameObject.GetComponent<ServerCoolingElement>().enabled = false;
  }

  public GameObject GetCollisionObject()
  {
    return m_coolingElement;
  }
}
