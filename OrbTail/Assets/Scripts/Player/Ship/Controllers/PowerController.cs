﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Component used to control active powers on a ship.
/// </summary>
public class PowerController : MonoBehaviour
{
    public delegate void DelegatePowerAttached(object sender, GameObject ship, Power power);

    /// <summary>
    /// Called whenever a new power is activated on this ship.
    /// </summary>
    public event DelegatePowerAttached OnPowerAttachedEvent;

    public void Start()
    {
        input = GetComponent<InputProxy>();
        
        GetComponentInChildren<ProximityHandler>().OnProximityEvent += OnProximity;
    }

    public void Update()
    {
        // Update powers.
        // #TODO Avoid frame-time new.

        foreach (var power in new List<Power>(powers.Values))
        {
            power.Update();
        }

        // Only the owner of the power can shoot it.
        // #TODO Input should not be handled here.

        if(input.Fire && powers.ContainsKey(PowerGroups.Main))
        {
            powers[PowerGroups.Main].Fire();
        }

        if(input.FireSpecial && powers.ContainsKey(PowerGroups.Passive))
        {
            powers[PowerGroups.Passive].Fire();
        }
    }

    /// <summary>
    /// Activate a new power.
    /// </summary>
    /// <param name="power">Power to activate</param>
    public void AddPower(Power power)
    {
        // Deactivate any previous power in the same group.

        Power old_power = null;

        if (powers.TryGetValue(power.Group, out old_power))
        {
            old_power.Deactivate();
        }

        // Activate and add the new power.

        powers.Add(power.Group, power);

        power.OnDeactivatedEvent += OnPowerDeactivated;

        power.Activate(gameObject);

        // Event.

        if (OnPowerAttachedEvent != null)
        {
            OnPowerAttachedEvent(this, gameObject, power);
        }
    }

    /// <summary>
    /// Called whenever an object enters the ship proximity field.
    /// Used to activate collected powers.
    /// </summary>
    private void OnProximity(object sender, Collider other)
    {
        var orb_controller = other.gameObject.GetComponent<OrbController>();

        if(orb_controller != null && orb_controller.GetImbuedPower() != null)
        {
            AddPower(orb_controller.GetImbuedPower());

            orb_controller.ImbuePower(null);
        }
    }

    /// <summary>
    /// Called whenever an active power is deactivated.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="group">Power group</param>
    private void OnPowerDeactivated(object sender)
    {
        var power = (Power)sender;

        powers.Remove(power.Group);
    }

    /// <summary>
    /// List of active powers, indexed by power group.
    /// Powers in the same group overwrite each other.
    /// </summary>
    private Dictionary<int, Power> powers = new Dictionary<int, Power>();

    private InputProxy input;
}
