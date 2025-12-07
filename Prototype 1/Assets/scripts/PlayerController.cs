using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 20f;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * velocidad);
    }
}
