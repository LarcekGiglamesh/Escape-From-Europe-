using UnityEngine;
using System.Collections;

public class DeathAnimation : MonoBehaviour
{
  public GameObject m_parent = null;
  public float m_height = 0.0f;
  public float m_distance = 2.0f;

	void Update()
  {
    float distance = Vector3.Distance(m_parent.transform.position, this.transform.position);

    //if (distance <= m_distance)
    //{
    //  // set height of enemy
    //  Vector3 pos = this.gameObject.transform.position;
    //  pos.y = m_height;
    //  this.transform.position = pos;

    //  // set parent to camera-rig
    //  //this.gameObject.transform.parent = m_parent.transform;
    //  this.gameObject.transform.SetParent(m_parent.transform, true);

    //  DoDeathAnimation();
    //}
	}

  void DoDeathAnimation()
  {
    //GetComponent<Animator>().Play("DeathAnimation");
    Debug.Log("Death...");
    this.gameObject.GetComponent<DeathAnimation>().enabled = false;
  }
}
