using UnityEngine;
using System.Collections;

public delegate void CallBack(GameObject a_colliding);

public class CustomCollisionDetection : MonoBehaviour
{
  private GameObject m_collidingObject = null;
  private bool m_checkForExit = false;
  private float m_lastCheckTime = 0.0f;
  private float m_checkTimeOffset = 0.5f;
  
  private string m_tag = "";
  private CallBack m_onTriggerEnter = null;
  private CallBack m_onTriggerExit = null;

  void Update()
  {
    if (m_checkForExit)
    {
      if (Time.time - m_lastCheckTime > m_checkTimeOffset)
      {
        Debug.Log("[EXIT] " + m_collidingObject);
        // callback
        m_onTriggerExit(m_collidingObject);
        m_checkForExit = false;
        m_collidingObject = null;
      }
    }
  }

  void OnTriggerStay(Collider a_other)
  {
    if (m_tag == "" || a_other.gameObject.tag == m_tag)
    {
      if (m_collidingObject == null)
      {
        Debug.Log("[ENTER] " + a_other.gameObject.name);
        // callback
        m_checkForExit = true;
        m_collidingObject = a_other.gameObject;
        m_onTriggerEnter(m_collidingObject);
      }

      m_lastCheckTime = Time.time;
    }
  }

  public void ListenOnCollision(string a_tag, CallBack a_onTriggerEnter, CallBack a_onTriggerExit)
  {
    m_tag = a_tag;
    m_onTriggerEnter = a_onTriggerEnter;
    m_onTriggerExit = a_onTriggerExit;
  }
}