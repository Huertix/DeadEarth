using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{

    public AIWaypointNetwork WaypointNetwork = null;
    public int CurrentIndex = 0;
    public bool HasPath = false;
    public bool PathPending = false;
    public bool PathStale = false;
    public NavMeshPathStatus PathStatus;

    private NavMeshAgent _navAgent = null;

    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();

        if (WaypointNetwork == null) return;

//        if (WaypointNetwork.Waypoints[CurrentIndex] != null)
//        {
//            _navAgent.destination = WaypointNetwork.Waypoints[CurrentIndex].position;
//        }

        SetNextDestination(false);
    }

    void SetNextDestination(bool increment)
    {
        if (!WaypointNetwork) return;

        int incStep = increment ? 1 : 0;
        Transform nextWaypointTransform = null;

//        while (nextWaypointTransform == null)
//        {
            int nextWaypoint =
                (CurrentIndex + incStep >= WaypointNetwork.Waypoints.Count) ? 0 : CurrentIndex + incStep;

            nextWaypointTransform = WaypointNetwork.Waypoints[nextWaypoint];

            if (nextWaypointTransform != null)
            {
                CurrentIndex = nextWaypoint;
                _navAgent.destination = nextWaypointTransform.position;
                return;
            }
//        }

        CurrentIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        HasPath = _navAgent.hasPath;
        PathPending = _navAgent.pathPending;
        PathStale = _navAgent.isPathStale;
        PathStatus = _navAgent.pathStatus;

        if ((!HasPath && !PathPending) || PathStatus == NavMeshPathStatus.PathInvalid)
            SetNextDestination(true);
        else
        {
            if (_navAgent.isPathStale)
                SetNextDestination(false);
        }
    }
}
