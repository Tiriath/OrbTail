﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Orbs that are detached by this ship as a result of a fight are stolen directly.
/// </summary>
public class OrbSteal : PowerUpEffect
{
//    private List<FightController> fight_controllers;

//    public OrbSteal() 
//        : base("OrbSteal")
//    {
//        this.Duration = 10.0f;
//    }

//    protected override void OnActivated()
//    {
//        base.OnActivated();

//        fight_controllers = new List<FightController>();

////         foreach (GameObject ship in GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>().ShipsInGame)
////         {
////             FightController tailController = ship.GetComponent<FightController>();
////             tailController.OnEventFight += OnFight;
////             tailControllers.Add(tailController);
////         }
//    }

//    protected override void OnDeactivated()
//    {
//        foreach (FightController tailController in fight_controllers)
//        {
//            tailController.FightEvent -= OnFight;
//        }

//        base.OnDeactivated();
//    }

//    /// <summary>
//    /// Called whenever a fight between this ship and another one occurs.
//    /// </summary>
//    void OnFight(GameObject attacker, GameObject defender, IList<GameObject> orbs)
//    {
//        if (attacker == Owner)
//        {
//            //attacker.GetComponent<Ship>().AttachOrb(orbs);
//        }
//    }
}
