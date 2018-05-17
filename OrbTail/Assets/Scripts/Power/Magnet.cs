using UnityEngine;
using System.Collections;

/// <summary>
/// Inflate the proximity radius of the ship in order to collect orbs more easily. #TODO Not an actual magnet, limited utility.
/// </summary>
public class Magnet : PowerUp
{
    public Magnet() 
        : base("Magnet", 0)
    {
        this.DropRate = 2;
        this.Duration = 10.0f;
        this.Cooldown = 0.0f;
        this.FireSFX = null;
    }

    public override PowerUp Generate()
    {
        return new Magnet();
    }

    protected override void OnActivated()
    {
        base.OnActivated();

        proximity = Owner.GetComponentInChildren<ProximityHandler>();
        proximity.Radius *= radius_factor;
    }

    protected override void OnDeactivated()
    {
        proximity.Radius /= radius_factor;

        base.OnDeactivated();
    }

    private const float radius_factor = 5.0f;

    private ProximityHandler proximity;
}
