﻿using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A homing missile that lock on a target.
/// </summary>
public class HomingMissile : Projectile
{
    /// <summary>
    /// Maximum missile steering.
    /// </summary>
    public float steering = 6.0f;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        // Lock onto the nearest target.

        float min_distance = float.MaxValue;

        foreach (Ship ship in FindObjectsOfType<Ship>())
        {
            if (ship != Owner)
            {
                var distance = (ship.transform.position - Owner.transform.position).sqrMagnitude;

                if (distance < min_distance)
                {
                    target = ship.gameObject;
                    min_distance = distance;
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();

        // Adjust missile direction.

        if (Target != null)
        {
            var direction = (Target.transform.position - transform.position).normalized;

            var forward = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * steering, 0).normalized;

            transform.rotation = Quaternion.LookRotation(forward, floating_object.Up);
        }
    }
}
