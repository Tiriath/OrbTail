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
    /// Execute an attach function to an orb.
    /// </summary>
    /// <param name="orb">Orb to attach.</param>
    /// <param name="attacher">Attacher function.</param>
    /// <returns>Returns true if the orb could be attached, returns false otherwise.</returns>
    bool AttachOrb(GameObject orb, Func<GameObject, bool> attacher);
}
