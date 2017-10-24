using UnityEngine;
using System.Collections;

public class DontCollideWithAnything : MonoBehaviour
{
	void Start ()
  {
    //Physics.IgnoreLayerCollision(1 << LayerMask.NameToLayer("Door"), Physics.AllLayers);
  }
}
