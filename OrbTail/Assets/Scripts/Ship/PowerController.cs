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
        if(input.PowerUpInput && PowerUp != null)
        {
            PowerUp.Fire();
        }
    }

    /// <summary>
    /// Called whenever an object enters the ship proximity field.
    /// Used to activate collected powers.
    /// </summary>
    private void OnProximity(object sender, Collider other)
    {
        var collectable = other.gameObject.GetComponent<PowerUpCollectable>();

        if (collectable && collectable.IsActive)
        {
            // Deactivate any existing powerup.

            if (PowerUp != null)
            {
                Destroy(PowerUp);
            }

            // Collect a new powerup.

            PowerUp = Instantiate(collectable.Collect()).GetComponent<PowerUp>();

            PowerUp.DestroyedEvent += OnPowerUpDestroyed;

            PowerUp.Owner = GetComponent<Ship>();

            NetworkServer.Spawn(PowerUp.gameObject);

            if (PowerAcquiredEvent != null)
            {
                PowerAcquiredEvent(this);
            }
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
