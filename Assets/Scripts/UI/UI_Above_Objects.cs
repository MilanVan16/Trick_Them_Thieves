using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Above_Objects : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Set right needed variables")]
    [SerializeField]
    private Image _imageUIPrefab;
    [SerializeField]
    private Canvas _canvas;


    private Image _imageUI;
    private TextMeshProUGUI _textUI;
    private GameObject _mainCharacter;

    [Header("Add text of name")]
    [SerializeField]
    private string _text;


    [Header("Optional Option 1")]
    [SerializeField]
    private bool _hasBoundary;
    [SerializeField]
    private float _radius;

    [Header("optional Option 2")]
    [SerializeField]
    private bool _isObjective;

    [Header("Is a hiding object")]
    [SerializeField]
    private bool _isAHidingObject;
    private string _hidingText = "Press E to exit";

    [Header("Is a door")]
    [SerializeField]
    private bool _isADoor;
    [SerializeField]
    private string _doorText;
    [SerializeField]
    private string _keyText;

    private Door _doorScript;

    private GameObject _door;
    private GameObject _key;

    private Image _doorUI;
    private TextMeshProUGUI _doorTextPro;
    private Image _keyUI;
    private TextMeshProUGUI _keyTextPro;

    void Start()
    {
        _mainCharacter = GameObject.FindWithTag("Player");

        if (!_isADoor)
        {

            //if (_yOffset == 0)
            //{
            //    _yOffset = transform.position.y + transform.localScale.y / 4;
            //}
            _imageUI = Instantiate(_imageUIPrefab);
            _textUI = _imageUI.GetComponentInChildren<TextMeshProUGUI>();

            _imageUI.transform.SetParent(_canvas.transform);

            _textUI.text = _text;
            _imageUI.transform.position = transform.position + General_Game.UIOffset;

        }

        if (_isObjective || _hasBoundary)
        {
            _imageUI.enabled = false;
            _textUI.gameObject.SetActive(false);
        }

        if (_isADoor)
        {
            _door = transform.gameObject;

            _doorScript = _door.GetComponent<Door>();


            _key = _door.transform.GetChild(0).gameObject;

            _doorUI = Instantiate(_imageUIPrefab);
            _doorTextPro = _doorUI.GetComponentInChildren<TextMeshProUGUI>();
            _doorUI.transform.SetParent(_canvas.transform);
            _doorUI.transform.position = _door.transform.position + General_Game.UIOffset;
            _doorTextPro.text = _doorText;
            _doorUI.enabled = false;
            _doorTextPro.gameObject.SetActive(false);

            _keyUI = Instantiate(_imageUIPrefab);
            _keyTextPro = _keyUI.GetComponentInChildren<TextMeshProUGUI>();
            _keyUI.transform.SetParent(_canvas.transform);
            _keyUI.transform.position = _key.transform.position + General_Game.UIOffset;
            _keyTextPro.text = _keyText;
            _keyUI.enabled = false;
            _keyTextPro.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasBoundary)
        {
            Option1();


        }


        if (_isObjective)
        {
            Option2();
        }

        if (_isADoor)
        {

            if (_doorScript.IsKeyPickedUp)
            {
                _keyUI.gameObject.SetActive(false);
                _doorTextPro.text = "Press E to open";
            }



            DoorKeyUI();
        }

    }

    private void Option1()
    {
        Vector3 vectorFromCharacterToThis = transform.position - _mainCharacter.transform.position;
        float distance = vectorFromCharacterToThis.magnitude;

        RaycastHit hit;

        if (distance <= _radius)
        {
            if (Physics.Raycast(_mainCharacter.transform.position, vectorFromCharacterToThis.normalized, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    _imageUI.enabled = true;
                    _textUI.gameObject.SetActive(true);

                    if (_isAHidingObject)
                    {
                        if (General_Game.IsHidden)
                        {
                            _textUI.text = _hidingText;
                        }
                        if (!General_Game.IsHidden)
                        {
                            _textUI.text = _text;
                        }
                    }
                }
            }
        }

        if (distance > _radius)
        {
            _imageUI.enabled = false;
            _textUI.gameObject.SetActive(false);
        }

    }
    private void Option2()
    {
        Vector3 vectorFromCharacterToThis = transform.position - _mainCharacter.transform.position;
        float distance = vectorFromCharacterToThis.magnitude;

        RaycastHit hit;

        if (distance <= General_Game.ObjectiveRadius)
        {
            if (Physics.Raycast(_mainCharacter.transform.position, vectorFromCharacterToThis.normalized, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    _imageUI.enabled = true;
                    _textUI.gameObject.SetActive(true);
                }
            }
        }

        if (distance > General_Game.ObjectiveRadius)
        {
            _imageUI.enabled = false;
            _textUI.gameObject.SetActive(false);
        }



    }

    private void DoorKeyUI()
    {
        DoorUI();
        if(!_doorScript.IsKeyPickedUp)
        {
            KeyUI();
        }

    }

    private void DoorUI()
    {
        Vector3 vectorCharacterToEnemy = _mainCharacter.transform.position - _door.transform.position;
        float distance = vectorCharacterToEnemy.magnitude;

        if (distance <= General_Game.DoorRadius)
        {
            RaycastHit hit;
            if (Physics.Raycast(_door.transform.position, vectorCharacterToEnemy.normalized, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    _doorUI.enabled = true;
                    _doorTextPro.gameObject.SetActive(true);
                }
            }
        }

        if (distance > General_Game.DoorRadius)
        {
            _doorUI.enabled = false;
            _doorTextPro.gameObject.SetActive(false);
        }
    }

    private void KeyUI()
    {
        Vector3 vectorCharacterToEnemy = _mainCharacter.transform.position - _key.transform.position;
        float distance = vectorCharacterToEnemy.magnitude;

        if (distance <= General_Game.KeyRadius)
        {
            RaycastHit hit;
            if (Physics.Raycast(_key.transform.position, vectorCharacterToEnemy.normalized, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    _keyUI.enabled = true;
                    _keyTextPro.gameObject.SetActive(true);
                }
            }
        }

        if (distance > General_Game.KeyRadius)
        {
            _keyUI.enabled = false;
            _keyTextPro.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (!_isADoor)
        {
            if (_imageUI != null)
            {
                _imageUI.gameObject.SetActive(false);
            }
        }

        if (_isADoor)
        {
            if(_doorUI != null)
            {
                _doorUI.gameObject.SetActive(false);
            }

        }

    }

}
