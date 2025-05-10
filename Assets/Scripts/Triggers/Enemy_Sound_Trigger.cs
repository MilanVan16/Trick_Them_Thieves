using UnityEngine;

public class Enemy_Sound_Trigger: MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player_Sound")
        {
            if(!_enemy.GetComponent<EnemyPatrol>().IsStunned )
            {
                _enemy.GetComponent<EnemyPatrol>().HeardPlayer = true;

            }
        }
    }
}
