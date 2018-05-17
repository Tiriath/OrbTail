using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grants a single homing missile that cause massive damage on impact.
/// </summary>
public class Missile : PowerUp
{
    public Missile() 
        : base("Missile", 0)
    {
        this.DropRate = 2;
        this.Duration = 0.0f;
        this.Cooldown = 0.0f;
        this.FireSFX = Resources.Load<AudioClip>("Sounds/Powers/MissileLaunch");

        this.missile_object = Resources.Load<GameObject>("Prefabs/Power/MissileRocket");
    }
    public override PowerUp Generate()
    {
        return new Missile();
    }

    protected override void OnFired()
    {
        var missile_location = Owner.transform.position + Owner.transform.forward * offset;
        var missile_rotation = Owner.transform.rotation;

        GameObject missile = (Network.peerType == NetworkPeerType.Disconnected) ?
            GameObject.Instantiate(missile_object, missile_location, missile_rotation) as GameObject :
            Network.Instantiate(missile_object, missile_location, missile_rotation, 0) as GameObject;

        missile.GetComponent<Rigidbody>().AddForce(Owner.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);

        missile.GetComponent<MissileBehavior>().SetTarget(FindTarget(Owner), Owner);

        Deactivate();
    }

    private GameObject FindTarget(GameObject owner)
    {
        float min_distance = float.MaxValue;
        GameObject nearest_ship = null;

        foreach (GameObject ship in GameObject.FindGameObjectsWithTag(Tags.Ship))
        {
            if (ship != Owner)
            {
                var distance = (ship.transform.position - Owner.transform.position).sqrMagnitude;

                if (distance < min_distance)
                {
                    nearest_ship = ship;
                    min_distance = distance;
                }
            }
        }

        return nearest_ship;
    }

    // Forward missile offset.
    private const float offset = 6f;

    // Missile prefab.
    private GameObject missile_object;
}
