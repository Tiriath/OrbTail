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

    /// <summary>
    /// Called whenever an orb is attached to a ship.
    /// </summary>
    private void OnOrbAttached(Ship ship, GameObject orb)
    {
        ship.LobbyPlayer.score += kOrbAttachedScore;
    }
}

