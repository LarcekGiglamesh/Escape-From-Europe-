using UnityEngine;
using System.Collections;

public interface IServerNotebook
{
  void ChangeState(Server a_server);
  void BootSystem();
}
