using System;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : Power
{
    private const float power_time = 10.0f;

    public Invincibility() : base(PowerGroups.Main, power_time, "Invincibility") { }

    private IDriver driver;

    protected override void ActivateServer()
    {

        var tail_stack = Owner.GetComponent<TailController>().DetachDriver;

        driver = tail_stack.Push( new InvincibleDetacherDriver());
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

        return new Invincibility();

    }

}
