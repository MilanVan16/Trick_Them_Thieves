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

    [Header("Add ofsset (not needed) and text of name")]
    [SerializeField]
    private string _text;
    [SerializeField]
    private float _yOffset = 0;

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

    void Start()
    {
        _mainCharacter = GameObject.FindWithTag("Player");
        if (_yOffset == 0)
        {
            _yOffset = transform.position.y + transform.localScale.y / 4;
        }
        _imageUI = Instantiate(_imageUIPrefab);
        _textUI = _imageUI.GetComponentInChildren<TextMeshProUGUI>();

        _imageUI.transform.SetParent(_canvas.transform);

        _textUI.text = _text;
        _imageUI.transform.position = transform.position + new Vector3(0, _yOffset, 0);

        if (_isObjective || _hasBoundary)
        {
            _imageUI.enabled = false;
            _textUI.gameObject.SetActive(false);
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
    private void OnDestroy()
    {
        _imageUI.gameObject.SetActive(false);

    }

}
