using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Linq;

public class EnemyPatrol : MonoBehaviour
{
    #region Enemy pathing
    [SerializeField]
    private Transform[] _waypointCollectionPerObjective; // Drag Waypoints into inspector if we make a longer path

    private Transform[][] _waypointsPerObjective;


    private NavMeshAgent _agent;  // NavMesh and NavAgent in place so it doesnt walk through walls


    private int _currentObjectiveWaypointCollection = 0;
    private int _currentWaypoint = 0;

    private float _originalSpeedThieves;

    private bool _isPoliceWaypointSet;
    #endregion

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        //+2 because of the 0 objective and police objective
        _waypointsPerObjective = new Transform[General_Game.ObjectivesCount +2][];

        for (int i = 0; i <= _waypointCollectionPerObjective.Length - 1; i++)
        {
            int amountOfWaypoints = _waypointCollectionPerObjective[i].transform.childCount;

            _waypointsPerObjective[i] = new Transform[amountOfWaypoints];

            for (int j = 0; j <= amountOfWaypoints - 1; j++)
            {
                _waypointsPerObjective[i][j] = _waypointCollectionPerObjective[i].transform.GetChild(j);
            }

        }

       // _agent.SetDestination(_waypointsPerObjective[General_Game.CurrentDoneObjectives][0].position);
        _agent.SetDestination(_waypointsPerObjective[0][0].position);
        _agent.updateRotation = true;

        _originalSpeedThieves = _agent.speed;

    }

    void Update()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
        {
            // Go to the next waypoint
            _currentWaypoint = (_currentWaypoint +1)% _waypointsPerObjective[_currentObjectiveWaypointCollection].Length;

            _agent.SetDestination(_waypointsPerObjective[_currentObjectiveWaypointCollection][_currentWaypoint].position);
        }

        if(_currentObjectiveWaypointCollection != General_Game.CurrentDoneObjectives)
        {
            _currentObjectiveWaypointCollection = General_Game.CurrentDoneObjectives;
            _agent.ResetPath();
            _currentWaypoint = 0;
        }

        if(General_Game.IsPoliceCalled && _currentObjectiveWaypointCollection == (_waypointCollectionPerObjective.Length - 1))
        {
            if(!_isPoliceWaypointSet)
            {
                // take the last waypoint collection
                _currentObjectiveWaypointCollection = _waypointCollectionPerObjective.Length - 1;
                _agent.ResetPath();
                _currentWaypoint = 0;

                _agent.speed = _originalSpeedThieves * General_Game.PoliceCalledThievesMultiplier;
                _isPoliceWaypointSet = true;
            }
           
        }

        
    }
}
