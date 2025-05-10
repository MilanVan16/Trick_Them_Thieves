using UnityEngine;
using UnityEngine.AI;

public class BearTrapPlacedDown : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyPatrol>().IsStunned = true;
            Destroy(gameObject);
        }
    }

}
