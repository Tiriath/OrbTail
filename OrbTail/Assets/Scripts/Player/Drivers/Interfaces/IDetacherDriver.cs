using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A driver used to determine how orbs are detached from a ship.
/// </summary>
public interface IDetacherDriver : IDriver
{
    /// <summary>
    /// Detach a given amount of orbs from a tail.
    /// The number of detached orbs is always less or equal than the requested amount.
    /// </summary>
    /// <returns>Returns the orbs detached this way.</returns>
    /// <param name="amount">Number of the orbs to detach.</param>
    /// <param name="tail">The tail which we detach orbs from.</param>
    List<GameObject> DetachOrbs(int amount, Tail tail);
}
