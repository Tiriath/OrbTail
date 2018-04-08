using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Orbs that are detached by this ship as a result of a fight are stolen directly.
/// </summary>
public class OrbSteal : Power
{
    private List<TailController> tailControllers;

    public OrbSteal() 
        : base("OrbSteal", PowerGroups.Main)
    {
        this.DropRate = 2;
        this.Duration = 10.0f;
        this.Cooldown = 0.0f;
        this.FireSFX = null;
    }
    public override Power Generate()
    {
        return new OrbSteal();
    }

    protected override void OnActivated()
    {
        base.OnActivated();

        tailControllers = new List<TailController>();

        foreach (GameObject ship in GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>().ShipsInGame)
        {
            TailController tailController = ship.GetComponent<TailController>();
            tailController.OnEventFight += OnFight;
            tailControllers.Add(tailController);
        }
    }

    protected override void OnDeactivated()
    {
        foreach (TailController tailController in tailControllers)
        {
            tailController.OnEventFight -= OnFight;
        }

        base.OnDeactivated();
    }

    /// <summary>
    /// Called whenever a fight between this ship and another one occurs.
    /// </summary>
    void OnFight(object sender, IList<GameObject> orbs, GameObject attacker, GameObject defender)
    {
        if (attacker == Owner)
        {
            foreach (GameObject orb in orbs)
            {
                if (!orb.GetComponent<OrbController>().IsLinked)
                {
                    attacker.GetComponent<TailController>().AttachDriver.Top().AttachOrbs(orb, attacker.GetComponent<Tail>());
                }
            }
        }
    }
}
