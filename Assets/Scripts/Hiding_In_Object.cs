using UnityEngine;

public class Hiding_In_Object : MonoBehaviour
{
    [Header("Important Variables")]
    [SerializeField]
    private GameObject _mainCharacter;
    [SerializeField]
    private float _hidingRadius;

    private bool _isHidenInThisCloset;

    private bool _enteredOrExitedHiddenThisFrame;

    void Start()
    {

    }


    void Update()
    {
        //calculating the distance so you cant enter it at the other side of the map
        Vector3 vectorFromCharacterToThis = transform.position -_mainCharacter.transform.position;
        float distance = vectorFromCharacterToThis.magnitude;

        RaycastHit hit;

        if (distance <= _hidingRadius)
        {
            if (General_Game.IsPoliceCalled == false)
            {
                if (Physics.Raycast(_mainCharacter.transform.position, vectorFromCharacterToThis.normalized, out hit))
                {
                    //the raycast so that there is no wall between the character and the closet
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        //looking if he isn't hidden yet and presses E to get in
                        if (Input.GetKeyDown(KeyCode.E) && General_Game.IsHidden == false)
                        {
                            _mainCharacter.GetComponent<MeshRenderer>().enabled = false;

                            //to also disable they eyes
                            MeshRenderer[] childrenMeshRenderers = _mainCharacter.GetComponentsInChildren<MeshRenderer>();
                            foreach (MeshRenderer child in childrenMeshRenderers)
                            {
                                child.enabled = false;
                            }

                            _mainCharacter.GetComponent<Main_Character_Movement>().enabled = false;
                            General_Game.IsHidden = true;
                            _isHidenInThisCloset = true;
                            _enteredOrExitedHiddenThisFrame = true;
                        }
                    }

                }
            }
        }

        //otherwise when entering it, the same frame the input is still seen as correct so you will get out of it directly
        if (!_enteredOrExitedHiddenThisFrame)
        {
            //for getting out of the hiding place
            if (Input.GetKeyDown(KeyCode.E) && General_Game.IsHidden == true && _isHidenInThisCloset == true)
            {
                _mainCharacter.GetComponent<MeshRenderer>().enabled = true;

                //to also disable the eyes
                _mainCharacter.GetComponent<Main_Character_Movement>().enabled = true;
                MeshRenderer[] childrenMeshRenderers = _mainCharacter.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer child in childrenMeshRenderers)
                {
                    child.enabled = true;
                }

                _isHidenInThisCloset = false;
                General_Game.IsHidden = false;
            }
        }
        _enteredOrExitedHiddenThisFrame = false;

    }
}
