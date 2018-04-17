using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to create and handle the game lobby.
/// </summary>
public class GameLobby : NetworkLobbyManager
{
    public delegate void DelegateLobbyStarted(GameLobby sender);
    
    public event DelegateLobbyStarted LobbyStarted;

    /// <summary>
    /// Get the singleton instance.
    /// </summary>
    public static GameLobby Instance
    {
        get
        {
            if(game_lobby == null)
            {
                game_lobby = FindObjectOfType<GameLobby>();
            }

            return game_lobby;
        }
    }

    /// <summary>
    /// Check whether the lobby is offline.
    /// </summary>
    public bool IsOffline
    {
        get
        {
            return game_configuration.game_type == GameType.Offline;
        }
    }

    /// <summary>
    /// Get a local player configuration from index.
    /// </summary>
    public PlayerConfiguration GetLocalPlayer(int local_player_index)
    {
        return local_players[local_player_index];
    }

    /// <summary>
    /// Called on the client when connected to a server.
    /// </summary>
    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);

        // Add local players after the manager finished initializing the new client.
        // For some reason if this is performed while on OnLobbyClientEnter it would attempt to create one more lobby player, failing.

        AddLocalPlayers();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        game_configuration = GetComponent<GameConfiguration>();
    }

    /// <summary>
    /// Called whenever a new scene is loaded.
    /// </summary>
    /// <param name="scene">Scene being loaded.</param>
    /// <param name="mode">Scene loading mode.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == lobbyScene)
        {
            CreateLobby();
        }
    }

    /// <summary>
    /// Create the lobby.
    /// </summary>
    private void CreateLobby()
    {
        if (LobbyStarted != null)
        {
            LobbyStarted(this);
        }

        if (IsOffline)
        {
            CreateOfflineLobby();       // Offline games are just hosted games where other players are AIs.
        }
        else
        {
            SearchLobby();              // Attempt to join an existing lobby, create a brand new one when none is found.
        }
    }

    /// <summary>
    /// Create an offline lobby.
    /// </summary>
    private void CreateOfflineLobby()
    {
        StartHost();
    }

    /// <summary>
    /// Join an existing lobby matching the current game configuration.
    /// </summary>
    private void SearchLobby()
    {
        // #TODO Assumes no lobby was found, create a new lobby.
        StartClient();

        //CreateOnlineLobby();
    }

    /// <summary>
    /// Create an online lobby for other players to join.
    /// </summary>
    private void CreateOnlineLobby()
    {
        // Advertise the match via the matchmaking service.

        //StartMatchMaker();

        //matchMaker.CreateMatch("OrbtailMatch", 4, true, "", "", "", 0, 0, OnMatchCreate);
    }

    /// <summary>
    /// Add local players to the current match (either hosted or joined).
    /// </summary>
    private void AddLocalPlayers()
    {
        local_players = new List<PlayerConfiguration>(GetComponents<PlayerConfiguration>());

        for (short index = 0; index < local_players.Count; ++index)
        {
            TryToAddPlayer();
        }
    }

    /// <summary>
    /// Singleton instance of the game lobby.
    /// </summary>
    private static GameLobby game_lobby;

    /// <summary>
    /// Current game configuration component.
    /// </summary>
    private GameConfiguration game_configuration = null;

    /// <summary>
    /// List of local players.
    /// </summary>
    private List<PlayerConfiguration> local_players;
}
