using UnityEngine;
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
        
        if (NetworkHelper.IsServerSide())
        {
            GetComponentInChildren<ProximityHandler>().OnProximityEvent += OnProximity;
        }
    }

    public void Awake()
    {
        powers = new Dictionary<int, Power>();
    }

    public void Update()
    {
        // Update powers.
        // #TODO Avoid frame-time new.

        foreach (Power power in new List<Power>(powers.Values))
        {
            power.Update();
        }

        // Only the owner of the power can shoot it.
        // #TODO Input should not be handled here.

        var network_view = GetComponent<NetworkView>();

        if (NetworkHelper.IsOwnerSide(network_view))
        {
            Power power;

            foreach (int group in input.FiredPowerUps.Where((int g) => { return powers.ContainsKey(g); }))
            {
                // Fire the power and notify the others.

                power = powers[group];

                if (power.Fire() && Network.peerType != NetworkPeerType.Disconnected)
                {
                    network_view.RPC("RPCFirePower", RPCMode.Others, power.Name);
                }
            }
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
        
        if(powers.TryGetValue(power.Group, out old_power))
        {
            old_power.Deactivate();
        }

        // Activate and add the new power.

        powers.Add(power.Group, power);

        power.EventDestroyed += OnPowerDeactivated;

        power.Activate(gameObject);

        // Relay the call.

        if (Network.isServer)
        {
            GetComponent<NetworkView>().RPC("RPCAddPower", RPCMode.Others, power.Name);
        }

        // Event.

        if (OnPowerAttachedEvent != null)
        {
            OnPowerAttachedEvent(this, gameObject, power);
        }
    }

    /// <summary>
    /// Add a new power by name.
    /// RPC call.
    /// </summary>
    /// <param name="power_name">Name of the power to activate.</param>
    [RPC]
    public void RPCAddPower(string power_name)
    {
        var power = PowerFactory.Instance.PowerFromName(power_name);

        AddPower(power);
    }

    /// <summary>
    /// Fire an active power by name (for activated powers).
    /// RPC call.
    /// </summary>
    /// <param name="power_name">Name of the power to fire.</param>
    [RPC]
    public void RPCFirePower(string power_name)
    {
        var group = PowerFactory.Instance.GroupFromName(power_name);

        powers[group].Fire();
    }

    /// <summary>
    /// Called whenever an object enters the ship proximity field.
    /// Used to activate collected powers.
    /// </summary>
    private void OnProximity(object sender, Collider other)
    {
        var game_object = other.gameObject;

        if (game_object.tag == Tags.Orb && game_object.GetComponent<RandomPowerAttacher>().enabled)
        {
            AddPower(PowerFactory.Instance.RandomPower);
        }
    }

    /// <summary>
    /// Called whenever an active power is deactivated.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="group">Power group</param>
    private void OnPowerDeactivated(object sender, int group)
    {
        powers.Remove(group);
    }

    /// <summary>
    /// List of active powers, indexed by power group.
    /// Powers in the same group overwrite each other.
    /// </summary>
    private Dictionary<int, Power> powers;

    private InputProxy input;
}
