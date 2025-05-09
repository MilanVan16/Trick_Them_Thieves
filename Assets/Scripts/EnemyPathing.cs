using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    private bool isStunned = false;

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

    #region Chasing instances
    [SerializeField]
    private GameObject SmallRangeTrigger;
    [SerializeField]
    private GameObject MediumRangeTrigger;
    [SerializeField]
    private GameObject BigRangeTrigger;
    private bool _isSmall;
    private bool _isMedium;
    private bool _isBig;
    [SerializeField]
    private GameObject Player;
    private Main_Character_Movement _playerScript;

    private GameObject _big;
    private GameObject _medium;
    private GameObject _small;

    [SerializeField]
    private Material[] Materials;

    private Renderer _renderer;

    private Vector3 _initialScale;
    private Vector3 _triggerInitialScale;

    [SerializeField]
    private GameObject[] _chasingWayPoints;

    private bool _isWalking;
    private bool _isCrouching;
    private bool _isRunning;
    #endregion

    private float _walkTime = 5f;
    private float _timer;
    private float _initialEnemySpeed = 4f;
    private float _increaser = 2.5f;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    [SerializeField]
    private GameObject[] _rotationWayPoint;

    private bool _hasGoneToRotationPoint = false;
    private bool _isRotating = false;
    private float _rotationTimer = 0f;
    private float _rotationDuration = 3f; // duration of rotation in seconds
    private Quaternion _startRotation;
    private Quaternion _targetRotation;


    void Start()
    {
        _initialScale = transform.localScale;
        _triggerInitialScale = SmallRangeTrigger.transform.localScale;

        _playerScript = Player.GetComponent<Main_Character_Movement>();

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

    }

    public void Stun(float duration)
    {
        if (!isStunned)
            StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        GetComponent<NavMeshAgent>().isStopped = true;

        yield return new WaitForSeconds(duration);

        isStunned = false;
        GetComponent<NavMeshAgent>().isStopped = false;
    }

    void Update()
    {
        EnemyFaster();

        if (General_Game.ChasingWaypoint != null)
            _chasingWayPoints[0].transform.position = General_Game.ChasingWaypoint;
        _chasingWayPoints[1].transform.position = transform.position;


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
            }
        }
        ChasingLogic();

        #region Chasing logic

        _isWalking = _playerScript._isWalking;
        _isCrouching = _playerScript._isCrouching;
        _isRunning = _playerScript._isRunning;

        _isSmall = General_Game.IsSmall;
        _isMedium = General_Game.IsMedium;
        _isBig = General_Game.IsBig;


        if (!_isCrouching)
        {
            transform.localScale = _initialScale;
        }

        if (_isCrouching)
        {
            _isWalking = false;
        }

        /*        if (_isWalking && _isMedium)
                {
                    _renderer.material = Materials[1];
                }
                if (_isCrouching && _isSmall)
                {
                    _renderer.material = Materials[0];
                }
                if (_isRunning && _isBig)
                {
                    _renderer.material = Materials[2];

                }*/
        #endregion

        if (General_Game.IsPoliceCalled)
        {
            if (!_isPoliceWaypointSet)
            {
                // take the last waypoint collection
                _currentObjectiveWaypointCollection = _waypointCollectionPerObjective.Length - 1;
                _agent.ResetPath();
                _currentWaypoint = 0;

                _agent.speed = _originalSpeedThieves * General_Game.PoliceCalledThievesMultiplier;
                _isPoliceWaypointSet = true;
            }

        }

        if (isStunned) return;



    }

    private void ChasingLogic()
    {
        if (General_Game.IsChasing)
        {
            _currentObjectiveWaypointCollection = _waypointCollectionPerObjective.Length - 1;
            _agent.SetDestination(General_Game.ChasingWaypoint);
            if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
            {
                General_Game.IsChasing = false;
            }
        }
    }

    private void EnemyFaster()
    {
        _timer += Time.deltaTime;
        if (General_Game.CurrentDoneObjectives == 1)
        {
            _agent.speed = _initialEnemySpeed + _increaser;
            if (_timer > 10)
                RotationAround();
        }
        if (General_Game.CurrentDoneObjectives == 2)
        {
            _agent.speed = _initialEnemySpeed + _increaser * 2;
            if (_timer > 8)
                RotationAround();
        }
        if (General_Game.CurrentDoneObjectives == 3)
        {
            _agent.speed = _initialEnemySpeed + _increaser * 3;
            if (_timer >= 6.5f)
            {
                RotationAround();
            }
        }
        if (General_Game.CurrentDoneObjectives == 4)
        {
            _agent.speed = _initialEnemySpeed + _increaser * 4;
            if (_timer >= 5)
            {
                RotationAround();
            }
        }
    }

    private void RotationAround()
    {
        // Move to rotation point once
        if (!_hasGoneToRotationPoint)
        {
            _agent.SetDestination(_rotationWayPoint[_rotationWayPoint.Length - 1].transform.position);
            _hasGoneToRotationPoint = true;
        }

        // Wait until enemy reaches the rotation point
        if (_hasGoneToRotationPoint && !_isRotating && _agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
        {
            _isRotating = true;
            _rotationTimer = 0f;
            _agent.isStopped = true;
        }

        // Perform rotation in place
        if (_isRotating)
        {
            _rotationTimer += Time.deltaTime;

            float rotationSpeed = 360f / _rotationDuration; // degrees per second
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            if (_rotationTimer >= _rotationDuration + 0.5f)
            {
                _isRotating = false;
                _hasGoneToRotationPoint = false;
                _agent.isStopped = false;
                _timer = 0f;

                // Resume patrol
                _agent.SetDestination(_waypointsPerObjective[_currentObjectiveWaypointCollection][_currentWaypoint].position);
            }
        }
    }
}
