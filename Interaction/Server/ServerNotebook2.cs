using UnityEngine;
using System.Collections;

public class ServerNotebook2 : MonoBehaviour, IServerNotebook
{
  public Material m_mat1;
  public Material m_mat2;
  public Material m_mat3;
  public Material m_mat4;

  private Server m_server = null;

  private bool m_isBooted = false;

  public void ChangeState(Server a_server)
  {
    if (m_isBooted)
    {
      return;
    }

    if (a_server.IsActivated())
    {
      ChangeMaterial(m_mat3);
    }
    else if (!a_server.IsInserted(ServerSlotType.cpu))
    {
      // 1
      ChangeMaterial(m_mat1);
    }
    else if (a_server.IsInserted(ServerSlotType.cpu))
    {
      // 2
      ChangeMaterial(m_mat2);
    }

    m_server = a_server;
  }

  public Server GetServer()
  {
    return m_server;
  }

  public void BootSystem()
  {
    if (m_isBooted)
    {
      return;
    }

    if (m_server && m_server.IsActivated())
    {
      ChangeMaterial(m_mat4);
      Debug.Log("BOOTING THE SYSTEM SUCCESS");
      // PLAY ESCAPE SOUND....
      m_isBooted = true;
    }
  }

  private void ChangeMaterial(Material a_mat)
  {
    this.GetComponent<Renderer>().material = a_mat;
  }
}
