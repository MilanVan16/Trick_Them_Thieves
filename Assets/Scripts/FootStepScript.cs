using UnityEngine;

public class FootStepScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _footStep;
    private Main_Character_Movement _playerScript;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private AudioSource _audioSource;
    void Start()
    {
        _playerScript = Player.GetComponent<Main_Character_Movement>();
        _footStep.SetActive(false);
    }

    void Update()
    {
        if (General_Game.IsHidden)
        {
            StopFootsteps();
        }
        if (!General_Game.IsHidden)
        {
            if (_playerScript._isWalking)
            {
                _audioSource.pitch = 1.1f;
            }
            if (_playerScript._isRunning)
            {
                _audioSource.pitch = 1.6f;
            }
            if (_playerScript._isCrouching)
            {
                _audioSource.pitch = 0.6f;
            }
            if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
            {
                footsteps();
            }

            if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d"))
            {
                StopFootsteps();
            }
        }
       
    }

    private void StopFootsteps()
    {
        _footStep.SetActive(false);
    }

    private void footsteps()
    {
        _footStep.SetActive(true);

    }
}
