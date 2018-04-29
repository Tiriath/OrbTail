using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Default attacher that just attaches each provided orb.
/// </summary>
public class DefaultAttacherDriver : BaseDriver, IAttacherDriver
{
    public List<GameObject> AttachOrbs(List<GameObject> orbs, Func<GameObject, bool> attacher)
    {
        var attached_orbs = new List<GameObject>();

        foreach (var orb in orbs)
        {
            if (attacher(orb))
            {
                attached_orbs.Add(orb);
            }
        }

        return attached_orbs;
    }
}
