using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A homing missile that lock on a target.
/// </summary>
public class HomingMissile : PowerUpEffect
{
    /// <summary>
    /// Explosion prefab.
    /// </summary>
    public GameObject explosion;

    /// <summary>
    /// Explosion force on impact.
    /// </summary>
    public float explosion_force = 4.0f;

    /// <summary>
    /// Maximum missile steering.
    /// </summary>
    public float steering = 6.0f;

    /// <summary>
    /// Missile forward speed.
    /// </summary>
    public float speed = 8.0f;

    /// <summary>
    /// Missile forward speed smoothing factor.
    /// </summary>
    public float speed_smooth = 10f;

    /// <summary>
    /// Number of orbs detached upon impact.
    /// </summary>
    public int damage = 3;

    public void OnDestroy()
    {
        if(explosion)
        {
            var instance = Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        rigid_body = GetComponent<Rigidbody>();
        floating_object = GetComponent<FloatingObject>();

        // Lock onto the nearest target.

        float min_distance = float.MaxValue;

        foreach (Ship ship in FindObjectsOfType<Ship>())
        {
            if (ship != Owner)
            {
                var distance = (ship.transform.position - Owner.transform.position).sqrMagnitude;

                if (distance < min_distance)
                {
                    target = ship;
                    min_distance = distance;
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();

        if (target != null)
        {
            var direction = (target.transform.position - transform.position).normalized;

            var forward = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * steering, 0).normalized;

            transform.rotation = Quaternion.LookRotation(forward, floating_object.Up);
        }

        var missile_forward = Vector3.Lerp(transform.forward, floating_object.Forward, Time.deltaTime * speed_smooth);

        rigid_body.AddForce(missile_forward * speed, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Called whenever the missile hits something.
    /// </summary>
    void OnTriggerEnter(Collider collision)
    {
        if(isServer)
        {
            var ship = collision.gameObject.GetComponent<Ship>();

            if (ship && ship != Owner)
            {
                ship.GetComponent<Rigidbody>().AddForce(transform.forward * explosion_force, ForceMode.VelocityChange);

                for(int number = 0; number < damage; ++number)
                {
                    ship.RpcDetachOrb();
                }

                Destroy(gameObject);
            }
        }
    }
    
    /// <summary>
    /// Missile rigid body.
    /// </summary>
    private Rigidbody rigid_body;

    /// <summary>
    /// Missile floating object.
    /// </summary>
    private FloatingObject floating_object;

    /// <summary>
    /// Target of this missile.
    /// </summary>
    private Ship target;
}
