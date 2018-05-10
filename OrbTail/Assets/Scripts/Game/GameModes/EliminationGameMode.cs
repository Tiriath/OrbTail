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
    /// <summary>
    /// Initial amount of lives for each player.
    /// </summary>
    public int lives = 5;

    protected override void OnMatchEnd()
    {
        base.OnMatchEnd();

        // #TODO The winner is the last ship standing (or tie if none).
    }

    protected override void OnShipCreated(Ship ship)
    {
        base.OnShipCreated(ship);

        ship.OrbDetachedEvent += OnOrbChanged;
        ship.OrbAttachedEvent += OnOrbChanged;

        if (isServer)
        {
            ship.AttachOrb(new List<GameObject>(orbs.GetRange(ship.player_index * lives, lives).Select(orb => orb.gameObject)));
        }
    }

    protected override void OnShipDestroyed(Ship ship)
    {
        base.OnShipDestroyed(ship);

        ship.OrbDetachedEvent -= OnOrbChanged;
        ship.OrbAttachedEvent -= OnOrbChanged;
    }

    /// <summary>
    /// Called whenever a ship loses one or more orbs.
    /// </summary>
    private void OnOrbChanged(Ship ship, List<GameObject> orbs)
    {
        var player = (LobbyPlayer)GameLobby.Instance.lobbySlots[ship.player_index];

        player.score = ship.TailLength;

        if(isServer && ship.TailLength == 0)
        {
            Destroy(ship);
        }
    }
}

