using UnityEngine;
using System.Collections;

public class DontCollideWithPlayer : MonoBehaviour
{
	void Start ()
  {
    //Vector3 x = Vector3.forward;
    //Vector3 y = Vector3.forward;

    //// opposite dir = -1.0f
    //// senkrecht = 0.0f;
    //// same dir = 1.0f;

    //float dotx = Vector3.Dot(x, y);

    Physics.IgnoreCollision(SingletonManager.Player.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>(), true);
	}

}
