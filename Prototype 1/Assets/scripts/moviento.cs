using UnityEngine;
using UnityEngine.InputSystem;

public class moviento : MonoBehaviour
{
    [Header("Velocidad")]
    public float velocidad;
    public float velocidadGiro;
    
    [Header("Estabilidad")]
    public float alturaMinima = -10f;
    
    private Rigidbody rb;
    private float inputMovimiento;
    private float inputGiro;
    private Vector3 posicionInicial;
    private Keyboard teclado;

    void Start()
    {
        posicionInicial = transform.position;
        
        rb = GetComponent<Rigidbody>();
        
        teclado = Keyboard.current;
        
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.mass = 1f;
        rb.linearDamping = 2f;
        rb.angularDamping = 3f;
    }

    void Update()
    {
        if (teclado != null)
        {
            if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed)
            {
                inputMovimiento = 1f;
            }
            else if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed)
            {
                inputMovimiento = -1f;
            }
            else
            {
                inputMovimiento = 0f;
            }
            
            if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed)
            {
                inputGiro = -1f;
            }
            else if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed)
            {
                inputGiro = 1f;
            }
            else
            {
                inputGiro = 0f;
            }
        }
        
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

    void FixedUpdate()
    {
        Vector3 movimiento = transform.forward * inputMovimiento * velocidad;
        rb.AddForce(movimiento, ForceMode.VelocityChange);
        
        float giro = inputGiro * velocidadGiro * Time.fixedDeltaTime;
        transform.Rotate(0, giro, 0);
    }
}
