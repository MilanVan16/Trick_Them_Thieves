using UnityEngine;

public class StunPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Main_Character_Movement player = other.GetComponent<Main_Character_Movement>();
        if (player != null)
        {
            player.PickUpStunItem();
            Destroy(gameObject);
        }
    }
}
