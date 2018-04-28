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
            var tail = ship.GetComponent<Tail>();

            tail.OnEventOrbDetached += OnOrbDetached;
            tail.OnEventOrbAttached += OnOrbAttached;

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
    /// Called whenever a ship loses one or more orbs.
    /// </summary>
    private void OnOrbDetached(object sender, GameObject ship, int count)
    {
        // #TODO Change player score.
    }

    /// <summary>
    /// Called whenever a ship acquires an orb.
    /// </summary>
    private void OnOrbAttached(object sender, GameObject orb, GameObject ship)
    {
        // #TODO Change player score.
    }

}

