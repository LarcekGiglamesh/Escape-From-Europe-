using UnityEngine;
using System.Collections;

public class ShelfDoor : MonoBehaviour
{
	void Start ()
  {
    Physics.IgnoreLayerCollision(0, LayerMask.NameToLayer(StringManager.Layer.ServerDoorEnd), true);
    Physics.IgnoreLayerCollision(LayerMask.NameToLayer(StringManager.Layer.ServerDoor), LayerMask.NameToLayer(StringManager.Layer.ServerDoorEnd), false);
  }
}
