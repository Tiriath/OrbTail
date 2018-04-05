using System;
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
        this.Duration = 7.0f;
        this.Cooldown = 0.0f;
        this.FireSFX = null;
    }

    public override Power Generate()
    {
        return new Jam();
    }

    protected override void OnActivated(bool is_server_side, bool is_owner_side)
    {
        base.OnActivated(is_server_side, is_owner_side);

        if (is_owner_side)
        {
            var steer_driver = Owner.GetComponent<MovementController>().GetSteerDriver();

            driver = steer_driver.Push(new InvertedSteerDriver(steer_driver.GetDefaultDriver().GetMaxSteer(), steer_driver.GetDefaultDriver().GetSteerSmooth()));
        }
    }

    protected override void OnDeactivated(bool is_server_side, bool is_owner_side)
    {
        if (is_owner_side && driver != null)
        {
            driver.Deactivate();
        }

        base.OnDeactivated(is_server_side, is_owner_side);
    }

    private IDriver driver;
}
