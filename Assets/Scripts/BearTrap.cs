using UnityEngine;

public class BearTrap : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            General_Game.BearTrapCount++;
            Destroy(gameObject);

        }

    }
}
