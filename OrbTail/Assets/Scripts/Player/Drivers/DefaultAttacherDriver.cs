using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Default attacher that just attaches orbs to a tail.
/// </summary>
public class DefaultAttacherDriver : BaseDriver, IAttacherDriver
{
    public void AttachOrbs(GameObject orb, Tail tail)
    {
        tail.AttachOrb(orb);
    }

    public void AttachOrbs(List<GameObject> orbs, Tail tail)
    {
        foreach(GameObject orb in orbs)
        {
            AttachOrbs(orb, tail);
        }
    }
}
