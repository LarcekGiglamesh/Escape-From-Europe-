using UnityEngine;
using System.Collections;

public enum InteractableType
{
  Item,
  Door,
  ServerDoor,
}

public class InteractableItem : MonoBehaviour
{
  [Range(10.0f, 50.0f)]
  public float m_mass = 10.0f;
  public InteractableType m_type = InteractableType.Item;

  private Rigidbody rigidbody = null;

  private bool currentlyInteracting = false;

  private WandController attachedWand = null;

  private Transform interactionPoint = null;

  private float velocityFactor = 20000f;
  private float rotationFactor = 400f;
  private Vector3 posDelta = Vector3.zero;
  Quaternion rotationDelta = Quaternion.identity;
  float angle = 0.0f;
  Vector3 axis = Vector3.zero;

  private Collider m_cameraRig = null;

  private Transform m_startTransform;

  //private bool isDoorOpen = false;
  public bool m_isDamaged = false;

  void Start()
  {
    m_startTransform = this.transform;

    rigidbody = GetComponent<Rigidbody>();
    rigidbody.mass = m_mass;

    // this is why we have physics based interaction, the mass of the object is relevant for moving/rotating
    velocityFactor /= rigidbody.mass;
    rotationFactor /= rigidbody.mass;

    m_cameraRig = GameObject.Find("[CameraRig]").GetComponent<Collider>();
    if (m_cameraRig == null)
    {
      Debug.LogError("Could not find camera rig, interaction will fail!");
    }
  }
	
	void FixedUpdate()
  {
    if (attachedWand && currentlyInteracting)
    {
      switch (m_type)
      {
        case InteractableType.Item:
          // calculate movementDelta
          posDelta = attachedWand.transform.position - interactionPoint.position;
          rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

          // calculate rotationDelta
          rotationDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
          rotationDelta.ToAngleAxis(out angle, out axis);

          // could be deleted, just that the angle fits perfeclty...
          if (angle > 180)
          {
            angle -= 360;
          }

          // move the item

          ApplyIfNotNan((Time.fixedDeltaTime * angle * axis) * rotationFactor);
          break;

        case InteractableType.Door:
          // calculate movementDelta
          posDelta = attachedWand.transform.position - interactionPoint.position;
          rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

          // calculate rotationDelta
          rotationDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
          rotationDelta.ToAngleAxis(out angle, out axis);

          // could be deleted, just that the angle fits perfeclty...
          if (angle > 180)
          {
            angle -= 360;
          }

          // move the item
          ApplyIfNotNan((Time.fixedDeltaTime * angle * axis) * rotationFactor);
          break;

        case InteractableType.ServerDoor:
          // calculate movementDelta
          posDelta = attachedWand.transform.position - interactionPoint.position;
          rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

          // calculate rotationDelta
          rotationDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
          rotationDelta.ToAngleAxis(out angle, out axis);

          // could be deleted, just that the angle fits perfeclty...
          if (angle > 180)
          {
            angle -= 360;
          }

          // move the item
          ApplyIfNotNan((Time.fixedDeltaTime * angle * axis) * rotationFactor);
          break;
      }
      
    }
	}

  private void ApplyIfNotNan(Vector3 a_vector)
  {
    Vector3 x = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
    if (float.IsNaN(x.x) || float.IsNaN(x.y) || float.IsNaN(x.z))
    {
      Debug.Log("Be careful, Physics detected NaN on float.");
    }
    else
    {
      this.rigidbody.angularVelocity = a_vector;
    }
  }

  public void BeginInteraction(WandController wand)
  {
    attachedWand = wand;
    interactionPoint = new GameObject().transform;
    interactionPoint.tag = "Untagged";
    interactionPoint.gameObject.name = "[IP] " + this.gameObject.name;
    interactionPoint.position = wand.transform.position;
    interactionPoint.rotation = wand.transform.rotation;
    interactionPoint.SetParent(transform, true);

    //CollisionObserver observer = this.gameObject.AddComponent<CollisionObserver>();
    //observer.Init(wand);


    GetComponent<Rigidbody>().isKinematic = false;

    // SERVER_DOOR SPECIAL
    if (m_type == InteractableType.ServerDoor)
    {
      GetComponent<Rigidbody>().isKinematic = false;
    }

    this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

    for (int i = 0; i < this.transform.childCount; i++)
    {
      GameObject child = this.transform.GetChild(i).gameObject;
      if (child.GetComponent<Collider>())
        Physics.IgnoreCollision(m_cameraRig, child.GetComponent<Collider>(), true);
    }
    Physics.IgnoreCollision(m_cameraRig, this.GetComponent<Collider>(), true);

    currentlyInteracting = true;
  }

  public void EndInteraction(WandController wand)
  {
    if (wand == attachedWand)
    {
      attachedWand = null;
      currentlyInteracting = false;

      // SERVER_DOOR SPECIAL
      if (m_type == InteractableType.ServerDoor)
      {
        GetComponent<Rigidbody>().isKinematic = true;
      }

      //Destroy(this.gameObject.GetComponent<CollisionObserver>());
      for (int i = 0; i < this.transform.childCount; i++)
      {
        GameObject child = this.transform.GetChild(i).gameObject;
        if (child.GetComponent<Collider>())
          Physics.IgnoreCollision(m_cameraRig, child.GetComponent<Collider>(), false);
      }

      Physics.IgnoreCollision(m_cameraRig, this.GetComponent<Collider>(), false);
      this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

      if (interactionPoint)
        if (interactionPoint.gameObject)
          Destroy(interactionPoint.gameObject);
    }
  }

  public bool IsInteracting()
  {
    return currentlyInteracting;
  }

  public WandController GetWand()
  {
    return attachedWand;
  }
}
