using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Default detacher that just detaches orbs from a tail.
/// </summary>
public class DefaultDetacherDriver : BaseDriver, IDetacherDriver
{
    public DefaultDetacherDriver()
    {
        detach_timestamp = Time.time;
    }

    public List<GameObject> DetachOrbs(int amount, Tail tail)
    {
        if (amount == 0 || (Time.time - detach_timestamp < cooldown))
        {
            return new List<GameObject>();
        }

        detach_timestamp = Time.time;

        return tail.DetachOrbs(amount);
    }

    /// <summary>
    /// Last time the tail lost some orbs.
    /// </summary>
    private float detach_timestamp = 1.0f;

    /// <summary>
    /// Number of seconds after detaching some orbs in which the tail cannot lose further orbs.
    /// </summary>
    private const float cooldown = 1.0f;
}
