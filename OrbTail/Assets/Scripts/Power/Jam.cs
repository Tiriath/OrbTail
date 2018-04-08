﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grants a debuff that inverts steering control for a limited amount of time.
/// </summary>
public class Jam : Power
{
    public Jam() 
        : base("Jam", PowerGroups.Jam)
    {
        this.DropRate = 1;
        this.Duration = 7.0f;
        this.Cooldown = 0.0f;
        this.FireSFX = null;
    }

    public override Power Generate()
    {
        return new Jam();
    }

    protected override void OnActivated()
    {
        base.OnActivated();

        var steer_driver = Owner.GetComponent<MovementController>().GetSteerDriver();

        driver = steer_driver.Push(new InvertedSteerDriver(steer_driver.GetDefaultDriver().GetMaxSteer(), steer_driver.GetDefaultDriver().GetSteerSmooth()));
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
