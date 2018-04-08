using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grants a temporary boost in speed which propels the ship forward.
/// </summary>
public class Boost : Power
{
    /// <summary>
    /// Create a new instance.
    /// </summary>
    public Boost()
        : base("Boost", PowerGroups.Passive)
    {
        this.DropRate = 0;
        this.Duration = 0.0f;
        this.Cooldown = 5.0f;
        this.FireSFX = Resources.Load<AudioClip>("Sounds/Powers/Boost");
    }
    public override Power Generate()
    {
        return new Boost();
    }

    /// <summary>
    /// Activate boost on ship.
    /// </summary>
    protected override void OnFired()
    {
        base.OnFired();

        Owner.GetComponent<Rigidbody>().AddForce(Owner.transform.forward * boost_force, ForceMode.Impulse);
    }

    /// <summary>
    /// Force applied to the ship when activated.
    /// </summary>
    private const float boost_force = 60.0f;
}