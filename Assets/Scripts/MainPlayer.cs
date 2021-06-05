using UnityEngine;

public class MainPlayer : Ship
{
    [SerializeField]
    private float Speed = 5f;

    private Vector2 MousePosition;

    private Vector3 Move;

    private void Update()
    {
        float X = Input.GetAxisRaw("Horizontal");
        float Y = Input.GetAxisRaw("Vertical");

        Move = X * Speed * transform.right + Y * Speed * transform.up;
        MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        HandleShooterRotation(MousePosition);
        Shoot();

        // if player moves time is not affected else time will slow down
        // which creates a slow motion effect
        if (Mathf.Abs(X) > 0f || Mathf.Abs(Y) > 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0.1f;
        }
        

        
    }


    private void FixedUpdate()
    {
        ApplyMovement(Move);
        HandleExplosion();
    }


    public override void Shoot()
    {

        // if mouse pressed shoot
        if (Input.GetMouseButtonDown(0))
        {
            // Spawn Projectile At ShootingPoint
            base.Shoot();
        }
       
    }

}
