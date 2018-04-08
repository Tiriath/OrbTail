using UnityEngine;
using System.Collections;

/// <summary>
/// Handles missle movement.
/// </summary>
public class MissileBehavior : MonoBehaviour
{
    /// <summary>
    /// Current missile target.
    /// </summary>
    public GameObject Target { get; set; }

    /// <summary>
    /// Owner of the missile.
    /// </summary>
    public GameObject Owner { get; set; }

    /// <summary>
    /// Set the missile target.
    /// </summary>
    /// <param name="target">Missile target.</param>
    /// <param name="owner">Object who shoot the missile.</param>
    public void SetTarget(GameObject target, GameObject owner)
    {
        Target = target;
        Owner = owner;
    }

    void Start()
    {
        explosion_sfx = Resources.Load<AudioClip>("Sounds/Powers/Explosion");

        fire_timestamp = Time.time;

        explosion_object = Resources.Load<GameObject>(explosion_prefab_path);
    }

    void Update()
    {
        // #TODO Rewrite this!

        var floating = GetComponent<FloatingObject>();

        if (Target != null)
        {
            var direction = (Target.transform.position - this.transform.position).normalized;

            var forward = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * maxMissileSteering, 0).normalized;

            this.transform.rotation = Quaternion.LookRotation(forward, -floating.ArenaDown);
        }

        Vector3 forwardProjected = Vector3.Cross(floating.ArenaDown,
                                                    Vector3.Cross(-floating.ArenaDown, this.transform.forward)
                                                    ).normalized;
        
        forwardProjected = Vector3.Lerp(this.transform.forward, forwardProjected, Time.deltaTime * smoothCurve);

        this.GetComponent<Rigidbody>().AddForce(forwardProjected * maxMissileSpeed, ForceMode.VelocityChange);

        // Timed destruction.

        if (Time.time > fire_timestamp + time_to_live)
        {
            DestroyMissile();
        }
    }

    /// <summary>
    /// Called whenever the missile hits something.
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        GameObject objectCollided = collision.gameObject;

        if (objectCollided.tag == Tags.Ship && objectCollided != Owner)
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * explosionForce, ForceMode.Impulse);

            collision.gameObject.GetComponent<TailController>().DetachDriver.Top().DetachOrbs(int.MaxValue, collision.gameObject.GetComponent<Tail>());

            DestroyMissile();
        }
    }

    /// <summary>
    /// Destroy the missile.
    /// </summary>
    private void DestroyMissile()
    {
        GameObject explosion = GameObject.Instantiate(explosion_object, this.gameObject.transform.position, Quaternion.identity) as GameObject;
        
        AudioSource.PlayClipAtPoint(explosion_sfx, transform.position);

        Target = null;
        GetComponent<Collider>().enabled = false;
        GetComponent<ParticleSystem>().enableEmission = false;
        GetComponent<MeshFilter>().mesh = null;

        Destroy(explosion);
        Destroy(this.gameObject);
    }

    public const string explosion_prefab_path = "Prefabs/Power/Explosion";
    private const float maxMissileSteering = 6.0f;
    private const float maxMissileSpeed = 8.0f;
    private const float explosionForce = 60.0f;

    private const float smoothCurve = 10f;

    /// <summary>
    /// Sound to play when the explosion starts.
    /// </summary>
    private AudioClip explosion_sfx;

    /// <summary>
    /// How many seconds the missile is live for.
    /// </summary>
    private const float time_to_live = 2.5f;

    /// <summary>
    /// Timestamp when the missile was fired.
    /// </summary>
    private float fire_timestamp;

    // Explosion prefab.
    private GameObject explosion_object;
}
