using UnityEngine;
using System.Collections;

public class ServerDoor : MonoBehaviour
{
  public float m_distance = 0.4f;

  private float min;
  private float max;

  private Vector3 m_lastPosition = Vector3.zero;

  private float m_movementDone = 0.0f;

  void Start()
  {
    m_lastPosition = this.transform.localPosition;
    min = m_lastPosition.z;
    max = min + m_distance;

    Physics.IgnoreLayerCollision(0, LayerMask.NameToLayer(StringManager.Layer.ServerDoorEnd), true);
    Physics.IgnoreLayerCollision(LayerMask.NameToLayer(StringManager.Layer.ServerDoor), LayerMask.NameToLayer(StringManager.Layer.ServerDoorEnd), false);
  }

  void FixedUpdate()
  {
    //Vector3 pos = this.transform.localPosition;
    //pos.z = Mathf.Clamp(this.transform.localPosition.z, min, max);
    //this.transform.localPosition = pos;

    //m_lastPosition = this.transform.localPosition;
    

    //Physics.IgnoreLayerCollision(Physics.AllLayers, LayerMask.NameToLayer("ServerDoorEnd"), true);
    //Physics.IgnoreLayerCollision(1 << LayerMask.NameToLayer("ServerDoor"), 1 << LayerMask.NameToLayer("ServerDoorEnd"), false);
  }
}
