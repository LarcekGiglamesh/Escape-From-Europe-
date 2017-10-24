using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class NoiseSource : MonoBehaviour
{
  [Range(10.0f, 10000.0f)]
  public float m_SoundStrength = 1000.0f;
  public float m_SoundVolume = 75.0f;
  public AudioClip m_AudioClip = null;
  public GameObject m_Prefab = null;

  private const string c_EnemyObject = "EnemyParent";
  private const string c_PlayerArea = "[CameraRig]";
  private EnemyScript s_EnemyScript = null;
  private AudioSource m_AudioSource = null;

  private bool m_DestroyActive = false;
  private float m_DestroyIn = 3.5f;

  void Start()
  {
    s_EnemyScript = GameObject.Find(c_EnemyObject).GetComponent<EnemyScript>();
    m_AudioSource = GetComponent<AudioSource>();
  }

  void Update()
  {
    if (m_DestroyActive)
    {
      if(m_DestroyIn <= 0.0f)
      {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
      }
      m_DestroyIn -= Time.deltaTime;
    }
  }

  void OnTriggerEnter(Collider col)
  {
    if(col.gameObject.name == c_PlayerArea && m_DestroyActive == false)
    {
      // Debug.Log("Noise Source");
      Instantiate(m_Prefab, this.transform.parent);
      s_EnemyScript.ReactToSoundSource(this.gameObject, m_SoundStrength);
      m_AudioSource.PlayOneShot(m_AudioClip, m_SoundVolume);
      m_DestroyActive = true;
    }
  }


}
