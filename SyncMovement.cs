using UnityEngine;
using System.Collections;

public class SyncMovement : MonoBehaviour
{
  public GameObject m_syncObject = null;
  private float min;
  public float max;

  private Vector3 m_lastPosition = Vector3.zero;

  private float m_movementDone = 0.0f;

  void Start()
  {
    m_lastPosition = this.transform.position;
    min = m_lastPosition.y;
    max = min - 0.4f;
  }

  void FixedUpdate()
  {
    Vector3 delta = this.transform.position - m_lastPosition;

    m_movementDone += delta.z;
    Vector3 pos = m_syncObject.transform.localPosition + new Vector3(0.0f,  delta.z, 0.0f);
    pos.y = Mathf.Clamp(pos.y, min, max);
    m_syncObject.transform.localPosition = pos;

    m_lastPosition = this.transform.position;
	}
}
