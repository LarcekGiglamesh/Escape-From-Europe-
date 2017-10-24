using UnityEngine;
using System.Collections;

public enum EnemyKillPlayerState
{
  MoveToPlayer,
  Kill
}

public class EnemyKillPlayer : MonoBehaviour
{
  public float m_moveToPlayerDistance = 0.5f; // distance to player
  //private EnemyKillPlayerState m_state = EnemyKillPlayerState.MoveToPlayer;
  private bool m_started = false;

  private GameObject m_playerCamera = null;
  private Vector3 m_directionXZ = Vector3.zero;
  private string m_animationName = null;
  private Animator m_animator = null;
  public GameObject m_deathSoundObject = null;
  public GameObject m_end = null;
  private bool m_endPort = false;
  public float m_endTime = 2.0f;
  public Light m_controllerLight;

	void Update ()
  {
	  if (m_started)
    {
      if (Vector3.Distance(this.transform.position, m_playerCamera.transform.position) > m_moveToPlayerDistance)
      {
        this.transform.Translate(m_directionXZ * Time.deltaTime, Space.World);
      }
      else
      {
        // distance reached
        m_animator.SetTrigger(m_animationName);
        m_deathSoundObject.GetComponent<AudioSource>().Play();
        //this.GetComponent<EnemyKillPlayer>().enabled = false;
        m_endPort = true;
        Debug.Log("KILLING PLAYER");
      }
    }

    if (m_endPort)
    {
      if (m_endTime > 0.0f)
      {
        m_endTime -= Time.deltaTime;

        if (m_endTime < 0.0f)
        {
          m_endTime = 0.0f;
          Debug.Log("TELEPORTED PLAYER AWAY");
          GameObject.Find("EnemyParent").SetActive(false);
          m_controllerLight.enabled = false;
          GameObject.Find("[CameraRig]").transform.position = m_end.transform.position;
          this.GetComponent<EnemyKillPlayer>().enabled = false;
          Time.timeScale = 0.0f;
        }
      }
    }
  }

  public void RunScript(Animator a_animator, string a_animationName)
  {
    m_started = true;
    m_playerCamera = GameObject.Find("Camera (eye)");
    m_directionXZ = m_playerCamera.transform.position - this.transform.position;
    m_directionXZ.y = 0.0f;
    m_animationName = a_animationName;
    m_animator = a_animator;
  }

}
