using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{

  [System.Serializable]
  public enum EnemyStates
  {
    Idle,
    Hunting,
    HuntingStart,
    HuntingEnd,
    Searching,
    Screaming,
    Turning,
    Walking,
    WalkingStart,
    WalkingEnd,
    Empty,
    Killing
  }

  private struct PathFindingAssist
  {
    public List<Transform> s_Positions;
    public float s_TotalDistance;
    public bool s_TargetReached;
  }

  // GameObjects
  private const string c_StartPosition = "Waypoint_001 (20)";
  private const string c_NextPosition = "Waypoint_001 (19)";
  private const string c_LastPosition = "Waypoint_001 (20)";
  // VR Objects
  private const string c_VrCamera = "[CameraRig]";
  // Animations
  private const string c_Animation_StartWalking = "Enemy_StartWalking";
  private const string c_Animation_ExitWalking = "Enemy_ExitWalking";
  private const string c_Animation_StartRunning = "Enemy_StartRunning";
  private const string c_Animation_ExitRunning = "Enemy_ExitRunning";
  private const string c_Animation_DoDeathAnimation = "Enemy_DoDeathAnimation";

  [Tooltip("TO BE REMOVED")]
  /// Use SingletonManager instead
  public GameObject m_Enemy;
  public GameObject m_AnimatorGameObject;
  [Tooltip("TO BE REMOVED")]
  /// Use SingletonManager instead
  public GameObject m_RelativeData = null;

  private GameObject m_SoundSource = null;
  private float m_SoundSourceDistance = 0.0f;

  public Transform m_StartingPosition;    // 20
  public Transform m_NextWaypoint;        // 19
  public Transform m_LastWaypointVisited; // 20
  public float m_NextWaypointDistance;

  private Enemy_OnLoadData m_RelativeDataScript;
  public EnemyStates m_EnemyState = EnemyStates.Walking;

  [Tooltip("Defines Next Action Taken based on Number")]
  public float m_ReactionBar = 0.0f;
  [Tooltip("Counts as Factor for ReactionBar, spans from 100 to 500 (:: Factor 1.0 to 5.0)")]
  public float m_PerceptionBar = 100.0f;

  private bool m_PlayDeathAnimation = false;
  public GameObject m_WaypointRootElement = null;

  #region privateData
  private float m_UpdateRateCurrent = 5.0f;
  public float m_CurrentIdleIntervall = 0.0f;
  public float m_CurrentWalkingIntervall = 0.0f;
  #endregion

  public List<Transform> m_HuntingWaypoints;

  // Use this for initialization
  void Start()
  {
    m_RelativeDataScript = m_RelativeData.GetComponent<Enemy_OnLoadData>();

    m_StartingPosition = GameObject.Find(c_StartPosition).transform;
    m_NextWaypoint = GameObject.Find(c_NextPosition).transform;
    m_LastWaypointVisited = GameObject.Find(c_LastPosition).transform;

    m_StartingPosition.position = new Vector3(m_StartingPosition.position.x, m_Enemy.transform.position.y, m_StartingPosition.position.z);
    m_Enemy.transform.position = m_StartingPosition.position;

    if (m_EnemyState == EnemyStates.Idle)
    {
      CalculateRandomIntervalls();
    }

    if (m_EnemyState == EnemyStates.Walking)
    {
      CalculateRandomIntervalls();
      m_AnimatorGameObject.GetComponent<Animator>().SetTrigger(c_Animation_StartWalking);
    }
  }


  /// <summary>
  /// Calculates a RandomIdleIntervall and RandomWalkingIntervall if necessary
  /// </summary>
  private void CalculateRandomIntervalls()
  {
    m_CurrentIdleIntervall = m_RelativeDataScript.m_IdleDurationinSeconds + Random.Range(0, m_RelativeDataScript.m_IdleDurationRandomOffset);
    m_CurrentWalkingIntervall = m_RelativeDataScript.m_IdleExitSlowMovementDuration + Random.Range(0, m_RelativeDataScript.m_IdleExitSlowMovementOffset);
  }

  /// <summary>
  /// A Sound was created. It travels to the Enemy but gets absorbed by the Distance but factored by the current PerceptionFactor of the Enemy as well. 
  /// This will cause dynamic interpretations of the Sound and makes the Enemy more interesting. 
  /// POSSIBLE NON GOLD MASTER FEATURE
  /// </summary>
  /// <param name="a_Source">Source to send Enemy to</param>
  /// <param name="a_SoundStrength">Strength of Sound</param>
  public void ReactToSoundSource(GameObject a_Source, float a_SoundStrength)
  {
    // Use Path Finding Algorithm to find Distance between SoundSource and Enemy
    float distanceToEnemy = 123456.0f;
    // The Strength of the Sound gets consumed by Distance, 
    a_SoundStrength = Mathf.Abs(distanceToEnemy - a_SoundStrength);
    // The Strength of the Sound directly affects the Reaction Bar
    float perceptionIncrease = (m_RelativeDataScript.m_PerceptionFactor * a_SoundStrength);
    // Make sure we're positive, else the Sound was swallowed by the distance
    perceptionIncrease = Mathf.Clamp(perceptionIncrease, 0, a_SoundStrength);
    // Reaction Bar gets increased by this value
    m_ReactionBar += perceptionIncrease;
    // Maximum
    BehaviourMinMaxBars();

    // Set Data
    m_SoundSource = a_Source;
    m_SoundSourceDistance = a_SoundStrength;

    m_EnemyState = EnemyStates.Hunting;
    m_HuntingWaypoints = new List<Transform>();
    m_HuntingWaypoints = GetWaypointForSource(a_Source.transform);
  }

  /// <summary>
  /// Update Bars accordingly to:
  ///   • Reaction Bar is increased to Sound Sources only, effected by Perception Bar Value and Factor
  ///   • Reaction Bar steadily looses a fixed value
  ///   • Perception Bar is increased while IDLE only
  ///   • Perception Value gets included in Reaction Bar Value
  /// </summary>
  private void BehaviourUpdateBars()
  {
    m_ReactionBar -= m_RelativeDataScript.m_ReactionBarDecreaseRate * Time.deltaTime;

    if (m_EnemyState == EnemyStates.Idle)
    {
      m_PerceptionBar += m_RelativeDataScript.m_PerceptionIncreaseRate * Time.deltaTime * m_RelativeDataScript.m_PerceptionIdleFactor;
    }
  }

  /// <summary>
  /// Border Values for Reaction and Perception Bars
  /// </summary>
  private void BehaviourMinMaxBars()
  {
    m_ReactionBar = (m_ReactionBar > 100.0f ? 100.0f : m_ReactionBar);
    m_ReactionBar = (m_ReactionBar < 0.0f ? 0.0f : m_ReactionBar);
    m_PerceptionBar = (m_PerceptionBar > 500.0f ? 500.0f : m_PerceptionBar);
    m_PerceptionBar = (m_PerceptionBar < 100.0f ? 100.0f : m_PerceptionBar);
  }

  /// <summary>
  /// State Switch for Enemy as by definitions
  /// default:
  ///   ReactionBar above   0 and below  10:     Idle
  ///   ReactionBar above  10 and below  40:     Walking
  ///   ReactionBar above  40 and below  70:     Searching
  ///   ReactionBar above  70 and below 101:     Hunting
  /// </summary>
  /// <returns></returns>
  private EnemyStates BehaviourUpdate()
  {

    switch (m_EnemyState)
    {
      case EnemyStates.Turning: return EnemyStates.Turning;
      case EnemyStates.HuntingStart: return EnemyStates.HuntingStart;
      case EnemyStates.HuntingEnd: return EnemyStates.HuntingEnd;
      case EnemyStates.WalkingStart: return EnemyStates.WalkingStart;
      case EnemyStates.WalkingEnd: return EnemyStates.WalkingEnd;
      default: break;
    }

    // Check for Updates on Idle Timer
    if (m_EnemyState == EnemyStates.Idle) /*&& m_RelativeDataScript.m_CanIdle*/
    {
      m_CurrentIdleIntervall -= m_RelativeDataScript.m_BehaviourUpdateRate;
      if (m_CurrentIdleIntervall <= 0.25f)
      {
        // Return only when Intervall is reached
        if (m_CurrentIdleIntervall <= 0.0f)
        {
          m_CurrentIdleIntervall = 0.0f;
          return EnemyStates.Walking;
        }
        else
        {
          // Play Animation
          m_AnimatorGameObject.GetComponent<Animator>().SetTrigger(c_Animation_StartWalking);
        }
      }
      return EnemyStates.Idle;
    }

    // Check for Update on Walking Timer
    if (m_EnemyState == EnemyStates.Walking)
    {
      m_CurrentWalkingIntervall -= m_RelativeDataScript.m_BehaviourUpdateRate;
      if (m_CurrentWalkingIntervall <= 0.25f)
      {
        // Return only when Intervall is reached
        if (m_CurrentWalkingIntervall <= 0.0f)
        {
          // Reset Data to be safe
          m_CurrentWalkingIntervall = 0.0f;
          m_CurrentIdleIntervall = 0.0f;
          // Change back to Idle
          CalculateRandomIntervalls();
          return EnemyStates.Idle;
        }
        else
        {
          // Play Animation
          m_AnimatorGameObject.GetComponent<Animator>().SetTrigger(c_Animation_ExitWalking);
        }
      }
      return EnemyStates.Walking;
    }


    EnemyStates newState = EnemyStates.Empty;

    // Switch State to Idle
    if (m_ReactionBar < m_RelativeDataScript.m_MaximumValue_Idle)
    {
      newState = EnemyStates.Idle;
      m_SoundSource = null;
      m_SoundSourceDistance = float.MaxValue;
    }

    // Switch State to Slow
    if (m_ReactionBar < m_RelativeDataScript.m_MaximumValue_Walking && m_ReactionBar >= m_RelativeDataScript.m_MaximumValue_Idle)
    {
      newState = EnemyStates.Walking;
      m_SoundSource = null;
      m_SoundSourceDistance = float.MaxValue;
    }

    // Switch State to Search
    if (m_ReactionBar < m_RelativeDataScript.m_MaximumValue_Searching && m_ReactionBar >= m_RelativeDataScript.m_MaximumValue_Hunting)
    {
      newState = EnemyStates.Searching;
      m_SoundSource = null;
      m_SoundSourceDistance = float.MaxValue;
    }

    // Switch State to Hunt
    if (m_ReactionBar > m_RelativeDataScript.m_MaximumValue_Searching)
    {
      newState = EnemyStates.Hunting;
      m_SoundSource = null;
      m_SoundSourceDistance = float.MaxValue;
    }

    return newState;
  }


  /// <summary>
  /// Check the list for the first Encounter of a target Position
  /// </summary>
  /// <param name="a_Current">The Waypoint that was reached.</param>
  /// <returns>A new Waypoint</returns>
  private Transform GetAnyNearbyWaypoint(Transform a_Current)
  {
    Transform retVal = null;

    int count = m_RelativeDataScript.m_Waypoints.Length;
    for (int i = 0; i < count; i++)
    {
      if (a_Current == m_RelativeDataScript.m_Waypoints[i].Position_From)
      {
        retVal = m_RelativeDataScript.m_Waypoints[i].Position_To;
        break;
      }
    }

    return retVal;
  }


  /// <summary>
  /// Pathfinding Algorithm based on Distance Calculation
  /// </summary>
  /// <param name="a_OriginalPosition">The Last Waypoint collected.</param>
  /// <param name="a_CurrentPosition">The Next Waypoint.</param>
  /// <param name="a_TargetPosition">Goal Waypoint to reach.</param>
  /// <returns></returns>
  private PathFindingAssist PathFindingAlgorithm(Transform a_OriginalPosition, Transform a_CurrentPosition, Transform a_TargetPosition)
  {
    PathFindingAssist pfa = new PathFindingAssist();
    Debug.Log(" ============== Algorithm not yet completed ============== ");

    int emergencyCounter = 99;
    while (emergencyCounter >= 0)
    {
      emergencyCounter--;

      Transform current = a_CurrentPosition;
      List<Transform> list = GetConnectedWaypoints(current);
      // Goal reached?
      if(a_CurrentPosition == a_TargetPosition)
      {
        pfa.s_TargetReached = true;
        return pfa;
      }
      // Count == 2 >> Straight Line
      if (list.Count == 2)
      {
        Debug.Log("Straight Line found");
        current = (a_OriginalPosition == list[0] ? list[1] : list[0]);
        pfa.s_Positions.Add(current);
        pfa.s_TotalDistance += Vector3.Distance(a_OriginalPosition.position, current.position);
        continue;
      }
      // Count == 1 >> Dead End
      else if (list.Count == 1)
      {
        Debug.Log("Dead End");
        pfa.s_TotalDistance = float.MaxValue;
        return pfa;
      }
      // Count > 2 >> Crossroad
      else if(list.Count > 2)
      {
        Debug.Log("Crossroad found");
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
          if (a_OriginalPosition != list[i])
          {
            List<PathFindingAssist> pfaSubList = new List<PathFindingAssist>();
            pfaSubList.Add(PathFindingAlgorithm(a_OriginalPosition, list[i], a_TargetPosition));
          }
        }

        for (int i = 0; i < count; i++)
        {
          
        }

        continue;
      }
    }

    Debug.Log(" ============== Algorithm not yet completed ============== ");
    return pfa;
  }

  /// <summary>
  /// Checks the Waypoint List for any Neighbourers of a Current Position within the Waypoint list and returns a new List with the Neighbour Positions.
  /// </summary>
  /// <param name="a_ThisPosition">Check this Positions</param>
  /// <returns>Returns a List of Transforms of Neighbour Positions</returns>
  private List<Transform> GetConnectedWaypoints(Transform a_ThisPosition)
  {
    List<Transform> retList = new List<Transform>();

    int count = m_RelativeDataScript.m_Waypoints.Length;
    for (int i = 0; i < count; i++)
    {
      // Left Hand Check
      if (a_ThisPosition == m_RelativeDataScript.m_Waypoints[i].Position_From)
      {
        retList.Add(m_RelativeDataScript.m_Waypoints[i].Position_To);
      }
      // Right Hand Check
      if (a_ThisPosition == m_RelativeDataScript.m_Waypoints[i].Position_To)
      {
        retList.Add(m_RelativeDataScript.m_Waypoints[i].Position_From);
      }
    }

    return retList;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  private Transform FindNextWaypoint()
  {
    //todo: LookUpFunction, A* or anything else
    return GetAnyNearbyWaypoint(m_NextWaypoint);
  }


  /// <summary>
  /// Plays Death Animation
  /// </summary>
  /// <returns>True if Started, False if already completed</returns>
  public bool PlayDeathAnimation()
  {
    // Channge Enemy State
    m_EnemyState = EnemyStates.Killing;

    // Do only Once
    if (m_PlayDeathAnimation == true) { return false; }

    // play sound
    GetComponent<AudioSource>().Play();

    //// Find VR Camera Rig
    GameObject camera = GameObject.Find("Camera (eye)");


    // lookAt player
    this.transform.LookAt(camera.transform);

    // Only allow once
    m_PlayDeathAnimation = true;

    // Set as Parent
    this.gameObject.transform.SetParent(GameObject.Find("[CameraRig]").transform, true);
    //// Move forward a bit
    //Vector3 newPosition = Vector3.MoveTowards(transform.position, camera.transform.position, 4.0f);
    //// Move up a bit
    //// newPosition.y += 1.0f;
    //// Apply Positioning
    //this.gameObject.transform.position = newPosition;

    // Player Faces Enemy
    //this.transform.LookAt(camera.transform);

    // Play Animation
    //m_AnimatorGameObject.GetComponent<Animator>().SetTrigger(c_Animation_DoDeathAnimation);

    this.GetComponent<EnemyKillPlayer>().RunScript(m_AnimatorGameObject.GetComponent<Animator>(), c_Animation_DoDeathAnimation);



    //// Turn off this script
    this.GetComponent<EnemyScript>().enabled = false;

    // Return TRUE
    return true;
  }

  /// <summary>
  /// Checks if Player is nearby and IF there's nothing inbetween blocking the view
  /// </summary>
  /// <returns>True if Started, False if already completed<</returns>
  private bool LookForNearbyPlayer()
  {
    //// Do only Once
    //if (m_PlayDeathAnimation == true) { return false; }
    //// VR CAM
    //Transform vrCam = GameObject.Find("Camera (eye)").transform;
    //// Get Distance
    //float distance = Vector3.Distance(vrCam.position, this.transform.position);
    //// Check Distance
    // //Debug.Log("Distance between Player an Enemy is: " + distance);
    //if (distance <= m_RelativeDataScript.m_NearbyPlayerDetectionRange)
    //{
    //  // Direction for RaycastAll
    //  Vector3 direction = vrCam.position - this.transform.position;
    //  // Raycast to detect if there's something blocking the view
    //  RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance);
    //  // Nothing nearby means solid hit
    //  Debug.Log("Number of hits were:" + hits.Length);
      
    //  if(hits.Length == 0)
    //  {
    //    // Play Death Animation
    //    PlayDeathAnimation();
    //    return true;
    //  }
    //}
    // Nope
    return false;
  }

  /// <summary>
  /// 
  /// </summary>
  void AnimationUpdate()
  {

  }


  /// <summary>
  /// General Updates
  /// </summary>
  void Update()
  {
    AnimationUpdate();
    m_PlayDeathAnimation = LookForNearbyPlayer();

    m_UpdateRateCurrent -= Time.deltaTime;
    if (m_UpdateRateCurrent < 0.0f)
    {
      m_EnemyState = BehaviourUpdate();
      //todo: Only Do Once
      m_UpdateRateCurrent = m_RelativeDataScript.m_BehaviourUpdateRate;
    }

    BehaviourUpdateBars();
    BehaviourMinMaxBars();
  }


  /// <summary>
  /// 
  /// </summary>
  /// <param name="a_State"></param>
  /// <returns></returns>
  private float GetSpeedByBehaviour(EnemyStates a_State)
  {
    float current = m_RelativeDataScript.m_WalkingSpeed;

    switch (a_State)
    {
      case EnemyStates.Idle:
        current = 0.0f;
        break;

      case EnemyStates.Walking:
      case EnemyStates.WalkingStart:
      case EnemyStates.WalkingEnd:
        current = m_RelativeDataScript.m_WalkingSpeed;
        break;

      case EnemyStates.Hunting:
      case EnemyStates.HuntingStart:
      case EnemyStates.HuntingEnd:
        current = m_RelativeDataScript.m_RunningSpeed;
        break;
    }

    return current;
  }

  /// <summary>
  /// This will return any of the neighbours. (which might only be one)
  /// </summary>
  /// <param name="a_List">All Nearby Waypoints</param>
  /// <returns>Next Waypoint</returns>
  private Transform GetNextNeighbourPosition(List<Transform> a_List)
  {
    // We Count the Number of Neighbours
    //  1: Dead End, which was already detected before entering this function
    //  2: Straight Line, get next Waypoint
    // +3: Crossroad, choose any of the Waypoints
    int count = a_List.Count;

    // If Count is 2, then we Reached a Straight Line
    if (count == 2)
    {
      for (int i = 0; i < count; i++)
      {
        // If not Last Visited, then we head to next Waypoint
        if (a_List[i] != m_LastWaypointVisited)
        {
          // Straight Line
          // Debug.Log("Straight Line to '"+ a_List[i] .gameObject.name + "'.");
          return a_List[i];
        }
      }
    }

    // For 3 or more, we ignore the Last Visited Waypoint and choose one out of the others
    // Prepare Array for Easy Randomized Waypoint
    int newCount = count - 1;
    Transform[] rand = new Transform[newCount];
    // Since Array needs to skip a certain Element, we use an additional Indexer
    int index = 0;
    // Debug.Log(" ====================================== \nCheck '" + count + "' Neighbours.");
    // Check all Waypoints connected
    for (int i = 0; i < count; i++)
    {
      // Debug.Log("Check if '"+a_List[i].gameObject.name+"' is not equal to '"+m_NextWaypoint.gameObject.name+ "' AND '" + m_LastWaypointVisited.gameObject.name + "'.");
      if (a_List[i] != m_NextWaypoint && a_List[i] != m_LastWaypointVisited)
      {
        // Debug.Log("Add '" + a_List[i].gameObject.name + "' to the list as Element '" + index + "'.");
        rand[index] = a_List[i];
        ++index;
      }
    }
    int randId = Random.Range(0, newCount);
    // Debug.Log("Take number '" + randId + "' from List, which represents '" + rand[randId].gameObject.name + "' and return it.");
    return rand[randId];
  }

  /// <summary>
  /// Public for Testings
  /// </summary>
  public List<Transform> options;

  /// <summary>
  /// Movement Updates
  /// </summary>
  void FixedUpdate()
  {
    // Skip when Enemy is killings something (omnomnom) or Idling (zzZZzzZZzzz)
    if (m_EnemyState == EnemyStates.Killing || m_EnemyState == EnemyStates.Idle)
    {
      return;
    }

    if(m_EnemyState == EnemyStates.Walking)
    {
      m_NextWaypointDistance = Vector3.Distance(m_Enemy.transform.position, m_NextWaypoint.position);
      // Debug.Log("Distance to next is '" + m_NextWaypointDistance + "', check until distance is lower than '" + m_RelativeDataScript.m_NextWaypointDetectionRange + "'");
      if (m_NextWaypointDistance <= m_RelativeDataScript.m_NextWaypointDetectionRange)
      {
        options = GetConnectedWaypoints(m_NextWaypoint);
        // Debug.Log("The Waypoint '"+m_NextWaypoint.gameObject.name+"' has '"+options.Count+"' Neighbours.");
        if (options.Count == 1)
        {
          Transform temp = m_NextWaypoint;
          m_NextWaypoint = m_LastWaypointVisited;
          m_LastWaypointVisited = temp;
          // Debug.Log(" 1: Dead End, Next Waypoint: " + m_NextWaypoint.gameObject.name);
        }
        else
        {
          Transform temp = m_NextWaypoint;
          m_NextWaypoint = GetNextNeighbourPosition(options);
          m_LastWaypointVisited = temp;
          // Debug.Log("+2: Next Waypoint: " + m_NextWaypoint.gameObject.name);
        }
        // Debug.Log("NEW:     Last: " + m_LastWaypointVisited.gameObject.name + "       Next: " + m_NextWaypoint.gameObject.name);
      }
    }

    if(m_EnemyState == EnemyStates.Hunting)
    {
      m_NextWaypointDistance = Vector3.Distance(m_Enemy.transform.position, m_NextWaypoint.position);
      if (m_NextWaypointDistance <= m_RelativeDataScript.m_NextWaypointDetectionRange)
      {
        if(m_HuntingWaypoints.Count == 1)
        {
          // Debug.Log("Last Waypoint reached!");
          // Play Animation
          m_AnimatorGameObject.GetComponent<Animator>().SetTrigger(c_Animation_ExitRunning);

          // Reset Data to be safe
          m_CurrentWalkingIntervall = 0.0f;
          m_CurrentIdleIntervall = 0.0f;
          // Idle again
          CalculateRandomIntervalls();
          m_EnemyState = EnemyStates.Idle;
          return;
        }

        int last = m_HuntingWaypoints.Count - 1;
        m_HuntingWaypoints.RemoveAt(last);
        m_LastWaypointVisited = m_NextWaypoint;
        m_NextWaypoint = m_HuntingWaypoints[last - 1];
      }
    }

    Vector3 toTarget = m_NextWaypoint.position - m_Enemy.transform.position;
    toTarget.y = 0.0f;

    float turnRate = m_RelativeDataScript.m_TurningRate * Time.fixedDeltaTime;
    Quaternion lookRotation = Quaternion.LookRotation(toTarget);
    m_Enemy.transform.rotation = Quaternion.RotateTowards(m_Enemy.transform.rotation, lookRotation, turnRate);

    float speed = GetSpeedByBehaviour(m_EnemyState);
    m_Enemy.transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
  }



  #region GamesAcademy.Goldmaster

  /// <summary>
  /// Just a Quick Working Pathfinding, much like the old project one.
  /// </summary>
  /// <param name="a_Source">Sound Source</param>
  /// <returns>List of Waypoints for the Enemy to hunt</returns>
  private List<Transform> GetWaypointForSource(Transform a_Source)
  {
    List<Transform> list = new List<Transform>();

    float shortestDistance = float.MaxValue;
    float curDistance;
    GameObject shortestObject = null;
    foreach (Transform child in m_WaypointRootElement.transform)
    {
      curDistance = Vector3.Distance(a_Source.position, child.position);
      if(curDistance < shortestDistance)
      {
        shortestDistance = curDistance;
        shortestObject = child.gameObject;
      }
    }

    // Debug.Log("The closest Waypoint is: " + shortestObject.name);
    GameObject nextWaypoint = m_NextWaypoint.gameObject;
    list.Add(shortestObject.transform);

    List<GameObject> blocked = new List<GameObject>();
    List<Transform> waypoints = new List<Transform>();
    GameObject check = shortestObject;
    blocked.Add(check);
    int counter = 1;
    while(check != nextWaypoint)
    {
      shortestDistance = float.MaxValue;
      shortestObject = null;

      waypoints = GetConnectedWaypoints(check.transform);
      int count = waypoints.Count;
      for(int i = 0; i < count; i++)
      {
        curDistance = Vector3.Distance(m_Enemy.transform.position, waypoints[i].position);
        if (curDistance < shortestDistance && !blocked.Contains(waypoints[i].gameObject))
        {
          shortestDistance = curDistance;
          shortestObject = waypoints[i].gameObject;
        }
        blocked.Add(waypoints[i].gameObject);
      }
      // Debug.Log(counter + ": Next Shortest GameObject is: " + shortestObject.name);
      list.Add(shortestObject.transform);
      check = shortestObject;
      counter++;

      if(counter > 99)
      {
        // Debug.LogError("Warning, Cycle through 99 objects.");
        return null;
      }
    }

    int last = list.Count - 1;
    m_NextWaypoint = list[last].transform;
    m_LastWaypointVisited = this.transform;
    m_EnemyState = EnemyStates.Hunting;
    m_AnimatorGameObject.GetComponent<Animator>().SetTrigger(c_Animation_StartRunning);

    return list;
  }
  #endregion
}


/*
 * 
   private bool LookForNearbyPlayer()
  {
    // Do only Once
    if (m_PlayDeathAnimation == true) { return false; }
    // VR CAM
    Transform vrCam = GameObject.Find(c_VrCamera).transform;
    // Get Distance
    float distance = Vector3.Distance(vrCam.position, this.transform.position);
    // Check Distance
     Debug.Log("Distance between Player an Enemy is: " + distance);
    if (distance <= m_RelativeDataScript.m_NearbyPlayerDetectionRange)
    {
      // Direction for RaycastAll
      Vector3 direction = vrCam.position - this.transform.position;
      // Raycast to detect if there's something blocking the view
      RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance);
      // Nothing nearby means solid hit
       Debug.Log("Number of hits were:" + hits.Length);
      if(hits.Length == 0)
      {
        // Play Death Animation
        PlayDeathAnimation();
        return true;
      }
    }
    // Nope
    return false;
  }
 * 
 * */

/* 
   public bool PlayDeathAnimation()
{
  // Channge Enemy State
  m_EnemyState = EnemyStates.Killing;


  // Do only Once
  if (m_PlayDeathAnimation == true) { return false; }

  // play sound
  GetComponent<AudioSource>().Play();

  // Only allow once
  m_PlayDeathAnimation = true;
  // Find VR Camera Rig
  GameObject camera = GameObject.Find(c_VrCamera);
  // Set as Parent
  this.gameObject.transform.SetParent(camera.transform, true);
  // Move forward a bit
  Vector3 newPosition = Vector3.MoveTowards(transform.position, camera.transform.position, 4.0f);
  // Move up a bit
  // newPosition.y += 1.0f;
  // Apply Positioning
  this.gameObject.transform.position = newPosition;
  // Player Faces Enemy
  this.transform.LookAt(camera.transform);
  // Play Animation
  m_AnimatorGameObject.GetComponent<Animator>().SetTrigger(c_Animation_DoDeathAnimation);
  // Turn off this script
  this.GetComponent<EnemyScript>().enabled = false;
  // Return TRUE
  return true;
}
 */
