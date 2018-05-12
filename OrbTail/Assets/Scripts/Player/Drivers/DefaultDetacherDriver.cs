using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Default detacher that just detaches each provided orb.
/// </summary>
public class DefaultDetacherDriver : BaseDriver, IDetacherDriver
{
    public GameObject DetachOrb(Func<GameObject> detacher)
    {
        return detacher();
    }
}