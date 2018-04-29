using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// A driver used to determine how orbs are detached from a ship.
/// </summary>
public interface IDetacherDriver : IDriver
{
    /// <summary>
    /// Execute a detach function a number of times.
    /// </summary>
    /// <param name="count">Number of orbs to detach..</param>
    /// <param name="detacher">Detacher function.</param>
    /// <returns>Returns the list of orbs detached.</returns>
    List<GameObject> DetachOrbs(int count, Func<GameObject> detacher);
}
