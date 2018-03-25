using UnityEngine;
using System.Collections;

/// <summary>
/// Driver determining how much damage a ship absorbs while colling with another ship.
/// </summary>
public interface IDefenceDriver : IDriver
{
    /// <summary>
    /// Gets the defence value of the ship.
    /// </summary>
    /// <returns>Returns the defence value of the ship.</returns>
    int GetDefence();

    /// <summary>
    /// Get the number of orbs lost by the ship as a result of an impact with another ship.
    /// </summary>
    /// <returns>Returns the number of orbs lost by the ship owning this driver as result of an impact with another ship.</returns>
    /// <param name="damage">Damage suffered by this ship.</param>
    int DamageToOrbs(float damage);
}
