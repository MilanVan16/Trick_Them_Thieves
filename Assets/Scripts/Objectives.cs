using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("ProgressBar")]
    [SerializeField]
    private Slider _progressBarPrefab;

    [SerializeField]
    private Canvas _screenCanvas;

    private Slider _prgressBarInstantiated;

    [Header("Key Text")]
    [SerializeField]
    private TextMeshProUGUI _keys;
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

        _prgressBarInstantiated = Instantiate(_progressBarPrefab);
        _prgressBarInstantiated.transform.position = new Vector3(_keys.transform.position.x - 100, _keys.transform.position.y + 150, 0);
        _prgressBarInstantiated.transform.localScale = _prgressBarInstantiated.transform.localScale / 3;
        _prgressBarInstantiated.transform.SetParent(_screenCanvas.transform);
        _prgressBarInstantiated.gameObject.SetActive(false);

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
        else
        {
            if(_prgressBarInstantiated != null)
            {
                _prgressBarInstantiated.gameObject.SetActive(false);
            }

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

        RaycastHit hit;


        if (Physics.Raycast(_mainCharacter.transform.position, vectorFromCharacterToThis.normalized, out hit))
        {
            if (Input.GetKey(KeyCode.E))
            {
                _prgressBarInstantiated.gameObject.SetActive(true);
                if (hit.collider.gameObject == this.gameObject)
                {
                    General_Game.IsWorkingOnObjective = true;
                    TimeBasedObjective();
                }


            }
            else
            {
                General_Game.IsWorkingOnObjective = false;
            }

            ProgressBarUpdate();
            _prgressBarInstantiated.gameObject.SetActive(true);
        }
       

    }
    private void TimeBasedObjective()
    {
        _currentWorkTimeObjective += Time.deltaTime;

        if (_currentWorkTimeObjective >= _timeBasedObjectiveDuration)
        {
            General_Game.IsWorkingOnObjective = false;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        General_Game.CurrentDoneObjectives++;
        General_Game.Timer += General_Game.ExtraTime;

        if (_prgressBarInstantiated != null)
        {
            Destroy(_prgressBarInstantiated.gameObject);
        }
    }

    private void ProgressBarUpdate()
    {
        _prgressBarInstantiated.value = 1 - _currentWorkTimeObjective / _timeBasedObjectiveDuration ;
    }

    
}
