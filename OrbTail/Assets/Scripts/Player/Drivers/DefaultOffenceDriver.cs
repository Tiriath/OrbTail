using UnityEngine;
using System.Collections;

/// <summary>
/// Driver used to calculate standard damage when two ships collide.
/// </summary>
public class DefaultOffenceDriver : BaseDriver, IOffenceDriver
{
    public DefaultOffenceDriver(float offence)
    {
        this.offence = offence;
    }

    public float GetOffence()
    {
        return offence;
    }

    public float GetDamage(Collision collision)
    {
        var ship = collision.gameObject;                                                                // Ship that caused the impact.

        var ship_forward = ship.GetComponent<FloatingObject>().Forward;

        // Assumes the ship can only deal damage with its frontal part.

        var damage = 0.0f;

        foreach(var contact_point in collision.contacts)
        {
            var impact_direction = (contact_point.point - ship.transform.position).normalized;

            var impact_scale = Mathf.Max(0.0f, Vector3.Dot(impact_direction, ship_forward));            // A frontal hit causes more damage than a glancing one.

            var impact_magnitude = collision.relativeVelocity.magnitude;

            damage += impact_magnitude * impact_scale;
        }

        return damage * offence;
    }

    /// <summary>
    /// Ship offence value.
    /// </summary>
    private float offence = 1.0f;
}
