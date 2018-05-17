using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// Component used to control active power-ups on a ship.
/// </summary>
public class PowerController : NetworkBehaviour
{
    public delegate void DelegatePower(PowerController sender, PowerUp power);

    public event DelegatePower PowerAcquiredEvent;

    public void Start()
    {
        input = GetComponent<InputProxy>();

        GetComponentInChildren<ProximityHandler>().OnProximityEvent += OnProximity;
    }

    public void Update()
    {
        if(input.PowerUpInput && power_up != null)
        {
            // #TODO Activate the power-up
        }
    }

    /// <summary>
    /// Called whenever an object enters the ship proximity field.
    /// Used to activate collected powers.
    /// </summary>
    private void OnProximity(object sender, Collider other)
    {
        var collectable = other.gameObject.GetComponent<PowerUpCollectable>();

        if(collectable)
        {
            // #TODO Collect the power up.

            if (PowerAcquiredEvent != null)
            {
                //PowerAcquiredEvent(this, power);
            }
        }
    }

    /// <summary>
    /// Power-up ready to be used.
    /// </summary>
    private PowerUp power_up;

    /// <summary>
    /// Used to read user input.
    /// </summary>
    private InputProxy input;
}
