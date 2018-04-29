using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// A driver used to determine how orbs are attached to a ship.
/// </summary>
public interface IAttacherDriver : IDriver
{
    /// <summary>
    /// Execute an attach function for each orb in the list.
    /// </summary>
    /// <param name="orbs">Orbs to attach.</param>
    /// <param name="attacher">Attacher function.</param>
    /// <returns>Returns the list of orbs detached.</returns>
    List<GameObject> AttachOrbs(List<GameObject> orbs, Func<GameObject, bool> attacher);
}
