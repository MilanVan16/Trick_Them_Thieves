using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Door
    [Header("Important Variables")]
    [SerializeField]
    private Color _color;
    private GameObject _player;
    #endregion

    #region Key
    private GameObject _key;

    private GameObject[] _keyObjects;

    [HideInInspector]
    public bool IsKeyPickedUp;
    #endregion

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _key = transform.GetChild(0).gameObject;

        int amountOfKeyParts = _key.transform.childCount;
        _keyObjects = new GameObject[amountOfKeyParts];

        for (int i = 0; i < amountOfKeyParts; i++)
        {
            _keyObjects[i] = _key.transform.GetChild(i).gameObject;
            _keyObjects[i].GetComponent<Renderer>().material.color = _color;
        }

        gameObject.GetComponent<Renderer>().material.color = _color;
    }

    void Update()
    {
        if (!IsKeyPickedUp)
        {
            KeyLogic();
        }
        DoorLogic();
    }

    private void KeyLogic()
    {
        Vector3 vectorFromPlayerToKey = _player.transform.position - _key.transform.position;
        float distance = vectorFromPlayerToKey.magnitude;

        if (distance <= General_Game.KeyRadius)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                if (Physics.Raycast(_key.transform.position, vectorFromPlayerToKey, out hit))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        IsKeyPickedUp = true;
                        Destroy(_key);
                    }
                }
            }

        }
    }
    private void DoorLogic()
    {
        Vector3 vectorFromPlayerToDoor =_player.transform.position - transform.position ;
        float distance = vectorFromPlayerToDoor.magnitude;

        if (distance <= General_Game.DoorRadius)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, vectorFromPlayerToDoor, out hit))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        if (IsKeyPickedUp)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }

        }
    }

}
