using UnityEngine;

public class StunItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EnemyPatrol enemy = other.GetComponent<EnemyPatrol>();
        if (enemy != null)
        {
            enemy.Stun(2f);  // Stun for 2 seconds
            Destroy(gameObject);  // Remove the item
        }
    }
}
