using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    #region stunned
    [HideInInspector]
    public bool IsStunned;

    private float _stunTimer;
    #endregion

    #region Sound
    [Header("Sound")]
    [HideInInspector]
    public bool HeardPlayer;
    [SerializeField]
    private Image _exclemationMarkPrefab;
    private Image _exclemationMarkInstantiated;
    [SerializeField]
    private Canvas _worldCanvas;

    private bool _settedNewPosition;

    private Vector3 _playerPositionWhenFollowing;

    private GameObject _player;
    #endregion
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();

        //+2 because of the 0 objective and police objective
        _waypointsPerObjective = new Transform[General_Game.ObjectivesCount + 2][];

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

        _exclemationMarkInstantiated = Instantiate(_exclemationMarkPrefab);
        _exclemationMarkInstantiated.transform.SetParent(_worldCanvas.transform);
        _exclemationMarkInstantiated.gameObject.SetActive(false);
        

    }


    void Update()
    {
        _exclemationMarkInstantiated.transform.position = transform.position + new Vector3(0, 5f, 0);

        if (!IsStunned)
        {
            if (!HeardPlayer)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
                {
                    // Go to the next waypoint
                    _currentWaypoint = (_currentWaypoint + 1) % _waypointsPerObjective[_currentObjectiveWaypointCollection].Length;

                    _agent.SetDestination(_waypointsPerObjective[_currentObjectiveWaypointCollection][_currentWaypoint].position);
                }

                if (_currentObjectiveWaypointCollection != General_Game.CurrentDoneObjectives)
                {
                    if (!General_Game.IsPoliceCalled)
                    {
                        _currentObjectiveWaypointCollection = General_Game.CurrentDoneObjectives;
                        _agent.ResetPath();
                        _currentWaypoint = 0;

                        _agent.speed += General_Game.ExtraSpeedPerObjective;

                    }
                }

                if (General_Game.IsPoliceCalled)
                {
                    if (!_isPoliceWaypointSet)
                    {
                        // take the last waypoint collection
                        _currentObjectiveWaypointCollection = _waypointCollectionPerObjective.Length - 1;
                        _agent.ResetPath();
                        _currentWaypoint = 0;

                        _agent.speed += General_Game.ExtraSpeedPerObjective * General_Game.PoliceCalledThievesMultiplier;
                        _isPoliceWaypointSet = true;
                    }
                }
            }
            GoingToPlayer();
        }
        StunAnimation();
    }

    private void StunAnimation()
    {
        if (IsStunned)
        {
            _agent.ResetPath();
            _stunTimer += Time.deltaTime;

            //when stunned and is following you, he just gives up
            _exclemationMarkInstantiated.gameObject.SetActive(false);
          

            if (_stunTimer >= General_Game.StunDuration)
            {
                HeardPlayer = false;
                _settedNewPosition = false;
                _agent.speed = _agent.speed / 1.5f;

                _stunTimer = 0;
                IsStunned = false;
                _agent.SetDestination(_waypointsPerObjective[_currentObjectiveWaypointCollection][_currentWaypoint].position);
            }
        }
    }
    private void GoingToPlayer()
    {
        if(HeardPlayer)
        {
            if(!_settedNewPosition)
            {
                _agent.ResetPath();
                _agent.SetDestination(_player.transform.position);
                _playerPositionWhenFollowing = _player.transform.position;
                _settedNewPosition = true;

               

                _agent.speed = _agent.speed *1.5f;
                _exclemationMarkInstantiated.gameObject.SetActive(true);
            }

            if(_settedNewPosition)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
                {
                    _exclemationMarkInstantiated.gameObject.SetActive(false);
                    _agent.SetDestination(_waypointsPerObjective[_currentObjectiveWaypointCollection][_currentWaypoint].position);
                    HeardPlayer = false;
                    _settedNewPosition = false;
                    _agent.speed = _agent.speed / 1.5f;
                }
            }
         
        }
    }
}
