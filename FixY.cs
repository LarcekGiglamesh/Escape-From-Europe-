using UnityEngine;
using System.Collections;

public class FixY : MonoBehaviour
{

  
  // Update is called once per frame
  void LateUpdate()
  {
    //hmdPos = HMD.transform.position;                         Can't work in world position
    //transform.position = InitialPos - hmdPos;                for some reason
    Vector3 pos = this.transform.localPosition;
    pos.y = 0.0f;
    this.transform.localPosition = pos;
  }

}