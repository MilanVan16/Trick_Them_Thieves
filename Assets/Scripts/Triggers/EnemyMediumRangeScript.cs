using UnityEngine;

public class EnemyMediumRangeScript : MonoBehaviour
{
    [HideInInspector]
    public bool _isMedium;
    [SerializeField]
    private GameObject Player;
    private Main_Character_Movement _playerScript;

    private void OnTriggerEnter(Collider other)   
    {
        _playerScript = Player.GetComponent<Main_Character_Movement>();
        if (other.CompareTag("Player"))
        {
            General_Game.IsMedium = true;
            if (_playerScript._isWalking)
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
            General_Game.IsMedium = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        General_Game.IsMedium = false;
    }
}
