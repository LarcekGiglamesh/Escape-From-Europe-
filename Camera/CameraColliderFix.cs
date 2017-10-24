using UnityEngine;
using System.Collections;

public class CameraColliderFix : MonoBehaviour
{
  public float m_heightOffset = 0.2f;
  public float m_playAreaHeightAdjustment = 0.009f;
  private Transform m_headset;
  private CapsuleCollider m_collider;

	void Start ()
  {
    m_headset = FindObjectOfType<SteamVR_Camera>().transform;
    m_collider = GetComponent<CapsuleCollider>();
  }

   void Update()
  {
    var newpresenceColliderYSize = (m_headset.transform.localPosition.y - m_heightOffset);
    var newpresenceColliderYCenter = (newpresenceColliderYSize != 0 ? (newpresenceColliderYSize / 2) + m_playAreaHeightAdjustment : 0);

    if (m_collider)
    {
      m_collider.height = newpresenceColliderYSize;
      m_collider.center = new Vector3(m_headset.localPosition.x, newpresenceColliderYCenter, m_headset.localPosition.z);
    }
  }
}
