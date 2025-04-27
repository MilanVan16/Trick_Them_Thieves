using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints; // Drag Waypoints into inspector if we make a longer path
    private NavMeshAgent agent;  // NavMesh and NavAgent in place so it doesnt walk through walls
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            // Go to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; //% icon makes it loop back to first waypoint
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
    private void OnTriggerEnter(Collider other)   // I tried using OnCollision but couldn't get it to work but on trigger works
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy touched the Player (Trigger)! Game Over!"); 
            Time.timeScale = 0f; // Freeze the game
        }
    }
}
