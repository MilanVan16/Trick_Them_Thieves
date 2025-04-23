using System;
using UnityEngine;

public class Main_Character_Movement : MonoBehaviour
{
    [SerializeField]
    private float _walkspeed, _runMultiplier, _amountOfStaminaSeconds, _staminaReplenishMultiplier, _rotationSpeed, _runRotationSpeedMultiplier, _standUpYScale, _crouchYScale;


    public float _currentStamina;

    private CharacterController _controller;

    public bool _isCrouching;
    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        _currentStamina = _amountOfStaminaSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Crouching();
    }

    private void Movement()
    {
        Vector3 movement = Vector3.zero;

        movement = Walking(movement);
        movement = Running(movement);
        CharacterLookAtRun(movement);

        _controller.Move(movement * Time.deltaTime);
    }

    private Vector3 Walking(Vector3 movement)
    {
        float yPosition = Input.GetAxisRaw("Vertical");
        float xPosition = Input.GetAxisRaw("Horizontal");

        movement = new Vector3(xPosition, 0, yPosition);
        movement.Normalize();
        movement *= _walkspeed;

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
            return movement;
        }

        if (!Input.GetKey(KeyCode.LeftShift) && _currentStamina <= _amountOfStaminaSeconds)
        {
            _currentStamina += Time.deltaTime * _staminaReplenishMultiplier;
        }

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
    private void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isCrouching ==false)
        {
            StartCrouching();
        }

        if(Input.GetKeyDown(KeyCode.R) && _isCrouching == true)
        {
            StandUp();
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            _isCrouching = !_isCrouching;
        }
    }
    private void StartCrouching()
    {
        transform.localScale = new Vector3(transform.localScale.x,_crouchYScale ,transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - _crouchYScale, transform.position.z);
        Physics.SyncTransforms();
    }
    private void StandUp()
    {
        transform.localScale = new Vector3(transform.localScale.x, _standUpYScale, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y + _standUpYScale/2, transform.position.z);
        Physics.SyncTransforms();
    }

}
