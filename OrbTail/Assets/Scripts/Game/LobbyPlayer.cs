using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Represents a player status.
/// </summary>
public class LobbyPlayer : NetworkLobbyPlayer
{
    public delegate void DelegateLobbyPlayerEvent(LobbyPlayer sender);

    public static event DelegateLobbyPlayerEvent PlayerJoinedEvent;
    public static event DelegateLobbyPlayerEvent LocalPlayerJoinedEvent;

    public event DelegateLobbyPlayerEvent PlayerLeftEvent;
    public event DelegateLobbyPlayerEvent PlayerIndexChangedEvent;
    public event DelegateLobbyPlayerEvent PlayerShipChangedEvent;
    public event DelegateLobbyPlayerEvent PlayerReadyEvent;
    public event DelegateLobbyPlayerEvent PlayerScoreEvent;

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
    /// Called when the player gets disconnected from the lobby.
    /// </summary>
    public void OnDestroy()
    {
        if (PlayerLeftEvent != null)
        {
            PlayerLeftEvent(this);
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
    /// Called on the local version of this behavior.
    /// </summary>
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        Debug.Log("OnStartAuthority (controller: " + playerControllerId + ")");

        var player_configuration = GameLobby.Instance.GetLocalPlayer(playerControllerId);

        CmdConfigureLobbyPlayer(player_configuration.ship_prefab.name, player_configuration.is_human);

        if(LocalPlayerJoinedEvent != null)
        {
            LocalPlayerJoinedEvent(this);
        }
    }

    /// <summary>
    /// Called on clients when the lobby player switches between ready and not ready.
    /// </summary>
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

        if(!is_human)
        {
            SendReadyToBeginMessage();                          // The AI is born ready!
        }
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
    private void CmdConfigureLobbyPlayer(string ship_prefab, bool is_human)
    {
        var game_lobby = GameLobby.Instance;

        for (int index = 0; index < game_lobby.maxPlayers; ++index)
        {
            if(game_lobby.lobbySlots[index] == this)
            {
                this.player_index = index;              // Set the player index.
                break;
            }
        }
        
        this.player_ship = ship_prefab;                 // Set the ship prefab.
        this.is_human = is_human;                       // Set player identity.
    }

    /// <summary>
    /// Default color for player without an index.
    /// </summary>
    private static readonly Color kDefaultPlayerColor = Color.white;

    /// <summary>
    /// List of colors associated to each player index.
    /// </summary>
    private static readonly Color[] kPlayerColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };
}
