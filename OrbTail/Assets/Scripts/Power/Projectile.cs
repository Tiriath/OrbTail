using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A propelled projectile which deals damage upon impact.
/// </summary>
public class Projectile : PowerUpEffect
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
    /// Projectile forward speed.
    /// </summary>
    public float speed = 8.0f;

    /// <summary>
    /// Number of orbs detached upon impact.
    /// </summary>
    public int damage = 3;

    public void OnDestroy()
    {
        if (explosion)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        rigid_body = GetComponent<Rigidbody>();
        floating_object = GetComponent<FloatingObject>();
    }

    public override void Update()
    {
        base.Update();

        rigid_body.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Called whenever the missile hits something.
    /// </summary>
    void OnTriggerEnter(Collider collision)
    {
        if (isServer)
        {
            var ship = collision.gameObject.GetComponent<Ship>();

            if (ship && ship != Owner)
            {
                if(!ship.GetComponent<Bubble>())
                {
                    ship.GetComponent<Rigidbody>().AddForce(transform.forward * explosion_force, ForceMode.VelocityChange);

                    ship.DetachOrbs(damage);
                }

                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Missile rigid body.
    /// </summary>
    protected Rigidbody rigid_body;

    /// <summary>
    /// Missile floating object.
    /// </summary>
    protected FloatingObject floating_object;
}
