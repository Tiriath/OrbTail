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
    float GetOffence();

    /// <summary>
    /// Calculate the damage dealt by the ship as a result of a collision.
    /// </summary>
    /// <returns>Returns the calculated damage.</returns>
    /// <param name="collision">Collision data.</param>
    float GetDamage(Collision collision);
}
