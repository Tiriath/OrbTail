using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// Base class for all powers.
/// </summary>
public class PowerUp : NetworkBehaviour
{
    public delegate void DelegatePower(PowerUp sender);

    /// <summary>
    /// Event raised whenever the powerup is destroyed.
    /// </summary>
    public event DelegatePower DestroyedEvent;

    /// <summary>
    /// Name of the power-up.
    /// </summary>
    public string power_up_name = "Powerup";

    /// <summary>
    /// Minimum seconds between two consecutive usages of this power-up.
    /// </summary>
    public float cooldown = 1.0f;

    /// <summary>
    /// Number of times this power-up can be used for before being destroyed.
    /// </summary>
    public int stacks = 1;

    /// <summary>
    /// Prefab of the effect of this power-up;
    /// </summary>
    public GameObject effect;

    /// <summary>
    /// The ship owning this powerup.
    /// </summary>
    public Ship Owner
    {
        get
        {
            return owner;
        }
        set
        {
            if (owner != value)
            {
                owner = value;

                transform.SetParent(owner.transform);

                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                fire_time = 0;
            }
        }
    }

    /// <summary>
    /// Get the remaining cooldown in seconds.
    /// </summary>
    public float RemainingCooldown
    {
        get
        {
            return cooldown - Mathf.Clamp(Time.time - fire_time, 0.0f, cooldown);
        }
    }

    public void OnDestroy()
    {
        if (DestroyedEvent != null)
        {
            DestroyedEvent(this);
        }
    }

    /// <summary>
    /// Fire the power if possible.
    /// </summary>
    public void Fire()
    {
        if (RemainingCooldown <= 0.0f && isServer)
        {
            fire_time = Time.time;

            // Fire the powerup.

            var power_up_effect = Instantiate(effect, transform.position, transform.rotation).GetComponent<PowerUpEffect>();

            power_up_effect.Owner = owner;

            NetworkServer.Spawn(power_up_effect.gameObject);

            // Reduce the number of stacks and eventually unbind the power.

            --stacks;

            if (stacks <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Timestamp when the power was fired.
    /// </summary>
    private float fire_time;

    /// <summary>
    /// Ship owning this power.
    /// </summary>
    private Ship owner;
}