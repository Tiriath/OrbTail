using System;
using System.Collections.Generic;
using UnityEngine;

public class Jam : Power
{
    private const float power_time = 7.0f;

    public Jam() : base(PowerGroups.Jam, power_time, "Jam") { }

    private IDriver driver;
    
    protected override void ActivateClient()
    {
        var steer_driver = Owner.GetComponent<MovementController>().GetSteerDriver();

        driver = steer_driver.Push( new InvertedSteerDriver(steer_driver.GetDefaultDriver().GetMaxSteer(), steer_driver.GetDefaultDriver().GetSteerSmooth() ));
    }

    public override void Deactivate()
    {
        base.Deactivate();

        if (driver != null)
        {
            driver.Deactivate();
        }
    }
    
    public override float IsReady { get { return 0.0f; } }

    public override Power Generate()
    {
        return new Jam();
    }

}
