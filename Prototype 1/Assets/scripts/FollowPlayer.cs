using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    
    void Start()
    {
        if (player == null)
        {
            PlayerController jugador = FindFirstObjectByType<PlayerController>();
            if (jugador != null)
            {
                player = jugador.gameObject;
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position + new Vector3(0, 3, -7);
        }
    }
}
