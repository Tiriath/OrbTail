using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grants a debuff that inverts steering control for a limited amount of time.
/// </summary>
public class Jam : PowerUp
{
    public Jam() 
        : base("Jam", 0)
    {
        this.DropRate = 1;
        this.Duration = 7.0f;
        this.Cooldown = 0.0f;
        this.FireSFX = null;
    }

    public override PowerUp Generate()
    {
        return new Jam();
    }

    protected override void OnActivated()
    {
        base.OnActivated();

        var steer_driver = Owner.GetComponent<MovementController>().SteerDriver;

        driver = steer_driver.Push(new InvertedSteerDriver(steer_driver.Top()));
    }

    protected override void OnDeactivated()
    {
        if (driver != null)
        {
            driver.Deactivate();
        }

        base.OnDeactivated();
    }

    private IDriver driver;
}
