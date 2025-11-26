using UnityEngine;
using UnityEngine.InputSystem;

public class moviento : MonoBehaviour
{
    [Header("Velocidad")]
    public float velocidad = 20f;

    [Header("Estabilidad")]
    public float alturaMinima = -10f;
    
    private Rigidbody rb;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
        
        rb = GetComponent<Rigidbody>();
        
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 1f;
        rb.linearDamping = 2f;
        rb.angularDamping = 3f;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * velocidad);
        
        if (transform.position.y < alturaMinima)
        {
            ReposicionarVehiculo();
        }
    }
    
    void ReposicionarVehiculo()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        transform.position = new Vector3(posicionInicial.x, 1f, posicionInicial.z);
        transform.rotation = Quaternion.identity;
    }
}