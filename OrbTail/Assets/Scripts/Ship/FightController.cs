using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// Controls ship fights.
/// </summary>
public class FightController : NetworkBehaviour
{
    public delegate void DelegateFight(GameObject attacker, GameObject defender);

    public event DelegateFight FightEvent;

    /// <summary>
    /// The offence power of the ship, as multiplicative factor of dealt damage.
    /// </summary>
    public float offence = 1.0f;

    /// <summary>
    /// The defence power of the ship, as damage required to detach each orb.
    /// </summary>
    public float defence = 15.0f;

    public void Awake()
    {
        ship = GetComponent<Ship>();
        floating_object = GetComponent<FloatingObject>();
    }

    /// <summary>
    /// Called whenever the ship hits another ship.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (hasAuthority)
        {
            // This ship is the defender, whereas the other is the attacker. This event is triggered on both ships simultaneously with reversed roles.

            var attacker = collision.gameObject.GetComponent<FightController>();

            if (attacker)
            {
                var damage = attacker.GetDamage(collision);

                //Debug.Log(gameObject.name + " received " + damage + " damage");

                CmdReceiveDamage(damage);
            }
        }

        if (FightEvent != null)
        {
            FightEvent(collision.gameObject, this.gameObject);
        }
    }

    /// <summary>
    /// Get the damage dealt by this ship as a result of a collision result.
    /// </summary>
    private float GetDamage(Collision collision)
    {
        var ship_forward = floating_object.Forward;

        // Assumes the ship can only deal damage with its frontal part.

        var damage = 0.0f;

        foreach (var contact_point in collision.contacts)
        {
            var impact_direction = (contact_point.point - transform.position).normalized;

            var impact_scale = Mathf.Max(0.0f, Vector3.Dot(impact_direction, ship_forward));        // A frontal hit causes more damage than a glancing one.

            damage += collision.relativeVelocity.magnitude * impact_scale;
        }

        return damage * offence;
    }

    /// <summary>
    /// Receive some damage and detach orbs as result.
    /// </summary>
    [Command]
    private void CmdReceiveDamage(float damage)
    {
        var lost_orbs = Mathf.FloorToInt(damage / defence);

        ship.DetachOrbs(lost_orbs);
    }

    /// <summary>
    /// Ship this controller refers to.
    /// </summary>
    private Ship ship;

    /// <summary>
    /// Floating object associated to the ship.
    /// </summary>
    private FloatingObject floating_object;
}
