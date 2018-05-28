using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Component used to control active power-ups on a ship.
/// </summary>
public class PowerController : NetworkBehaviour
{
    public delegate void DelegatePowerEvent(PowerController sender);

    public event DelegatePowerEvent PowerAcquiredEvent;

    /// <summary>
    /// Get the current power-up.
    /// </summary>
    public PowerUp PowerUp { get; private set; }

    public void Start()
    {
        input = GetComponent<InputProxy>();

        GetComponentInChildren<ProximityHandler>().OnProximityEvent += OnProximity;
    }

    public void OnDestroy()
    {
        GetComponentInChildren<ProximityHandler>().OnProximityEvent -= OnProximity;

        if (PowerUp != null)
        {
            OnPowerUpDestroyed(PowerUp);
        }
    }

    public void Update()
    {
        if (isLocalPlayer && input.PowerUpInput)
        {
            CmdFire();
        }
    }

    /// <summary>
    /// Called whenever an object enters the ship proximity field.
    /// Used to activate collected powers.
    /// </summary>
    private void OnProximity(object sender, Collider other)
    {
        if(!isServer)
        {
            return;
        }

        var collectable = other.gameObject.GetComponent<PowerUpCollectable>();

        if (collectable && collectable.IsActive)
        {
            // Deactivate any existing powerup.

            if (PowerUp != null)
            {
                Destroy(PowerUp);
            }

            // Collect a new powerup.

            var power_up = Instantiate(collectable.Collect()).GetComponent<PowerUp>();

            power_up.Owner = GetComponent<Ship>();

            NetworkServer.Spawn(power_up.gameObject);

            RpcOnPowerAcquired(power_up.gameObject);
        }
    }

    /// <summary>
    /// Called on each client when this ship acquires a new power.
    /// </summary>
    [ClientRpc]
    private void RpcOnPowerAcquired(GameObject power_up)
    {
        PowerUp = power_up.GetComponent<PowerUp>();

        PowerUp.DestroyedEvent += OnPowerUpDestroyed;

        if (PowerAcquiredEvent != null)
        {
            PowerAcquiredEvent(this);
        }
    }

    /// <summary>
    /// Fire the active powerup.
    /// </summary>
    [Command]
    private void CmdFire()
    {
        if(PowerUp != null)
        {
            PowerUp.Fire();
        }
    }

    /// <summary>
    /// Called whenever a power on this owner is destroyed.
    /// </summary>
    private void OnPowerUpDestroyed(PowerUp power)
    {
        if(power == PowerUp)
        {
            PowerUp = null;     // This check is needed since when a power is replaced by another one, its destruction won't happen instantly.
        }

        power.DestroyedEvent -= OnPowerUpDestroyed;
    }

    /// <summary>
    /// Used to read user input.
    /// </summary>
    private InputProxy input;
}
