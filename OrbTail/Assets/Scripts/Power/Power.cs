using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Base class for all powers.
/// </summary>
public abstract class Power
{
    public delegate void DeactivatedDelegate(object sender);

    /// <summary>
    /// Event raised whenever the power is deactivated.
    /// </summary>
    public event DeactivatedDelegate OnDeactivatedEvent;

    /// <summary>
    /// Path containing power prefabs.
    /// </summary>
    public const string kPowerPrefabPath = "Prefabs/Power/";

    /// <summary>
    /// Get the power name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Get the group of this power.
    /// </summary>
    public int Group { get; private set; }

    /// <summary>
    /// The owner of the power.
    /// </summary>
    public GameObject Owner { get; private set; }

    /// <summary>
    /// Timestamp when the power was activated.
    /// </summary>
    protected float ActivationTime { get; private set; }

    /// <summary>
    /// Duration of the power in seconds.
    /// Unset if the power has no duration.
    /// </summary>
    protected float? Duration { get; private set; }

    /// <summary>
    /// Get whether the power is ready, as percentage.
    /// </summary>
    public virtual float IsReady
    {
        get
        {
            return 1.0f;
        }
    }

    /// <summary>
    /// Clone this power
    /// </summary>
    public abstract Power Generate();

    /// <summary>
    /// Activate the power.
    /// </summary>
    /// <param name="gameObj">Ship the power is activated on.</param>
    public void Activate(GameObject owner)
    {
        this.Owner = owner;
        this.ActivationTime = Time.time;

        // Activate the power.

        ActivateShared();

        if(NetworkHelper.IsServerSide())
        {
            ActivateServer();
        }

        if (NetworkHelper.IsOwnerSide(Owner.GetComponent<NetworkView>()))
        {
            ActivateClient();
        }
    }
    
    /// <summary>
    /// Deactivate the power.
    /// </summary>
    public virtual void Deactivate()
    {
        // Destroy the VFX.

        if(fx != null)
        {
            fx.transform.parent = null;

            GameObjectFactory.Instance.Destroy(kPowerPrefabPath + Name, fx);
        }

        // Event.

        if (OnDeactivatedEvent != null)
        {
            OnDeactivatedEvent(this);
        };
    }

    /// <summary>
    /// Called at frame-time to update the power status.
    /// </summary>
    public virtual void Update()
    {
        // Destroy the power when the duration expires.

        if (Duration.HasValue && (Time.time >= ActivationTime + Duration.Value))
        {
            Duration = null;

            Deactivate();
        }
    }
    
    /// <summary>
    /// Fire the power if possible.
    /// </summary>
    /// <returns>Returns true if the power could be fired successfully, returns false otherwise.</returns>
    public virtual bool Fire() { return false; }

    /// <summary>
    /// Activate the power.
    /// Called on both server-side and client-side: used to handle cosmetic stuffs (such as VFX).
    /// </summary>
    protected virtual void ActivateShared()
    {
        fx = GameObjectFactory.Instance.Instantiate(kPowerPrefabPath + Name, Owner.transform.position, Quaternion.identity);

        fx.transform.parent = Owner.transform;
    }

    /// <summary>
    /// Activate the power.
    /// Called on server-side: used to modify tail controller.
    /// </summary>
    protected virtual void ActivateServer()
    {

    }

    /// <summary>
    /// Activate the power. 
    /// Called on client-side: used to modify movement controller.
    /// </summary>
    protected virtual void ActivateClient()
    {

    }

    /// <summary>
    /// Create a new power from explicit group, duration and name.
    /// </summary>
    /// <param name="group">Power group.</param>
    /// <param name="duration">Duration of the power, in seconds.</param>
    /// <param name="name">Power name</param>
    protected Power(int group, float? duration, string name)
    {
        this.Group = group;
        this.Duration = duration;
        this.Name = name;
    }

    /// <summary>
    /// VFX object associated to the power.
    /// </summary>
    private GameObject fx;
}