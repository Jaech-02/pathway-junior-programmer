using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class CollisionHandler : MonoBehaviour
{
    [Header("Collision Settings")]
    public string playerTag = "Player";
    
    [Header("Effects")]
    public AudioClip collisionSound;
    [Range(0f, 1f)]
    public float soundVolume = 1f;
    
    [Header("Bounce")]
    public float impulseForce;
    public float verticalImpulse;
    public float torqueForce;
    public float continuousForce;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }
        
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.mass = 1f;
            rb.linearDamping = 0.5f;
            rb.angularDamping = 0.5f;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (!string.IsNullOrEmpty(playerTag) && collision.collider.CompareTag(playerTag) == false &&
            collision.collider.GetComponent<PlayerController>() == null)
        {
            return;
        }
        
        if (rb != null)
        {
            Vector3 relativeVelocity = collision.relativeVelocity;
            relativeVelocity.y = 0f;
            Vector3 direction = -relativeVelocity.normalized;
            
            Vector3 impulse = direction * impulseForce + Vector3.up * verticalImpulse;
            rb.AddForce(impulse, ForceMode.Impulse);
            
            Vector3 contactPoint = collision.contacts[0].point - rb.worldCenterOfMass;
            Vector3 torqueAxis = Vector3.Cross(contactPoint.normalized, Vector3.up);
            if (torqueAxis.sqrMagnitude < 0.001f)
            {
                torqueAxis = Vector3.right;
            }
            rb.AddTorque(torqueAxis.normalized * torqueForce, ForceMode.Impulse);
        }
        
        PlayCollisionSound();
    }
    
    void OnCollisionStay(Collision collision)
    {
        if (!string.IsNullOrEmpty(playerTag) && collision.collider.CompareTag(playerTag) == false &&
            collision.collider.GetComponent<PlayerController>() == null)
        {
            return;
        }
        
        if (rb != null)
        {
            Vector3 relativeVelocity = collision.relativeVelocity;
            relativeVelocity.y = 0f;
            Vector3 direction = -relativeVelocity.normalized;
            
            rb.AddForce(direction * continuousForce * Time.fixedDeltaTime, ForceMode.Force);
        }
    }
    
    void PlayCollisionSound()
    {
        if (collisionSound != null)
        {
            GameObject audioObj = new GameObject("AudioTemp_" + collisionSound.name);
            audioObj.transform.position = transform.position;
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            
            audioSource.clip = collisionSound;
            audioSource.volume = soundVolume;
            audioSource.spatialBlend = 0f;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            
            audioSource.Play();
            Object.Destroy(audioObj, collisionSound.length + 0.1f);
        }
    }
}

