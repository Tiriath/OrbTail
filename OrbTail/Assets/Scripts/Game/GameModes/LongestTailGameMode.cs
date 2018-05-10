using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player with the longest tail at the end of the match wins
/// </summary>
public class LongestTailGameMode : BaseGameMode
{
    protected override void OnMatchEnd()
    {
        base.OnMatchEnd();

        // #TODO The winner is the ship with the longest tail.
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

    /// <summary>
    /// Called whenever a ship orb count changes.
    /// </summary>
    private void OnOrbChanged(Ship ship, List<GameObject> orbs)
    {
        var player = (LobbyPlayer)GameLobby.Instance.lobbySlots[ship.player_index];

        player.score = ship.TailLength;
    }
}

