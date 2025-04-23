using UnityEngine;

public class Camera_Logic : MonoBehaviour
{

    [SerializeField]
    private GameObject _mainCharacter;
    private Vector3 _offset;
    void Start()
    {
        _offset = transform.position - _mainCharacter.transform.position;
    }

    private void LateUpdate()
    {
        transform.position = _mainCharacter.transform.position + _offset;

    }
}
