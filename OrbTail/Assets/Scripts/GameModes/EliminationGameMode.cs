using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    ship.AttachOrb(orbs[ship.player_index * lives + orb_index].gameObject);
                }
            }
        }
    }

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

        // Activate the spectator on the killed player.

        if (spectator_prefab != null && ship.LobbyPlayer.is_human && ship.isLocalPlayer)
        {
            var spectator = Instantiate<GameObject>(spectator_prefab).GetComponent<Spectator>();

            spectator.LobbyPlayer = ship.LobbyPlayer;

            // Attach the follow camera to the spectator.

            foreach(var camera in FindObjectsOfType<FollowCamera>().Where(follow_camera => (follow_camera.LobbyPlayer == ship.LobbyPlayer)))
            {
                camera.ViewTarget = spectator.gameObject;
            }
            
            // Reassign HUD ownership.

            foreach(var hud in FindObjectsOfType<HUDHandler>().Where(hud_handler => (hud_handler.LobbyPlayer == ship.LobbyPlayer)))
            {
                hud.Owner = spectator.gameObject;
            }
        }

        base.OnShipDestroyed(ship);
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

        if (isServer && ship.TailLength == 0)
        {
            Destroy(ship.gameObject);
        }
    }
}

