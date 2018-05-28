using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Base class power-ups effect.
/// </summary>
public class PowerUpEffect : NetworkBehaviour
{
    /// <summary>
    /// Duration of this effect.
    /// </summary>
    public float duration = 0.0f;

    /// <summary>
    /// Target game object.
    /// </summary>
    [SyncVar(hook ="OnSyncTarget")]
    public GameObject target;

    /// <summary>
    /// The ship owning this powerup effect.
    /// </summary>
    public Ship Owner { get; set; }

    /// <summary>
    /// The target of this effect.
    /// </summary>
    public Ship Target { get; set; }

    public override void OnStartClient()
    {
        base.OnStartClient();

        timestamp = Time.time;
    }

    public virtual void Update()
    {
        if(duration > 0.0f && (Time.time - timestamp) > duration)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Timestamp when this power was created.
    /// </summary>
    private float timestamp;

    /// <summary>
    /// Called whenever the target of this powerup changes.
    /// </summary>
    private void OnSyncTarget(GameObject ship)
    {
        this.target = ship;
        Target = ship.GetComponent<Ship>();
    }
}