using UnityEngine;
using UnityEngine.UI;

public class Main_Character_Movement : MonoBehaviour
{
    [SerializeField]
    private float _walkspeed, _runMultiplier, _amountOfStaminaSeconds, _staminaReplenishMultiplier, _rotationSpeed, _runRotationSpeedMultiplier, _standUpYScale, _crouchYScale;

    [SerializeField]
    private GameObject _spawnPosition;

    private float _currentStamina;

    private CharacterController _controller;

    public bool _isCrouching;
    public bool _isWalking;
    public bool _isRunning;



    #region Stamina UI
    [Header("Stamina UI")]
    [SerializeField]
    private Slider _staminaSlider;
    #endregion

    #region Beartrap
    [Header("bearTrap")]
    [SerializeField]
    private GameObject _bearTrapPlaceDownPrefab;
    #endregion

    #region sound
    [SerializeField]
    private float _crouchingSoundScale;
    [SerializeField]
    private float _walkingSoundScale;
    [SerializeField]
    private float _runningSoundScale;
    [SerializeField]
    private float _objectivesSoundScale;

    private float _currentScale;

    [SerializeField]
    private GameObject _soundTrigger;

    #endregion sound
    void Start()
    {
        transform.position = new Vector3(_spawnPosition.transform.position.x, _spawnPosition.transform.position.y + _standUpYScale, _spawnPosition.transform.position.z);

        _controller = gameObject.GetComponent<CharacterController>();
        _currentStamina = _amountOfStaminaSeconds;

        _currentScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();


        _staminaSlider.value = _currentStamina / _amountOfStaminaSeconds;

        BearTrapUsing();

        if(General_Game.IsWorkingOnObjective)
        {
            _currentScale = _objectivesSoundScale;
        }

        ChangeSoundRadius();
    }

    private void Movement()
    {
        Vector3 movement = Vector3.zero;

        movement = Walking(movement);
        if (!_isCrouching)
            movement = Running(movement);

        Crouching(movement);

        CharacterLookAtRun(movement);

        _controller.Move(movement * Time.deltaTime);
    }

    private Vector3 Walking(Vector3 movement)
    {
        float yPosition = Input.GetAxisRaw("Vertical");
        float xPosition = Input.GetAxisRaw("Horizontal");


        movement = new Vector3(xPosition, 0, yPosition);

        if (movement == Vector3.zero)
        {
            _isWalking = true;
            _currentScale = 0;
        }
        if (movement != Vector3.zero)
        {
            _currentScale = _walkingSoundScale;
        }
        movement.Normalize();
        movement *= _walkspeed;

        if(_isCrouching)
        {
            movement /= 1.5f;
        }

        return movement;
    }
    private Vector3 Running(Vector3 movement)
    {
        if (_currentStamina < 0.01f)
        {
            _currentStamina = 0;
        }

        if (Input.GetKey(KeyCode.LeftShift) && _currentStamina >= 0.01f)
        {
            _currentStamina -= Time.deltaTime;
            movement *= _runMultiplier;
            _isRunning = true;
            _currentScale = _runningSoundScale;
            return movement;
        }
        else _isRunning = false;

        if (!Input.GetKey(KeyCode.LeftShift) && _currentStamina <= _amountOfStaminaSeconds)
        {
            _currentStamina += Time.deltaTime * _staminaReplenishMultiplier;
            _isRunning = true;
        }
        else _isRunning = false;

        return movement;
    }

    private void CharacterLookAtRun(Vector3 movement)
    {
        movement = movement.normalized;
        if (movement != Vector3.zero)
        {

            Quaternion lookAtRotation = Quaternion.LookRotation(movement, Vector3.up);

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, Time.deltaTime * _rotationSpeed);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, Time.deltaTime * _rotationSpeed * _runRotationSpeedMultiplier);
            }

        }

    }
    private void Crouching(Vector3 movement)
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && _isCrouching == false)
        {
            StartCrouching();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && _isCrouching == true)
        {
            StandUp();
        }


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isCrouching = !_isCrouching;
        }

        if (_isCrouching)
        {
            if(movement != Vector3.zero)
            {
                _currentScale = _crouchingSoundScale;
            }

        }
    }
    private void StartCrouching()
    {
        transform.localScale = new Vector3(transform.localScale.x, _crouchYScale, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - _crouchYScale, transform.position.z);
        Physics.SyncTransforms();
    }
    private void StandUp()
    {
        transform.localScale = new Vector3(transform.localScale.x, _standUpYScale, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y + _standUpYScale / 2, transform.position.z);
        Physics.SyncTransforms();
    }

    private void BearTrapUsing()
    {
        if (General_Game.BearTrapCount != 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                PlaceDownBeartrap();
                General_Game.BearTrapCount--;
            }
        }
    }

    private void PlaceDownBeartrap()
    {
        GameObject beartrap = Instantiate(_bearTrapPlaceDownPrefab);
        beartrap.transform.position = transform.position - new Vector3(0, transform.localScale.y, 0) + new Vector3(0, _bearTrapPlaceDownPrefab.transform.localScale.y, 0);
    }

    private void ChangeSoundRadius()
    {
        _soundTrigger.transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
    }

}
