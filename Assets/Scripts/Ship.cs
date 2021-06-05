using System.Collections;
using UnityEngine;

public class Ship : MonoBehaviour
{

    // Explosion Vars
    protected bool ExplosionBegin = false;
    protected bool ExplosionFinished = true;
    protected Vector2 Direction = new Vector2(0f, 0f);
    protected float ExplosionForce = 0f;
    protected float ExplosionTimer = 0f;
    protected bool LoseControl = false;
    protected bool ExplosionOccured = false;
    public float LoseControlTimer = 0.5f;

    // Handle Shooting
    protected Camera MainCamera;
    protected Rigidbody2D Rb;

    public GameObject Projectile;
    public GameObject ShootingPoint;
    public GameObject Shooter;

    public Material ProjectileMaterial;

    public AudioClip sfx;
    private AudioSource m_AudioSource;

    public ParticleSystem DestroyedParticle;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        Rb = gameObject.GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
    }


    // Shoot projectile
    public virtual void Shoot()
    {
        if (!ShootingPoint.activeSelf)
            return;

        m_AudioSource.clip = sfx;
        m_AudioSource.volume = 0.3f;
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        GameObject Obj = Instantiate(Projectile, ShootingPoint.transform.position, ShootingPoint.transform.rotation);
        Obj.GetComponent<Projectile>().SetMaterial(ProjectileMaterial);
    }


    // Rotate Shooting point
    public void HandleShooterRotation(Vector2 Pos)
    {
        if (Rb == null)
            return;

        // Calculate Direction
        Vector2 Direction = Pos - Rb.position;
        // Get Angle
        float Angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 90f;
        // Rotate
        Shooter.transform.rotation = Quaternion.Euler(0f, 0f, Angle);
    }


    // Set Pushback parametres
    public void Explosion(Vector2 Direction, float ExplosionForce)
    {
        this.ExplosionForce = ExplosionForce;
        this.Direction = Direction;
        ExplosionBegin = true;
    }


    // Perform pushback by adding force
    public void HandleExplosion()
    {
        if (GetComponent<Rigidbody2D>() == null)
            return;

        if (ExplosionBegin)
        {
            GetComponent<Rigidbody2D>().AddForce(Direction * ExplosionForce * Time.fixedDeltaTime);
            ExplosionBegin = false;
            ExplosionFinished = false;
            ExplosionTimer = 0f;
            ExplosionOccured = true;
        }

        if (!ExplosionFinished)
        {
            if(ExplosionTimer > LoseControlTimer)
            {
                ExplosionFinished = true;
                LoseControl = false;
                Rb.velocity = new Vector2(0, 0);
            }
            else
            {
                LoseControl = true;
            }
        }

        ExplosionTimer += Time.deltaTime;
    }


    // Move Player
    public void ApplyMovement(Vector3 Move)
    {
        if (!LoseControl)
            transform.position += Move * Time.fixedDeltaTime;
            
    }


    // Delay destruction to handle some task before destruction
    public IEnumerator UniqueDestroy()
    {
        // Disable renderer and collisions
        foreach (SpriteRenderer Sr in GetComponentsInChildren<SpriteRenderer>())
        {
            Sr.enabled = false;
        }

        GetComponent<Collider2D>().enabled = false;
        ShootingPoint.SetActive(false);

        Destroy(Rb);

        Instantiate(DestroyedParticle, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
