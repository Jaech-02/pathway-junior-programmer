using UnityEngine;

public class cuboObstaculo : MonoBehaviour
{
    [Header("Rotacion")]
    public float velocidadRotacion = 90f; 
    
    [Header("Colision")]
    public string tagVehiculo = "Player";
    
    [Header("Efectos")]
    public GameObject efectoParticulas;
    public AudioClip sonidoColision;
    [Range(0f, 1f)]
    public float volumenSonido = 1f;
    public float tiempoDestruccion = 0.1f;
    public float tiempoVidaParticulas = 2f;
    public Color colorParticulas = Color.yellow;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    void Update()
    {
        transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(tagVehiculo) && other.CompareTag(tagVehiculo))
        {
            DestruirObstaculo();
        }
        else if (other.GetComponent<moviento>() != null)
        {
            DestruirObstaculo();
        }
    }

    void DestruirObstaculo()
    {
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
            audioSource.priority = 128;
            audioSource.pitch = 1f;
            
            audioSource.Play();
            
            Destroy(audioObj, sonidoColision.length + 0.1f);
            
            Debug.Log("Sonido reproducido: " + sonidoColision.name + " (Volumen: " + volumenSonido + ")");
        }
        else
        {
            Debug.LogWarning("Sonido de colision no asignado en " + gameObject.name);
        }
        
        if (efectoParticulas != null)
        {
            GameObject particulasInstancia = Instantiate(efectoParticulas, transform.position, Quaternion.identity);
            
            ParticleSystem[] sistemasParticulas = particulasInstancia.GetComponentsInChildren<ParticleSystem>();
            
            foreach (ParticleSystem ps in sistemasParticulas)
            {
                if (ps != null)
                {
                    var main = ps.main;
                    main.startColor = colorParticulas;
                    
                    var renderer = ps.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        Material mat = new Material(renderer.material);
                        mat.color = colorParticulas;
                        mat.SetColor("_TintColor", colorParticulas);
                        renderer.material = mat;
                    }
                    
                    ps.Play();
                }
            }
            
            if (sistemasParticulas.Length == 0)
            {
                ParticleSystem ps = particulasInstancia.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    var main = ps.main;
                    main.startColor = colorParticulas;
                    
                    var renderer = ps.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        Material mat = new Material(renderer.material);
                        mat.color = colorParticulas;
                        mat.SetColor("_TintColor", colorParticulas);
                        renderer.material = mat;
                    }
                    
                    ps.Play();
                }
            }
            
            Destroy(particulasInstancia, tiempoVidaParticulas);
        }
        
        GetComponent<Renderer>().enabled = false;
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        Destroy(gameObject, tiempoDestruccion);
    }
}

