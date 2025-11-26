using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class cuboObstaculo : MonoBehaviour
{
    [Header("Colision")]
    public string tagVehiculo = "Player";

    [Header("Efectos")]
    public AudioClip sonidoColision;
    [Range(0f, 1f)]
    public float volumenSonido = 1f;

    [Header("Rebote")]
    public float fuerzaImpulso = 2f;
    public float impulsoVertical = 0.3f;
    public float fuerzaTorque = 5f;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!string.IsNullOrEmpty(tagVehiculo) && collision.collider.CompareTag(tagVehiculo) == false &&
            collision.collider.GetComponent<moviento>() == null)
        {
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 velRelativa = collision.relativeVelocity;
            velRelativa.y = 0f;
            Vector3 direccion = -velRelativa.normalized;

            Vector3 impulso = direccion * fuerzaImpulso + Vector3.up * impulsoVertical;
            rb.AddForce(impulso, ForceMode.Impulse);

            Vector3 puntoContacto = collision.contacts[0].point - rb.worldCenterOfMass;
            Vector3 ejeTorque = Vector3.Cross(puntoContacto.normalized, Vector3.up);
            if (ejeTorque.sqrMagnitude < 0.001f)
            {
                ejeTorque = Vector3.right;
            }
            rb.AddTorque(ejeTorque.normalized * fuerzaTorque, ForceMode.Impulse);
        }

        if (sonidoColision != null)
        {
            GameObject audioObj = new GameObject("AudioTemp_" + sonidoColision.name);
            audioObj.transform.position = transform.position;
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            
            audioSource.clip = sonidoColision;
            audioSource.volume = volumenSonido;
            audioSource.spatialBlend = 0f;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            
            audioSource.Play();
            Object.Destroy(audioObj, sonidoColision.length + 0.1f);
        }
    }
}