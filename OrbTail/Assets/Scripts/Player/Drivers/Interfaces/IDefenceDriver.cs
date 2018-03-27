using UnityEngine;
using System.Collections;

/// <summary>
/// Driver used to determine the amount of orbs lost as a consequence of an impact.
/// </summary>
public interface IDefenceDriver : IDriver
{
    /// <summary>
    /// Gets the defence value of the ship.
    /// </summary>
    /// <returns>Returns the defence value of the ship.</returns>
    float GetDefence();

    /// <summary>
    /// Get the number of orbs lost by the ship as a result of a damaging event.
    /// </summary>
    /// <returns>Returns the number of orbs lost by the ship owning this driver as result of the provided damage.</returns>
    /// <param name="damage">Damage suffered by this ship.</param>
    int ReceiveDamage(float damage);
}
