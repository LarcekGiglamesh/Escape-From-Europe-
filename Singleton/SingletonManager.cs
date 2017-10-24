using UnityEngine;
using System.Collections;

public class SingletonManager : MonoBehaviour
{
  private static GameObject m_player = null;
  private static SteamVRManager m_steamVRManager = null;

  public static SteamVRManager SteamVRManager
  {
    get
    {
      if (m_steamVRManager == null)
      {
        m_steamVRManager = SteamVRManager.GetInstance();
      }
      return m_steamVRManager;
    }
  }

  public static GameObject Player
  {
    get
    {
      if (m_player == null)
      {
        m_player = GameObject.Find("[CameraRig]");
      }
      return m_player;
    }
  }
}


//private static GameObject m_player = null;
//private static GameObject m_enemy = null;

//private static SteamVR.Device
/*private static MouseManager m_mouseManager = null;
private static GameManager m_gameManager = null;
private static MainTerminalController m_mainTerminalController = null;
private static UIManager m_uiManager = null;
private static AudioManager m_audioManager = null;
private static Minimap m_minimap = null;
private static GrayScaleManager m_grayScaleManager = null;

private static XmlSave m_xmlSave = null;
private static BGMixer m_bgmixer = null;
private static TextToSpeech m_textToSpeech = null;
private static BedFix m_bedFix = null;
private static Tutorial m_tutorial = null;
private static EnemyFeedback m_enemyFeedback = null;

public static MouseManager MouseManager
{
  get
  {
    if (m_mouseManager == null)
    {
      m_mouseManager = MouseManager.GetInstance();
    }
    return m_mouseManager;
  }
}

public static GameManager GameManager
{
  get
  {
    if (m_gameManager == null)
    {
      Debug.Log("Getting GameManager");
      m_gameManager = GameManager.GetInstance();
    }
    return m_gameManager;
  }
}

public static MainTerminalController MainTerminalController
{
  get
  {
    if (m_mainTerminalController == null)
    {
      m_mainTerminalController = MainTerminalController.GetInstance();
    }
    return m_mainTerminalController;
  }
}

public static UIManager UIManager
{
  get
  {
    if (m_uiManager == null)
    {
      m_uiManager = UIManager.GetInstance();
    }
    return m_uiManager;
  }
}

public static GameObject Player
{
  get
  {
    if (m_player == null)
    {
      m_player = GameObject.Find(StringManager.Names.player);
    }

    return m_player;
  }
}

public static AudioManager AudioManager
{
  get
  {
    if (m_audioManager == null)
    {
      m_audioManager = AudioManager.GetInstance();
    }
    return m_audioManager;
  }
}

public static Minimap Minimap
{
  get
  {
    if (m_minimap == null)
    {
      m_minimap = Minimap.GetInstance();
    }
    return m_minimap;
  }
}

public static GrayScaleManager GrayScaleManager
{
  get
  {
    if (m_grayScaleManager == null)
    {
      m_grayScaleManager = GrayScaleManager.GetInstance();
    }
    return m_grayScaleManager;
  }
}

public static GameObject Enemy
{
  get
  {
    if (m_enemy == null)
    {
      m_enemy = EnemyAiScript.GetInstance();
    }

    return m_enemy;
  }
}

public static XmlSave XmlSave
{
  get
  {
    if (m_xmlSave == null)
    {
      m_xmlSave = XmlSave.GetInstance();
    }

    return m_xmlSave;
  }
}

public static BGMixer BGMixer
{
  get
  {
    if (m_bgmixer == null)
    {
      m_bgmixer = BGMixer.GetInstance();
    }

    return m_bgmixer;
  }
}

public static TextToSpeech TextToSpeech
{
  get
  {
    if (m_textToSpeech == null)
    {
      m_textToSpeech = TextToSpeech.GetInstance();
    }

    return m_textToSpeech;
  }
}

public static BedFix BedFix
{
  get
  {
    if (m_bedFix == null)
    {
      m_bedFix = BedFix.GetInstance();
    }

    return m_bedFix;
  }
}

public static Tutorial Tutorial
{
  get
  {
    if (m_tutorial == null)
    {
      m_tutorial = Tutorial.GetInstance();
    }

    return m_tutorial;
  }
}

public static EnemyFeedback EnemyFeedback
{
  get
  {
    if (m_enemyFeedback == null)
    {
      m_enemyFeedback = EnemyFeedback.GetInstance();
    }

    return m_enemyFeedback;
  }
}*/
