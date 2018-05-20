using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prevents any orb from being detached from the ship when this power is active.
/// </summary>
public class Invincibility : PowerUpEffect
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        transform.SetParent(Owner.transform);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
