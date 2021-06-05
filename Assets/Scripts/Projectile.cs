using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Projectile : MonoBehaviour
{

    private Rigidbody2D Rb;
    
    public float Speed = 100f;
    public float ExplosionForce = 100;
    public int BounceNumber = 3;
    private int BounceCount = 0;

    public GameObject Trigger;
    public GameObject Particle;
    private Material Mater;

    public AudioClip sfx;
    private GameObject GManager;
    private AudioSource m_AudioSource;

    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Rb.velocity = transform.up.normalized * Speed;
    }

    private void Start()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        
        // ignoring collision to prevent rigidbodies effects
        // will come in handy when handling pushback manually
        foreach(GameObject Obj in Enemies)
            Physics2D.IgnoreCollision(Obj.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

        foreach (GameObject Obj in Players)
            Physics2D.IgnoreCollision(Obj.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

        GManager = GameObject.FindGameObjectWithTag("GameManager");
        m_AudioSource = GetComponent<AudioSource>();
    }

    
    public void SetMaterial(Material Mat)
    {
        SpriteRenderer Sr = GetComponent<SpriteRenderer>();
        Sr.material = Mat;
        TrailRenderer Trail = GetComponent<TrailRenderer>();
        Trail.material = Mat;
        Mater = Mat;
    }


    // Used trigger and not a collider is to prevent Rigidbody handling physics automatically
    // i want to apply pushback manually for better controle
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.tag.Equals("Enemy"))
        {
            GManager.GetComponent<GameManager>().UpdateScore(5);
            collision.transform.GetComponent<Enemy>().UpdateHealth(-1);
        }

        if (collision.transform.tag.Equals("Player"))
        {
            GManager.GetComponent<GameManager>().UpdateScore(-5);
        }

        if (collision.transform.tag.Equals("Player") || collision.transform.tag.Equals("Enemy"))
        {
            Rigidbody2D playerRb = collision.transform.GetComponent<Rigidbody2D>();
            Vector2 Direction = GetComponent<Rigidbody2D>().position - playerRb.position;
            Direction = -Direction.normalized;
            playerRb.transform.GetComponent<Ship>().Explosion(Direction, ExplosionForce);
            StartCoroutine(UniqueDestroy());
        }
    }



    // play sound on collision with other gameobjects
    private void OnCollisionEnter2D(Collision2D collision)
    {

        m_AudioSource.clip = sfx;
        m_AudioSource.volume = 0.3f;
        m_AudioSource.PlayOneShot(m_AudioSource.clip);

        if (collision.transform.tag.Equals("Wall"))
        {
            BounceCount++;
            if (BounceCount >= BounceNumber)
                StartCoroutine(UniqueDestroy());
            else
            {
                GetComponentInChildren<Light2D>().intensity *= 3;
            }
        }
    }


    // delay destruction
    IEnumerator UniqueDestroy()
    {
        // Disable renderer and collisions
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<TrailRenderer>().enabled = false;
        GetComponentInChildren<Collider2D>().enabled = false;
        Destroy(Rb);

        Particle.GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().material = Mater;
        Instantiate(Particle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

}
