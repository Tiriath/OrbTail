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

        var orbs = new Queue<GameObject>(GameObject.FindGameObjectsWithTag(Tags.Orb));
        var ships = new List<GameObject>(GameObject.FindGameObjectsWithTag(Tags.Ship));

        int orbs_per_player = orbs.Count / ships.Count;

        foreach (GameObject ship in ships)
        {
            var ship_component = ship.GetComponent<Ship>();

            ship_component.OrbDetachedEvent += OnOrbChanged;
            ship_component.OrbAttachedEvent += OnOrbChanged;

            if (hasAuthority)
            {
                for (int i = 0; i < orbs_per_player; i++)
                {
                    ship_component.AttachOrb(orbs.Dequeue());
                }
            }
        }

        // Initialize player scores.

        foreach (LobbyPlayer lobby_player in GameLobby.Instance.lobbySlots)
        {
            lobby_player.score = orbs_per_player;
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
        // #TODO Change player score.
        // #TODO Remove the player when the orb count drops to 0.
        // #TODO When the number of players drops to less than 1 the match ends.
    }
}

