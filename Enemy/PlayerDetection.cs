using UnityEngine;
using System.Collections;

public class PlayerDetection : MonoBehaviour
{
  public float m_dotAngle = 0.75f;
  private bool m_hasDetectedPlayer = false;
  public GameObject m_controllerLeft;
  public GameObject m_controllerRight;
  public GameObject m_cameraRig;
  public float m_speed = 1.0f;

  void LateUpdate()
  {
    //if (m_hasDetectedPlayer)
    //{
    //  // rotate player
    //  Vector3 targetDir = m_cameraRig.transform.position - transform.position;
    //  float step = m_speed * Time.deltaTime;

    //  Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);

    //  m_cameraRig.transform.rotation = Quaternion.LookRotation(newDir);
    //  Debug.Log("....");
    //  //this.transform.parent.GetComponent<EnemyScript>().PlayDeathAnimation();
    //}
  }

  void OnTriggerEnter(Collider a_other)
  {
    if (m_hasDetectedPlayer)
    {
      return;
    }

    if (a_other.gameObject.name == "[CameraRig]")
    {
      // Direction for RaycastAll
      Transform vrCam = GameObject.Find("Camera (eye)").transform;

      //Vector3 offsetPos = vrCam.position;
      //offsetPos.y -= m_offsetYScan;
      Vector3 direction = vrCam.position - this.transform.position;

      Debug.DrawRay(this.transform.position, this.transform.forward * 100.0f, Color.green);

      // check angle
      //float dotAngle = Vector3.Dot(direction.normalized, this.transform.forward);

      //if (dotAngle < m_dotAngle)
      //{
      //  return;
      //}

      // Raycast to detect if there's something blocking the view
      RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, direction.magnitude);

      Debug.DrawRay(this.transform.position, direction);

      bool isObjectBlocking = false;

      foreach (RaycastHit hit in hits)
      {
        if (!hit.collider.gameObject.GetComponent<InteractableItem>() || hit.collider.gameObject.tag == StringManager.Tags.Controller)
        {
          Debug.Log("Blocking Object found: " + hit.collider.gameObject.name);
          isObjectBlocking = true;
        }
        else
        {
          Debug.Log("Non-Blockig object found: " + hit.collider.gameObject.name);
        }
      }

      // NEXT HIT CHECK WITH LOWERED CAM

      //Vector3 vrCamYModified = vrCam.position;
      //vrCamYModified.y -= 0.5f;
      //direction = vrCamYModified - this.transform.position;

      //RaycastHit[] hits1 = Physics.RaycastAll(transform.position, direction, direction.magnitude);

      //Debug.DrawRay(this.transform.position, direction);

      //foreach (RaycastHit hit in hits1)
      //{
      //  if (!hit.collider.gameObject.GetComponent<InteractableItem>() || hit.collider.gameObject.tag == StringManager.Tags.Controller)
      //  {
      //    Debug.Log("Blocking Object found: " + hit.collider.gameObject.name);
      //    isObjectBlocking = true;
      //  }
      //  else
      //  {
      //    Debug.Log("Non-Blockig object found: " + hit.collider.gameObject.name);
      //  }
      //}



      if (!isObjectBlocking)
      {
        // controller disable
        m_cameraRig.GetComponent<PlayerMovement_Sebastian>().enabled = false;
        this.transform.parent.GetComponent<EnemyScript>().PlayDeathAnimation();
        Debug.Log("DeathANimation can be played now");
        m_hasDetectedPlayer = true;
      }     
    }
  }
}
