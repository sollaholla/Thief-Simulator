using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum AIType
{
    Ped,
    Cop
}

public enum AIState
{
    AlertingCop,
    Chasing,
    Patrolling
}

[RequireComponent(typeof(CharacterMover))]
[RequireComponent(typeof(RoomHandler))]
public class AI : MonoBehaviour
{
    public AIType aiType = AIType.Ped;
    public NavMeshAgent navAgent;
    public float minWaitTime;
    public float maxWaitTime;
    public Transform waypointSet;
    public float visionRange = 10f;
    public float visionAngle = 60f;
    public float maxDetectTime = 0.5f;
    public float investigationDelay = 1f;
    public float investigationDuration = 4f;
    public bool investigating;

    private CharacterMover cc;
    private GameObject player;
    private int waypointIndex;

    private float detectTime;
    public AIState state = AIState.Patrolling;

    private Waypoint[] waypoints;
    private GameObject[] cops;

    private RoomHandler roomHandler;
    private RoomHandler playerRoomHandler;
    private PickupHandler playerPickupHandler;
    private Distraction currentDistraction;

    private void Awake()
    {
        cc = GetComponent<CharacterMover>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerRoomHandler = player.GetComponent<RoomHandler>();
        playerPickupHandler = player.GetComponent<PickupHandler>();
        roomHandler = GetComponent<RoomHandler>();
        detectTime = maxDetectTime;
    }

    private void InitWaypoints()
    {
        if (waypointSet != null)
        {
            waypoints = new Waypoint[waypointSet.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                var t = waypointSet.GetChild(i);
                var wait = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
                waypoints[i] = new Waypoint { point = t, waitTime = wait };
            }

            var targetWaypoint = waypoints.OrderBy(x => Vector3.Distance(transform.position, x.point.position)).First();
            waypointIndex = Array.IndexOf(waypoints, targetWaypoint);
        }
    }

    private void Start()
    {
        InitWaypoints();
        BeginPatrol();
        navAgent.SetDestination(transform.position);
    }

    private void BeginPatrol()
    {
        if (waypoints != null)
        {
            state = AIState.Patrolling;
            StartCoroutine(Patrol());
        }
    }

    void FixedUpdate()
    {
        DetectRobber();
        navAgent.transform.position = transform.position;
    }

    private void DetectRobber()
    {
        if ((roomHandler.room == -1 || playerRoomHandler.room != roomHandler.room) && !playerPickupHandler.hasItem)
        {
            return;
        }

        if (playerPickupHandler.hasItem && roomHandler.room != playerRoomHandler.room)
        {
            return;
        }

        if (state == AIState.AlertingCop)
        {
            return;
        }

        var distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < visionRange)
        {
            var direction = (player.transform.position - transform.position).normalized;
            var angle = Vector3.Angle(transform.forward, direction);
            if (angle < visionAngle)
            {
                if (detectTime > 0)
                {
                    detectTime -= Time.deltaTime;
                }
                else
                {
                    if (aiType == AIType.Ped)
                    {
                        detectTime = maxDetectTime;
                        StopAllCoroutines();
                        StartCoroutine(AlertCop());
                        state = AIState.AlertingCop;
                    }
                    else
                    {
                        Debug.Log("GAME OVER");
                        StopAllCoroutines();
                    }
                }
            }
        }
    }

    public void GoInvestigate(Distraction distraction, bool run)
    {
        if (state != AIState.Patrolling)
        {
            return;
        }

        if (currentDistraction == distraction)
        {
            return;
        }

        currentDistraction = distraction;
        StopAllCoroutines();
        StartCoroutine(Investigate(distraction, run));
    }

    private IEnumerator Investigate(Distraction distraction, bool run)
    {
        investigating = true;
        cc.Move(0, 0, false, false);
        yield return new WaitForSeconds(investigationDelay);
        navAgent.SetDestination(distraction.transform.position);

        var time = investigationDuration;
        while (true)
        {
            OnNavigate(run);

            if (navAgent.remainingDistance < navAgent.stoppingDistance)
            {
                if (time > 0)
                {
                    time -= Time.deltaTime;
                }
                else
                {
                    cc.Move(0, 0, false, false);
                    yield return new WaitForSeconds(investigationDuration);
                    distraction.Stop();
                    currentDistraction = null;
                    BeginPatrol();
                    investigating = false;
                    break;
                }
            }

            yield return null;
        }
    }

    private IEnumerator AlertCop()
    {
        var lkl = player.transform.position;
        cops = GameObject.FindGameObjectsWithTag("Cop");

        while (true)
        {
            GameObject closest = null;
            float minDistance = Mathf.Infinity;
            foreach (var cop in cops)
            {
                var distance = Vector3.Distance(cop.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = cop;
                }
            }

            if (closest != null)
            {
                navAgent.SetDestination(closest.transform.position);

                if (navAgent.remainingDistance < navAgent.stoppingDistance)
                {
                    BeginPatrol();

                    var copAI = closest.GetComponent<AI>();
                    if (copAI != null)
                    {
                        var distraction = new GameObject("PlayerDistraction").AddComponent<Distraction>();
                        distraction.distractionDelay = 0;
                        distraction.transform.position = lkl;
                        copAI.GoInvestigate(distraction, true);
                    }
                    break;
                }
            }

            OnNavigate(true);

            yield return null;
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            navAgent.SetDestination(waypoints[waypointIndex].point.position);

            while (true)
            {
                OnNavigate(false);

                if (navAgent.remainingDistance < navAgent.stoppingDistance)
                {
                    cc.Move(0, 0, false, false);
                    yield return new WaitForSeconds(waypoints[waypointIndex].waitTime);
                    break;
                }

                yield return null;
            }

            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            yield return null;
        }
    }

    private void OnNavigate(bool run)
    {
        var moveDirection = new Vector3(navAgent.desiredVelocity.x, 0, navAgent.desiredVelocity.z).normalized;
        cc.Move(moveDirection.x, moveDirection.z, run, false);

        if (moveDirection.sqrMagnitude > 0.25f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.fixedDeltaTime * 5f);
        }
    }
}

[System.Serializable]
public class Waypoint
{
    public Transform point;
    public float waitTime;
}
