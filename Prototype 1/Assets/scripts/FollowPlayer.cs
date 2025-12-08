using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 5, -7);
    
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
            transform.position = player.transform.position + offset;
        }
    }
}
