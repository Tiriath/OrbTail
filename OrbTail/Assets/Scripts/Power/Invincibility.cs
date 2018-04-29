using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grants immunity to collisions. Prevents any orb from being detached for a limited amount of time.
/// </summary>
public class Invincibility : Power
{
    public Invincibility() 
        : base("Invincibility", PowerGroups.Main)
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
    
    public override Power Generate()
    {
        return new Invincibility();
    }
}
