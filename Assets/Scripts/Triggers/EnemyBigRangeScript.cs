using UnityEngine;

public class EnemyBigRangeScript : MonoBehaviour
{
    [HideInInspector]
    public bool _isBig;
    [SerializeField]
    private GameObject Player;
    private Main_Character_Movement _playerScript;
    private void OnTriggerEnter(Collider other)
    { 
        _playerScript = Player.GetComponent<Main_Character_Movement>();
        if (other.CompareTag("Player"))
        {
            General_Game.IsBig = true;
            if (_playerScript._isRunning)
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
            General_Game.IsBig = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        General_Game.IsBig = false;
    }
}
