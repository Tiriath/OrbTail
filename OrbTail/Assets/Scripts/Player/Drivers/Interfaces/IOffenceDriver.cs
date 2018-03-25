using UnityEngine;
using System.Collections;

/// <summary>
/// Driver determining how much damage a ship deals to another one when colliding.
/// </summary>
public interface IOffenceDriver : IDriver
{
    /// <summary>
    /// Gets the offence value of the ship.
    /// </summary>
    /// <returns>Returns the offence value of the ship.</returns>
    int GetOffence();

    /// <summary>
    /// Calculate the damage dealt by the ship owning this driver to the provided defender ship.
    /// </summary>
    /// <returns>Returns the calculated damage.</returns>
    /// <param name="defender">The defender ship.</param>
    /// <param name="collision">Collision between the attacker and the defender ship.</param>
    float GetDamage(GameObject defender, Collision collision);
}
