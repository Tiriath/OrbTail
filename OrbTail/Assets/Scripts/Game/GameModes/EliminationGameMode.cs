using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The last player standing wins the game. If the player has no more orbs it gets eliminated
/// </summary>
public class EliminationGameMode : BaseGameMode
{
    protected override void OnMatchSetup()
    {
        base.OnMatchSetup();

        if(isServer)
        {
            var orbs = new List<GameObject>(FindObjectsOfType<OrbController>().Select(controller => controller.gameObject));

            var orb_count = orbs.Count / ships.Count;

            foreach (Ship ship in ships)
            {
                ship.AttachOrb(orbs.GetRange(ship.player_index * orb_count, orb_count));
            }
        }
    }

    protected override void OnMatchEnd()
    {
        base.OnMatchEnd();

        // #TODO The winner is the last ship standing (or tie if none).
    }

    /// <summary>
    /// Called whenever a ship loses one or more orbs.
    /// </summary>
    private void OnOrbChanged(Ship ship, List<GameObject> orbs)
    {
        ship.LobbyPlayer.score = ship.TailLength;

        if(isServer && ship.TailLength == 0)
        {
            // #TODO Destroy the ship.
        }

        // #TODO When the number of players drops to less than 1 the match ends.
    }

    protected override void OnShipCreated(Ship ship)
    {
        base.OnShipCreated(ship);

        ship.OrbDetachedEvent += OnOrbChanged;
        ship.OrbAttachedEvent += OnOrbChanged;
    }

    protected override void OnShipDestroyed(Ship ship)
    {
        base.OnShipDestroyed(ship);

        ship.OrbDetachedEvent -= OnOrbChanged;
        ship.OrbAttachedEvent -= OnOrbChanged;
    }
}

