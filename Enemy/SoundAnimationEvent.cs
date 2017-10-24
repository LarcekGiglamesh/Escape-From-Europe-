using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class SoundAnimationEvent : MonoBehaviour
{

  private AudioSource m_AudioSource = null;
  public AudioClip[] m_AudioClips = null;
  private EnemyScript s_EnemyScript = null;

  private float m_CurrentTime = 0.0f;
  private float m_MaxTime = 5.06f;
  private int m_Indexer = 0;

  private List<int> m_Indize;

  void Start()
  {
    m_AudioSource = GetComponent<AudioSource>();
    s_EnemyScript = this.transform.parent.GetComponent<EnemyScript>();

    prepareData2();
  }

  private void prepareData2()
  {
    m_Indize = new List<int>();
    int count = m_AudioClips.Length;
    for (int i = 0; i < count; i++)
    {
      m_Indize.Add(i);
    }
  }

  /// <summary>
  /// Play Sound on given Step Frame
  /// </summary>
  /// <param name="a_Id"></param>
  public void playSoundRun(int a_Id)
  {
    // Only when Walking or Hunting
    if (s_EnemyScript.m_EnemyState == EnemyScript.EnemyStates.Walking || s_EnemyScript.m_EnemyState == EnemyScript.EnemyStates.Hunting)
    {
      // A List was prepared with all possible Indize
      // i copy that list
      List<int> randomizer = new List<int>(m_Indize);
      // then i get one specific element from that list
      int reduced = (a_Id - 1) % (m_AudioClips.Length);
      // and remove it, to get a list with all indize but the one in a_Id
      randomizer.Remove(reduced);
      // now i simply count
      int counter = randomizer.Count;
      // and get a random number from that
      int index = Random.Range(0, counter);

      // Debug.Log("Play Sound '"+ m_AudioClips[index].name + "' since Enemy is walking and triggered Event.");
      // Now we're playing a Random Step Sound though this makes it impossible to get 'a_Id' played here.
      //m_AudioSource.PlayOneShot(m_AudioClips[index], 0.75f);
      m_AudioSource.clip = m_AudioClips[index];
      m_AudioSource.Play();
    }
  }
  
}
