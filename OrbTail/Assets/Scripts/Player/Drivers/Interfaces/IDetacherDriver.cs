using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// A driver used to determine how orbs are detached from a ship.
/// </summary>
public interface IDetacherDriver : IDriver
{
    /// <summary>
    /// Execute a detach function on an attached orb.
    /// </summary>
    /// <param name="detacher">Detacher function.</param>
    /// <returns>Returns the detached orb, if any.</returns>
    GameObject DetachOrb(Func<GameObject> detacher);
}
