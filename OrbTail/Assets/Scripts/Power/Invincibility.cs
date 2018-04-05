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

    protected override void OnActivated(bool is_server_side, bool is_owner_side)
    {
        base.OnActivated(is_server_side, is_owner_side);

        if(is_server_side)
        {
            var tail_stack = Owner.GetComponent<TailController>().DetachDriver;

            driver = tail_stack.Push( new InvincibleDetacherDriver() );
        }
    }

    protected override void OnDeactivated(bool is_server_side, bool is_owner_side)
    {
        if (is_server_side && driver != null)
        {
            driver.Deactivate();
        }

        base.OnDeactivated(is_server_side, is_owner_side);
    }
    
    public override Power Generate()
    {
        return new Invincibility();
    }
}
