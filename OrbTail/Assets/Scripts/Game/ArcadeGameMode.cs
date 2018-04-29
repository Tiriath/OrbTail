using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The player with the higher score at the end of the match wins
/// </summary>
public class ArcadeGameMode : BaseGameMode
{
    /// <summary>
    /// Reward for each attached orb.
    /// </summary>
    public const int kOrbAttachedScore = 10;

    protected override void OnMatchSetup() 
    {
        base.OnMatchSetup();

        // #TODO Attach to Tail's OnEventOrbAttached.

        foreach (GameObject ship in GameObject.FindGameObjectsWithTag(Tags.Ship))
        {
            ship.GetComponent<Ship>().OrbAttachedEvent += OnOrbAttached;
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

        // #TODO The winner is the one with the highest score.
    }

    private void OnOrbAttached(Ship ship, List<GameObject> orbs)
    {
        // #TODO Change player score.
    }
}

