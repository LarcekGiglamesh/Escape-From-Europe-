using UnityEngine;
using System.Collections.Generic;

public class CTag : MonoBehaviour {

  public enum CTAGS
  {
    Untagged,
    Respawn,
    Finish,
    EditorOnly,
    MainCamera,
    Player,
    GameController
  }

  public List<CTAGS> m_Tag;

  /// <summary>
  /// Checks whether the given Tag exists within the List. If the List is empty, it checks the GAMEOBJECT.TAG instead.
  /// </summary>
  /// <param name="check">given Tag (String) to check</param>
  /// <returns>true if within List, else false</returns>
  public bool ccontains(CTAGS check)
  {
    bool retVal = false;
    if (m_Tag.Count == 0)
    {
      retVal = (gameObject.tag.Equals(check));
    }
    else
    {
      retVal = m_Tag.Contains(check);
    }
    return retVal;
  }

}
