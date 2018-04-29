using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Default detacher that just detaches each provided orb.
/// </summary>
public class DefaultDetacherDriver : BaseDriver, IDetacherDriver
{
    public List<GameObject> DetachOrbs(int count, Func<GameObject> detacher)
    {
        var orbs = new List<GameObject>();

        for (; count > 0; --count)
        {
            var orb = detacher();

            if (orb != null)
            {
                orbs.Add(orb);
            }
        }

        return orbs;
    }
}