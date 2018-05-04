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

    protected override void OnMatchEnd()
    {
        base.OnMatchEnd();
        
        // #TODO The winner is the one with the highest score.
    }

    private void OnOrbAttached(Ship ship, List<GameObject> orbs)
    {
        ship.LobbyPlayer.score += kOrbAttachedScore;
    }

    protected override void OnShipCreated(Ship ship)
    {
        base.OnShipCreated(ship);

        ship.OrbAttachedEvent += OnOrbAttached;
    }

    protected override void OnShipDestroyed(Ship ship)
    {
        base.OnShipDestroyed(ship);

        ship.OrbAttachedEvent -= OnOrbAttached;
    }
}

