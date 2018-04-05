using UnityEngine;
using System.Collections;

/// <summary>
/// Inflate the proximity radius of the ship in order to collect orbs more easily. #TODO Not an actual magnet, limited utility.
/// </summary>
public class Magnet : Power
{
    public Magnet() 
        : base("Magnet", PowerGroups.Main)
    {
        this.Duration = 10.0f;
        this.Cooldown = 0.0f;
        this.FireSFX = null;
    }

    public override Power Generate()
    {
        return new Magnet();
    }

    protected override void OnActivated(bool is_server_side, bool is_owner_side)
    {
        base.OnActivated(is_server_side, is_owner_side);

        if (is_server_side)
        {
            proximity = Owner.GetComponentInChildren<ProximityHandler>();
            proximity.Radius *= radius_factor;
        }
    }

    protected override void OnDeactivated(bool is_server_side, bool is_owner_side)
    {
        if(is_server_side)
        {
            proximity.Radius /= radius_factor;
        }

        base.OnDeactivated(is_server_side, is_owner_side);
    }

    private const float radius_factor = 5.0f;

    private ProximityHandler proximity;
}
