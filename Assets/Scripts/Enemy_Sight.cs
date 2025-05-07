using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_Sight : MonoBehaviour
{
    #region important variables
    private Transform _rayPosition;

    #endregion
    private void Start()
    {
        _rayPosition = transform.GetChild(0).transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            RaycastHit enemyToPlayer;
            Vector3 direction = other.gameObject.transform.position - _rayPosition.position;

            if (Physics.Raycast(_rayPosition.position, direction.normalized, out enemyToPlayer))
            {
                if(enemyToPlayer.collider.gameObject.tag == "Player")
                {
                    SceneManager.LoadScene("GameOver");
                }
            }
        }
    }
   
}
