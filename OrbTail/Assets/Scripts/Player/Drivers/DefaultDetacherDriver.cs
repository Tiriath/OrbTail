using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Default detacher that just detaches orbs from a tail.
/// </summary>
public class DefaultDetacherDriver : BaseDriver, IDetacherDriver
{
    public List<GameObject> DetachOrbs(int amount, Tail tail)
    {
        return tail.DetachOrbs(amount);
    }
}
