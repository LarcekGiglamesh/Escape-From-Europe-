using UnityEngine;
using System.Collections;

public class CameraRigFixedHeight : MonoBehaviour
{
  public GameObject m_follow = null;

  void Start()
  {
    Physics.IgnoreLayerCollision(LayerMask.NameToLayer("CameraRig"), LayerMask.NameToLayer("Default"), true);
    //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("CameraRig"), LayerMask.NameToLayer("InteractableObject"), true);
  }

  void Update()
  {
    Vector3 diff = this.transform.position - m_follow.transform.position;
    Debug.Log(diff);
    
    //this.GetComponent<SphereCollider>().center = new Vector3(diff.x, this.GetComponent<SphereCollider>().center.y, diff.y);
  }
}
