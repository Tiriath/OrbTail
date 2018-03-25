using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Detacher that prevents any orbs from being detached from a tail.
/// Can be used to model the effect of a barrier or invincibility.
/// </summary>
public class InvincibleDetacherDriver : BaseDriver, IDetacherDriver
{
    public List<GameObject> DetachOrbs(int amount, Tail tail)
    {
        return new List<GameObject>();
    }
}
