using System;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Objective Option")]
    [SerializeField]
    private bool _timeBasedObjective;
    [SerializeField]
    private float _timeBasedObjectiveDuration;

    private float _currentWorkTimeObjective = 0;

    [Header("Objective Option")]
    [SerializeField]
    private bool _pickupObjective;

    [Header("Important variables")]
    [SerializeField]
    private GameObject _mainCharacter;

    [Header("Don't change")]
    [SerializeField]
    private  Material _timeBasedObjectiveMaterial, _pickupObjectiveMaterial;
    void Start()
    {
        if(_timeBasedObjective)
        {
            gameObject.GetComponent<MeshRenderer>().material= _timeBasedObjectiveMaterial   ;
        }

        if(_pickupObjective)
        {
            gameObject.GetComponent<MeshRenderer>().material = _pickupObjectiveMaterial;
        }
    }

    void Update()
    {

        CharacterInteraction();

    }

    private void CharacterInteraction()
    {
        Vector3 VectorDistance = _mainCharacter.transform.position - transform.position;
        float distance = VectorDistance.magnitude;

        if (distance < General_Game.ObjectiveRadius)
        {
            Objective();
        }

    }

    private void Objective()
    {
        if (_pickupObjective)
        {
            PickupObjectiveInput();
        }

        if (_timeBasedObjective)
        {
            TimeBasedObjectiveInput();
        }

    }

    private void PickupObjectiveInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupObjective();
        }

    }
    private void PickupObjective()
    {
        Destroy(gameObject);
    }

    private void TimeBasedObjectiveInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            TimeBasedObjective();
        }

    }
    private void TimeBasedObjective()
    {
        _currentWorkTimeObjective += Time.deltaTime;

        if (_currentWorkTimeObjective >= _timeBasedObjectiveDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        General_Game.CurrentDoneObjectives++;
    }
}
