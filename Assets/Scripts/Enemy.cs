using UnityEngine;


// Inherit from Ship
public class Enemy : Ship
{

    public Transform Player;
    public int Health = 3;
    // Time between shots
    public float ShootingDelay = 1f;
    public int ShootingRate = 3;
    public LayerMask LMask;

    private float TimePassed = 0f;
    private Vector2 ShootPosition;


    private void Awake()
    {
        TimePassed = ShootingDelay;
    }

    private void Update()
    {
        ShootPosition = Player.position;
        HandleShooterRotation(ShootPosition);
        HandleShooting();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // when hit pushback
        HandleExplosion();
    }


    void HandleShooting()
    {

        TimePassed += Time.deltaTime;

        Vector2 Pos = ShootingPoint.transform.position;
        Vector2 Direction = ShootPosition - Pos;
        RaycastHit2D Hit = Physics2D.Raycast(Pos, Direction, Mathf.Infinity, ~LMask);
        
        // If raycast detect player Shoot it
        if (Hit.transform.tag.Equals("Player"))
        {
            if (TimePassed > ShootingDelay)
            {
                Shoot();
                TimePassed = 0f;
            }
        }

        
    }


    public void UpdateHealth(int value)
    {
        Health += value;
        if (Health <= 0)
            StartCoroutine(UniqueDestroy());
    }

}
