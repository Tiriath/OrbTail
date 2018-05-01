using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The player with the longest tail at the end of the match wins
/// </summary>
public class LongestTailGameMode : BaseGameMode
{
    protected override void OnMatchSetup()
    {
        base.OnMatchSetup();

        var ships = new List<GameObject>(GameObject.FindGameObjectsWithTag(Tags.Ship));

        foreach (GameObject ship in ships)
        {
            var ship_component = ship.GetComponent<Ship>();

            ship_component.OrbDetachedEvent += OnOrbChanged;
            ship_component.OrbAttachedEvent += OnOrbChanged;

            //ship.GetComponent<GameIdentity>().SetScore(0);

        }

        // Reset player scores.

        foreach (LobbyPlayer lobby_player in GameLobby.Instance.lobbySlots)
        {
            lobby_player.score = 0;
        }
    }

    protected override void OnMatchEnd()
    {
        base.OnMatchEnd();

        // #TODO The winner is the ship with the longest tail.
    }

    /// <summary>
    /// Called whenever a ship orb count changes.
    /// </summary>
    private void OnOrbChanged(Ship ship, List<GameObject> orbs)
    {
        // #TODO Change player score.
    }

}

