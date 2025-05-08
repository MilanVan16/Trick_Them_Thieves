using UnityEngine;

public class EnemySmallRangeScript : MonoBehaviour
{
    [HideInInspector]
    public bool _isSmall;
    [SerializeField]
    private GameObject Player;
    private Main_Character_Movement _playerScript;
    private void OnTriggerEnter(Collider other)
    {
        _playerScript = Player.GetComponent<Main_Character_Movement>();
        if (other.CompareTag("Player"))
        {
            General_Game.IsSmall = true;
            if (_playerScript._isCrouching)
            {
                General_Game.IsChasing = true;
                General_Game.ChasingWaypoint = Player.transform.position;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            General_Game.IsSmall = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        General_Game.IsSmall = false;
    }
}
