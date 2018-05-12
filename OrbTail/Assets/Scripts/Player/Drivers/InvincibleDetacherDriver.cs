using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Detacher that prevents any orbs from being detached.
/// Can be used to model the effect of a barrier or invincibility.
/// </summary>
public class InvincibleDetacherDriver : BaseDriver, IDetacherDriver
{
    public GameObject DetachOrb(Func<GameObject> detacher)
    {
        return null;
    }
}
