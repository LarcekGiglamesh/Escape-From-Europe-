using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
  public float m_minHeight = 2.0f;
  public Transform m_moveTo;
  public float m_speed = 0.05f;
  private Vector3 m_startPosition;

  private bool m_startMoving = false;

	void Start ()
  {
    m_startPosition = this.gameObject.transform.position;
	}
	
	void Update ()
  {
	  if (!m_startMoving && this.gameObject.transform.position.y - m_startPosition.y >= m_minHeight)
    {
      Debug.Log("Starting to move door.");
      m_startMoving = true;
      this.GetComponent<InteractableItem>().enabled = false;
      this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }


    if (m_startMoving)
    {
      //Vector3.MoveTowards(this.transform.position, m_moveTo.position, Time.deltaTime * 100.0f);
      Vector3 delta = Vector3.Lerp(this.transform.position, m_moveTo.position, Time.deltaTime);
      delta.x = 0.0f;
      delta.y = 0.0f;
      delta.z = -delta.z;
      this.transform.Translate(delta * m_speed);
      
    }

    float target = m_moveTo.position.y + transform.parent.transform.position.y;
    if (this.gameObject.transform.position.y >= target)
    {
      Debug.Log("Door moving done.");
      
      this.GetComponent<Door>().enabled = false;
      
    }
	}
}
