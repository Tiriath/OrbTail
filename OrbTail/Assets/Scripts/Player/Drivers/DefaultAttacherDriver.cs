using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Default attacher that just attaches each provided orb.
/// </summary>
public class DefaultAttacherDriver : BaseDriver, IAttacherDriver
{
    public bool AttachOrb(GameObject orb, Func<GameObject, bool> attacher)
    {
        return attacher(orb);
    }
}
