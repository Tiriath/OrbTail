using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player with the longest tail at the end of the match wins
/// </summary>
public class LongestTailGameMode : BaseGameMode
{
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

    /// <summary>
    /// Called whenever a ship acquires or loses one orb.
    /// </summary>
    private void OnOrbChanged(Ship ship, GameObject orb)
    {
        ship.LobbyPlayer.score = ship.TailLength;
    }
}

