﻿using UnityEngine;
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
    /// Duration of the power in seconds.
    /// </summary>
    public float Duration { get; protected set; }

    /// <summary>
    /// Cooldown of the power in seconds.
    /// </summary>
    public float Cooldown { get; protected set; }

    /// <summary>
    /// SFX clip played when the power is fired.
    /// </summary>
    public AudioClip FireSFX { get; protected set; }

    /// <summary>
    /// Whether the power is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Get whether the power can be fired, as percentage.
    /// </summary>
    public float IsReady
    {
        get
        {
            return Cooldown > 0.0f ? Mathf.Clamp01((Time.time - FireTime) / Cooldown) : 1.0f;       // Assumes powers without a cooldown are always ready.
        }
    }

    /// <summary>
    /// Clone this power.
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
        this.FireTime = Time.time;

        OnActivated(NetworkHelper.IsServerSide(), NetworkHelper.IsOwnerSide(Owner.GetComponent<NetworkView>()));

        IsActive = true;
    }
    
    /// <summary>
    /// Deactivate the power.
    /// </summary>
    public void Deactivate()
    {
        // Deactivate the power.

        OnDeactivated(NetworkHelper.IsServerSide(), NetworkHelper.IsOwnerSide(Owner.GetComponent<NetworkView>()));

        // Event.

        if (OnDeactivatedEvent != null)
        {
            OnDeactivatedEvent(this);
        };

        IsActive = false;
    }

    /// <summary>
    /// Fire the power if possible.
    /// </summary>
    /// <returns>Returns true if the power could be fired successfully, returns false otherwise.</returns>
    public bool Fire()
    {
        if (IsReady >= 1.0f && IsActive)
        {
            FireTime = Time.time;

            OnFired(NetworkHelper.IsServerSide(), NetworkHelper.IsOwnerSide(Owner.GetComponent<NetworkView>()));

            return true;
        }

        return false;
    }

    /// <summary>
    /// Called at frame-time to update the power status.
    /// </summary>
    public virtual void Update()
    {
        // Destroy the power when the duration expires (for powers having a duration).

        if (IsActive && Duration > 0.0f && (Time.time >= ActivationTime + Duration))
        {
            Deactivate();
        }
    }

    /// <summary>
    /// Timestamp when the power was activated.
    /// </summary>
    protected float ActivationTime { get; private set; }

    /// <summary>
    /// Timestamp when the power was fired.
    /// </summary>
    protected float FireTime { get; private set; }

    /// <summary>
    /// The owner of the power.
    /// </summary>
    protected GameObject Owner { get; private set; }

    /// <summary>
    /// Called whenever the power is activated.
    /// @param is_server_side Whether the activation happened on the server-side.
    /// @param is_owner_side Whether the activation happened on the owner-side.
    /// </summary>
    protected virtual void OnActivated(bool is_server_side, bool is_owner_side)
    {
        vfx = GameObjectFactory.Instance.Instantiate(kPowerPrefabPath + Name, Owner.transform.position, Quaternion.identity);

        vfx.transform.parent = Owner.transform;
    }

    /// <summary>
    /// Called whenever the power is deactivated.
    /// @param is_server_side Whether the deactivation happened on the server-side.
    /// @param is_owner_side Whether the deactivation happened on the owner-side.
    /// </summary>
    protected virtual void OnDeactivated(bool is_server_side, bool is_owner_side)
    {
        if (vfx != null)
        {
            vfx.transform.parent = null;

            GameObjectFactory.Instance.Destroy(kPowerPrefabPath + Name, vfx);
        }
    }

    /// <summary>
    /// Called whenever the power is fired.
    /// @param is_server_side Whether the firing happened on the server-side.
    /// @param is_owner_side Whether the firing happened on the owner-side.
    /// </summary>
    protected virtual void OnFired(bool is_server_side, bool is_owner_side)
    {
        if (FireSFX)
        {
            AudioSource.PlayClipAtPoint(FireSFX, Owner.gameObject.transform.position, 0.2f);
        }
    }

    /// <summary>
    /// Create a new power.
    /// </summary>
    /// <param name="group">Power group.</param>
    /// <param name="name">Power name</param>
    protected Power(string name, int group)
    {
        this.Group = group;
        this.Name = name;
        this.IsActive = false;
    }

    /// <summary>
    /// VFX object associated to the power.
    /// </summary>
    private GameObject vfx;
}