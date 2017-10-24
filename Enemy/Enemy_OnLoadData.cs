using UnityEngine;
using System.Collections.Generic;

public class Enemy_OnLoadData : MonoBehaviour
{

  [System.Serializable]
  public struct WaypointData
  {
    public Transform Position_From;
    public Transform Position_To;
    public float DistanceAbs;
  }

  public WaypointData[] m_Waypoints;

  public float m_WalkingSpeed = 4.0f;
  [Range(0.1f, 2.0f)]
  public float m_WalkingAnimationSpeed = 1.0f;

  public float m_SlowWalkingSpeed = 2.0f;
  [Range(0.1f, 2.0f)]
  public float m_SlowWalkingAnimationSpeed = 1.0f;

  public float m_RunningSpeed = 8.0f;
  [Range(0.1f, 2.0f)]
  public float m_RunningAnimationSpeed = 1.0f;

  public float m_TurningRate = 4.0f;
  [Range(0.1f, 2.0f)]
  public float m_TurningAnimationSpeed = 1.0f;

  public bool m_CanIdle = true;
  [Tooltip("If the Distance between Enemy and next Waypoint reaches below this value, a new Waypoint gets choosen.")]
  public float m_NextWaypointDetectionRange = 5.0f;
  [Tooltip("Update Rate for Behaviour")]
  public float m_BehaviourUpdateRate = 0.25f;

  // public float m_ReactionBarMaximum = 100.0f; // outdated
  [Tooltip("Reduction Rate per Second")]
  public float m_ReactionBarDecreaseRate = 1.0f;
  public float m_ReactionFactor = 1.0f;

  // public float m_PerceptionBarMaximum = 100.0f; // outdated
  [Tooltip("Reduction Rate per Second")]
  public float m_PerceptionIncreaseRate = 1.0f;
  public float m_PerceptionFactor = 0.15f;
  public float m_PerceptionIdleFactor = 3.0f;

  public float m_MaximumValue_Idle = 10.0f;
  public float m_MaximumValue_Walking = 40.0f;
  public float m_MaximumValue_Searching = 70.0f;
  public float m_MaximumValue_Hunting = 100.0f;

  public float m_MinTurnDistance = 10.0f;
  public float m_NearbyPlayerDetectionRange = 10.0f;

  [Tooltip("If the Enemy switches to Idle, he will wait this long +/- RandomOffset below.")]
  public float m_IdleDurationinSeconds = 8.0f;
  public float m_IdleDurationRandomOffset = 4.0f;
  [Tooltip("If the Enemy idled long enough, he will switch to slow movement this long +/- RandomOffset below.")]
  public float m_IdleExitSlowMovementDuration = 15.0f;
  public float m_IdleExitSlowMovementOffset = 10.0f;

  // public Animator m_AnimationController = null;

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  private WaypointData[] CreateWaypointList()
  {
    List<WaypointData> list = new List<WaypointData>();



    // List to Array
    int listCount = list.Count;
    WaypointData[] array = new WaypointData[listCount];
    for (int i = 0; i < listCount; i++)
    {
      array[i].Position_From = list[i].Position_From;
      array[i].Position_To = list[i].Position_To;
      array[i].DistanceAbs = list[i].DistanceAbs;
    }

    return array;
  }

  void Start()
  {
    int count = m_Waypoints.Length;
    for (int i = 0; i < count; i++)
    {
      m_Waypoints[i].DistanceAbs = 0.0f;
      m_Waypoints[i].DistanceAbs = Vector3.Distance(m_Waypoints[i].Position_From.position, m_Waypoints[i].Position_To.position);
    }

    // Value need to be PUBLIC to be visible but needs to stay 100% as well.
    m_MaximumValue_Hunting = 100.0f;
  }

}
