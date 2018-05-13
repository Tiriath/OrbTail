using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// The last player standing wins the game. If the player has no more orbs it gets eliminated
/// </summary>
public class EliminationGameMode : BaseGameMode
{
    /// <summary>
    /// Initial amount of lives for each player.
    /// </summary>
    public int lives = 5;

    /// <summary>
    /// Prefab used for spectating the match.
    /// </summary>
    public GameObject spectator_prefab;

    protected override void OnMatchCountdown()
    {
        base.OnMatchCountdown();

        if (isServer)
        {
            foreach(var ship in ships)
            {
                for(int orb_index = 0; orb_index < lives; ++orb_index)
                {
                    ship.RpcAttachOrb(orbs[ship.player_index * lives + orb_index].gameObject);
                }
            }
        }
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
    /// Called whenever a ship acquires or loses one orb.
    /// </summary>
    private void OnOrbChanged(Ship ship, GameObject orb)
    {
        ship.LobbyPlayer.score = ship.TailLength;

        if(isServer && ship.TailLength == 0)
        {
            var connection = ship.LobbyPlayer.connectionToClient;

            Destroy(ship.gameObject);

            // Activate the spectator on the killed player.

            if(spectator_prefab != null)
            {
                var spectator = Instantiate<GameObject>(spectator_prefab);

                NetworkServer.ReplacePlayerForConnection(connection, spectator_prefab, ship.LobbyPlayer.playerControllerId);
            }
        }
    }
}

