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
    [SyncVar(hook = "OnSyncOwner")]
    public GameObject owner;

    public Ship Owner { get; private set; }

    /// <summary>
    /// The target of this effect.
    /// </summary>
    public Ship Target { get; private set; }

    /// <summary>
    /// Called whenever the client starts for this instance.
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();

        OnSyncTarget(target);
        OnSyncOwner(owner);

        timestamp = Time.time;
    }

    public virtual void Update()
    {
        if(isServer && duration > 0.0f && (Time.time - timestamp) > duration)
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
    private void OnSyncTarget(GameObject target)
    {
        this.target = target;

        if(target)
        {
            Target = target.GetComponent<Ship>();
        }
    }

    /// <summary>
    /// Called whenever the owner of this powerup changes.
    /// </summary>
    private void OnSyncOwner(GameObject owner)
    {
        this.owner = owner;

        if(owner)
        {
            Owner = owner.GetComponent<Ship>();
        }
    }
}