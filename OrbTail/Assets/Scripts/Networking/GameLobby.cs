﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to create and handle the game lobby.
/// </summary>
public class GameLobby : NetworkLobbyManager
{
    public delegate void DelegateLobbyStarted(GameLobby sender);
    public delegate void DelegateLobbyStop(GameLobby sender);
    
    public event DelegateLobbyStarted LobbyStarted;
    public event DelegateLobbyStop LobbyStop;

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
    /// Maximum number of connection attempts before giving up.
    /// </summary>
    public int max_attempts = 8;

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
    /// Disconnect from the lobby.
    /// If the lobby is hosted it will be destroyed.
    /// </summary>
    public void DisconnectLobby()
    {
        lobbyScene = null;                                                          // Prevent the application from reloading the lobby scene again.
        playScene = null;

        if(!is_host)
        {
            StopClient();                                                           // Client.
        }
        else if(match_id.HasValue)
        {
            matchMaker.DestroyMatch(match_id.Value, 0, OnDestroyMatch);             // Online host.
        }
        else
        {
            StopHost();                                                             // Offline host.
        }
    }

    /// <summary>
    /// Called on the server when server\host is started.
    /// </summary>
    public override void OnLobbyStartServer()
    {
        Debug.Log("OnLobbyStartServer");

        is_host = true;
    }

    /// <summary>
    /// Called on the client when connected to a server.
    /// </summary>
    public override void OnClientConnect(NetworkConnection connection)
    {
        Debug.Log("OnClientConnect");

        base.OnClientConnect(connection);

        // Add local players after the manager finished initializing the new client.
        // For some reason if this is performed while on OnLobbyClientEnter it would attempt to create one more lobby player, failing.

        AddLocalPlayers();
    }

    /// <summary>
    /// Called on the client whenever the list of online matches is available.
    /// </summary>
    public override void OnMatchList(bool success, string extended_info, List<MatchInfoSnapshot> match_list)
    {
        this.match_list = new Stack<MatchInfoSnapshot>(match_list);

        TryConnectToMatch();
    }

    /// <summary>
    /// Called on the host whenever a new match is created.
    /// </summary>
    public override void OnMatchCreate(bool success, string extended_info, MatchInfo match_info)
    {
        Debug.Log("OnMatchCreate");

        if (!success)
        {
            Debug.LogError("Could not create an online match.");
        }

        base.OnMatchCreate(success, extended_info, match_info);

        match_id = match_info.networkId;
    }

    /// <summary>
    /// Called on the client whenever a new match is joined.
    /// </summary>
    public override void OnMatchJoined(bool success, string extended_info, MatchInfo match_info)
    {
        Debug.Log("OnMatchJoined");

        base.OnMatchJoined(success, extended_info, match_info);

        match_id = match_info.networkId;

        if(!success)
        {
            TryConnectToMatch();            // Process the next candidate match.
        }
    }

    /// <summary>
    /// Called whenever the match is destroyed on the server.
    /// </summary>
    public override void OnDestroyMatch(bool success, string extended_info)
    {
        Debug.Log("OnDestroyMatch");

        Debug.Assert(success, "Could not destroy the match.");

        base.OnDestroyMatch(success, extended_info);

        StopMatchMaker();
        StopHost();

        match_id = null;
    }

    /// <summary>
    /// Called on the server when all the players in the lobby are ready.
    /// </summary>
    public override void OnLobbyServerPlayersReady()
    {
        // Select the arena to start.

        var arena = game_configuration.arena;

        if(arena.Length == 0)
        {
            // Randomize an arena.

        }

        ServerChangeScene(arena);
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

        original_lobby_scene = lobbyScene;
    }

    /// <summary>
    /// Called whenever a new scene is loaded.
    /// </summary>
    /// <param name="scene">Scene being loaded.</param>
    /// <param name="mode">Scene loading mode.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == original_lobby_scene)
        {
            lobbyScene = original_lobby_scene;
            playScene = original_lobby_scene;       // Any valid scene will do, this is only needed to prevent some validation errors. OnLobbyServerPlayersReady will take care of the actual arena to load.

            is_host = false;

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
        networkAddress = "localhost";

        StartHost();
    }

    /// <summary>
    /// Join an existing lobby matching the current game configuration.
    /// </summary>
    private void SearchLobby()
    {
        StartMatchMaker();

        matchMaker.ListMatches(0, max_attempts, "", true, 0, 0, OnMatchList);
    }

    /// <summary>
    /// Attempt to connect to the next online match.
    /// If no other match is available create a new lobby.
    /// </summary>
    private void TryConnectToMatch()
    {
        if(match_list.Count == 0)
        {
            CreateOnlineLobby();
        }
        else
        {
            var next_match = match_list.Pop();

            matchMaker.JoinMatch(next_match.networkId, "", "", "", 0, 0, OnMatchJoined);
        }
    }

    /// <summary>
    /// Create an online lobby for other players to join via unity's matchmaking.
    /// </summary>
    private void CreateOnlineLobby()
    {
        StartMatchMaker();

        matchMaker.CreateMatch("OrbtailMatch", (uint) maxPlayers, true, "", "", "", 0, 0, OnMatchCreate);
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

    /// <summary>
    /// Whether this lobby is acting as a host.
    /// </summary>
    bool is_host = false;

    /// <summary>
    /// Current match ID.
    /// </summary>
    NetworkID? match_id;

    /// <summary>
    /// List of online matches to connect to.
    /// </summary>
    Stack<MatchInfoSnapshot> match_list;

    /// <summary>
    /// The original lobby scene.
    /// </summary>
    string original_lobby_scene;
}
