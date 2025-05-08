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
    private Material _timeBasedObjectiveMaterial, _pickupObjectiveMaterial;
    void Start()
    {
        if (_timeBasedObjective)
        {
            gameObject.GetComponent<MeshRenderer>().material = _timeBasedObjectiveMaterial;
        }

        if (_pickupObjective)
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
        Vector3 vectorFromCharacterToThis = transform.position - _mainCharacter.transform.position;
        float distance = vectorFromCharacterToThis.magnitude;

        if (distance < General_Game.ObjectiveRadius)
        {
            Objective(vectorFromCharacterToThis);
        }

    }

    private void Objective(Vector3 vectorFromCharacterToThis)
    {
        if (_pickupObjective)
        {
            PickupObjectiveInput(vectorFromCharacterToThis);
        }

        if (_timeBasedObjective)
        {
            TimeBasedObjectiveInput(vectorFromCharacterToThis);
        }

    }

    private void PickupObjectiveInput(Vector3 vectorFromCharacterToThis)
    {
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(_mainCharacter.transform.position, vectorFromCharacterToThis.normalized, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    PickupObjective();
                }

            }
        }
    }
    private void PickupObjective()
    {
        Destroy(gameObject);
    }

    private void TimeBasedObjectiveInput(Vector3 vectorFromCharacterToThis)
    {
        if (Input.GetKey(KeyCode.E))
        {
            RaycastHit hit;


            if (Physics.Raycast(_mainCharacter.transform.position, vectorFromCharacterToThis.normalized, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    TimeBasedObjective();
                }

            }

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
        General_Game.Timer += General_Game.ExtraTime;
    }
}
