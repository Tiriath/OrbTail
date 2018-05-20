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

        ship.OrbDetachedEvent += OnOrbDetached;
        ship.OrbAttachedEvent += OnOrbAttached;
    }

    protected override void OnShipDestroyed(Ship ship)
    {
        base.OnShipDestroyed(ship);

        ship.OrbDetachedEvent -= OnOrbDetached;
        ship.OrbAttachedEvent -= OnOrbAttached;
    }

    /// <summary>
    /// Called whenever a ship acquires an orb.
    /// </summary>
    private void OnOrbAttached(Ship ship, GameObject orb)
    {
        ship.LobbyPlayer.score = ship.TailLength;
    }

    /// <summary>
    /// Called whenever a ship loses one or more orbs.
    /// </summary>
    private void OnOrbDetached(Ship ship, List<GameObject> orbs)
    {
        ship.LobbyPlayer.score = ship.TailLength;
    }
}

