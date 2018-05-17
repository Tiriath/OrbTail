using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grants immunity to collisions. Prevents any orb from being detached for a limited amount of time.
/// </summary>
public class Invincibility : PowerUp
{
    public Invincibility() 
        : base("Invincibility", 0)
    {
        this.DropRate = 2;
        this.Duration = 10.0f;
        this.Cooldown = 0.0f;
        this.FireSFX = null;
    }

    private IDriver driver;

    protected override void OnActivated()
    {
        base.OnActivated();

        driver = Owner.GetComponent<Ship>().DetachDriver.Push(new InvincibleDetacherDriver());
    }

    protected override void OnDeactivated()
    {
        if (driver != null)
        {
            driver.Deactivate();
        }

        base.OnDeactivated();
    }
    
    public override PowerUp Generate()
    {
        return new Invincibility();
    }
}
