using UnityEngine;

public class Camera_Logic : MonoBehaviour
{

    [SerializeField]
    private GameObject _mainCharacter;
    [SerializeField]
    private Vector3 _offset;
    
    void Start()
    {
    }

    private void LateUpdate()
    {
        transform.position = _mainCharacter.transform.position + _offset;

    }
}
