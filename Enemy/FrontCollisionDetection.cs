using UnityEngine;
using System.Collections;

public class FrontCollisionDetection : MonoBehaviour
{
  private const string c_Enemy = "EnemyParent";
  private const string c_PlayerTag = "Player";
  private EnemyScript s_EnemyScript = null;

  // Use this for initialization
  void Start()
  {
    s_EnemyScript = GameObject.Find(c_Enemy).GetComponent<EnemyScript>();
  }

  /// <summary>
  /// If colliding with Player, do Death Animation
  /// </summary>
  /// <param name="a_Collider">Collider</param>
  void OnTriggerEnter(Collider a_Collider)
  {
    if (a_Collider.gameObject.tag == c_PlayerTag)
    {
      Debug.Log("Front Collision of Player! Do Death Animation!");
      s_EnemyScript.PlayDeathAnimation();
      this.GetComponent<FrontCollisionDetection>().enabled = false;
      this.transform.parent.gameObject.SetActive(false);
    }
  }

}
