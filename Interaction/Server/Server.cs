using UnityEngine;
using System.Collections;

//public enum ServerAcceptCartridgeType
//{
//  One,
//  Two,
//  Three,
//  Four,
//  Five,
//  Six,
//  Seven,
//  Eigth,
//  Nine,
//  Ten,
//}

public enum ServerSlotState
{
  Insert,
  Remove
}

public enum ServerSlotType
{
  // each state is a bit-field
  none = 0x0, // 0000
  hdd = 0x1, // 0001
  cpu = 0x2, // 0010
  coolingLeft = 0x4, // 0100 
  coolingRight = 0x8 // 1000
}

public class Server : MonoBehaviour
{
  //public ServerAcceptCartridgeType m_cartridgeAcceptType = ServerAcceptCartridgeType.One;
  public uint m_totalSlots = 1; // maybe the server has several slots which need to be activated
  public uint m_currentActivatedSlots = 0; // for every slot that will be activated, this will be increased

  public AudioClip m_insert;
  public AudioClip m_remove;
  public AudioClip m_activated;

  public GameObject m_notebookScreen;

  ServerSlotType m_currentInsertedTypes = ServerSlotType.none;

  public bool IsActivated()
  {
    // total slots reached? ==> activated
    if (m_currentActivatedSlots >= m_totalSlots)
    {
      m_currentActivatedSlots = m_totalSlots;
      return true;
    }

    return false;
  }

  public void UpdateServerSlot(ServerSlotState a_state, ServerSlotType a_type)
  {
    switch (a_state)
    {
      case ServerSlotState.Insert:
        m_currentActivatedSlots++;
        Insert(a_type);
        if (IsActivated())
        {
          // ALL SLOTS ENABLED
          Debug.Log("Server [" + gameObject.name + "] activated.");
          GetComponent<AudioSource>().Play(); // ENABLE SOUND
        }
        break;

      case ServerSlotState.Remove:
        Remove(a_type);
        if (IsActivated())
        {
          // server is currently activated, but object is pulled off
          // server will be deactivated
          Debug.Log("Server [" + gameObject.name + "] deactivated.");
          GetComponent<AudioSource>().Play(); // DISABLE SOUND
        }
        m_currentActivatedSlots--;
        break;
    }

    // update info screen
    m_notebookScreen.GetComponent<IServerNotebook>().ChangeState(this);
  }

  public void Insert(ServerSlotType a_type)
  {
    m_currentInsertedTypes |= a_type;
  }

  public void Remove(ServerSlotType a_type)
  {
    m_currentInsertedTypes ^= a_type;
  }

  public bool IsInserted(ServerSlotType a_type)
  {
    ServerSlotType tmp = m_currentInsertedTypes & a_type;

    // if tmp is the same as a_value the bit is set
    return tmp == a_type;
  }

  public void PlaySound(ServerSlotState a_state)
  {

    AudioSource s = GetComponent<AudioSource>();

    switch (a_state)
    {
      case ServerSlotState.Insert:
        if (IsActivated())
        {
          s.clip = m_activated;
        }
        else
        {
          s.clip = m_insert;
        }
          s.Play();
        break;
      case ServerSlotState.Remove:
        s.clip = m_remove;
        s.Play();
        break;
      
    }
  }
}
