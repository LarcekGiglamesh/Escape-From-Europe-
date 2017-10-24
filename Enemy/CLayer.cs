using UnityEngine;
using System.Collections.Generic;

public class CLayer : MonoBehaviour {

  public List<LayerMask> m_Layer;

  /// <summary>
  /// Checks whether the given LayerMask exists within the List. If the List is empty, it checks the GAMEOBJECT.LAYER instead.
  /// </summary>
  /// <param name="check">given LayerMask to check</param>
  /// <returns>true if within List, else false</returns>
  public bool ccontains(LayerMask check)
  {
    bool retVal = false;
    if(m_Layer.Count == 0)
    {
      retVal = (gameObject.layer == check);
    }else
    {
      retVal = m_Layer.Contains(check);
    }
    return retVal;
  }

}
