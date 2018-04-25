﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Represents a player configuration in a lobby.
/// </summary>
public class LobbyPlayer : NetworkLobbyPlayer
{
    public delegate void DelegatePlayerJoined(LobbyPlayer sender);
    public delegate void DelegatePlayerLeft(LobbyPlayer sender);

    public delegate void DelegatePlayerIndexChanged(LobbyPlayer sender);
    public delegate void DelegatePlayerShipChanged(LobbyPlayer sender);
    public delegate void DelegatePlayerReady(LobbyPlayer sender);

    public delegate void DelegatePlayerScore(LobbyPlayer sender);

    public static event DelegatePlayerJoined PlayerJoinedEvent;
    public event DelegatePlayerLeft PlayerLeftEvent;

    public event DelegatePlayerIndexChanged PlayerIndexChangedEvent;
    public event DelegatePlayerShipChanged PlayerShipChangedEvent;
    public event DelegatePlayerReady PlayerReadyEvent;

    public event DelegatePlayerScore PlayerScoreEvent;

    /// <summary>
    /// Player name.
    /// </summary>
    [SyncVar]
    public string player_name = "anon";

    /// <summary>
    /// Player ship.
    /// </summary>
    [SyncVar(hook = "OnSyncPlayerShip")]
    public string player_ship = null;

    /// <summary>
    /// Player index, relative to the match.
    /// </summary>
    [SyncVar(hook = "OnSyncPlayerIndex")]
    public int player_index = -1;

    /// <summary>
    /// Whether this player is human.
    /// </summary>
    [SyncVar(hook = "OnSyncIsHuman")]
    public bool is_human = true;

    /// <summary>
    /// Current player score. Has different meaning according to the current game mode.
    /// </summary>
    [SyncVar(hook = "OnSyncScore")]
    public int score = 0;

    /// <summary>
    /// Get the color associated to this player.
    /// The color depends on the player index only.
    /// </summary>
    public Color Color
    {
        get
        {
            return player_index < 0 ? kDefaultPlayerColor : kPlayerColors[player_index];
        }
    }

    /// <summary>
    /// Called on server and client when the player joins the lobby.
    /// </summary>
    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();

        if(PlayerJoinedEvent != null)
        {
            PlayerJoinedEvent(this);
        }
    }

    /// <summary>
    /// Called when the player gets disconnected from the lobby.
    /// </summary>
    public void OnDestroy()
    {
        if (player_indexes != null)                      // This is true only on the server.
        {
            player_indexes.Push(this.player_index);
        }

        if (PlayerLeftEvent != null)
        {
            PlayerLeftEvent(this);
        }
    }

    /// <summary>
    /// Called on the local version of this behaviour.
    /// </summary>
    public override void OnStartAuthority()
    {
        Debug.Log("OnStartAuthority (controller: " + playerControllerId + ")");

        var player_configuration = GameLobby.Instance.GetLocalPlayer(playerControllerId);

        player_ship = player_configuration.ship_prefab.name;
        is_human = player_configuration.is_human;
        score = 0;

        CmdAcquirePlayerIndex();                        // Ask a free player index to the server.
    }

    /// <summary>
    /// Called on clients when the lobby player switches between ready and not ready.
    /// </summary>
    /// <param name="ready_state"></param>
    public override void OnClientReady(bool ready_state)
    {
        base.OnClientReady(ready_state);

        if(PlayerReadyEvent != null)
        {
            PlayerReadyEvent(this);
        }
    }

    /// <summary>
    /// Called whenever the player ship is synced.
    /// </summary>
    /// <param name="player_ship">Selected ship name.</param>
    private void OnSyncPlayerShip(string player_ship)
    {
        this.player_ship = player_ship;

        if(PlayerShipChangedEvent != null)
        {
            PlayerShipChangedEvent(this);
        }
    }

    /// <summary>
    /// Called whenever the player index is synced.
    /// </summary>
    /// <param name="player_index">New player index.</param>
    private void OnSyncPlayerIndex(int player_index)
    {
        this.player_index = player_index;

        if(PlayerIndexChangedEvent != null)
        {
            PlayerIndexChangedEvent(this);
        }
    }

    /// <summary>
    /// Called whenever the player identity is synced.
    /// </summary>
    /// <param name="is_human">Whether the player is human or AI.</param>
    private void OnSyncIsHuman(bool is_human)
    {
        this.is_human = is_human;
    }

    /// <summary>
    /// Called whenever the player score is synced.
    /// </summary>
    /// <param name="score">New score.</param>
    private void OnSyncScore(int score)
    {
        this.score = score;

        if(PlayerScoreEvent != null)
        {
            PlayerScoreEvent(this);
        }
    }

    /// <summary>
    /// Acquire an unique player index for this lobby.
    /// Called on the client, executed on the server.
    /// </summary>
    [Command]
    private void CmdAcquirePlayerIndex()
    {
        if(player_indexes == null)
        {
            // Fill the list with all the available indexes.

            player_indexes = new Stack<int>();

            for(int index = GameLobby.Instance.maxPlayers - 1; index >= 0; --index)
            {
                player_indexes.Push(index);
            }
        }

        // Fetch the first free index in the list.

        this.player_index = player_indexes.Pop();
    }

    /// <summary>
    /// List of available player indexes.
    /// </summary>
    private static Stack<int> player_indexes;

    /// <summary>
    /// Default color for player without an index.
    /// </summary>
    private static readonly Color kDefaultPlayerColor = Color.white;

    /// <summary>
    /// List of colors associated to each player index.
    /// </summary>
    private static readonly Color[] kPlayerColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };
}
