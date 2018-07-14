using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

/// <summary>
/// Script used to create and handle the game lobby.
/// </summary>
public class GameLobby : NetworkLobbyManager
{
    public delegate void DelegateLobbyEvent(GameLobby sender);
    
    public event DelegateLobbyEvent LobbyStarted;

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
    /// Maximum connection attempts to perform before giving up.
    /// </summary>
    public int max_connection_attempts = 8;
    
    /// <summary>
    /// Delay before adding a new AI to the match when every other human is ready, in seconds.
    /// </summary>
    public float ai_join_delay = 1.0f;

    /// <summary>
    /// List of ship prefabs.
    /// </summary>
    public GameObject[] ship_prefabs;

    /// <summary>
    /// Prefab used to start the match after a short countdown.
    /// </summary>
    public GameObject countdown_prefab;

    /// <summary>
    /// Get a local player configuration from index.
    /// </summary>
    public PlayerConfiguration GetLocalPlayer(short controller_id)
    {
        var players = local_players.Where(local_player => local_player.player_controller_id == controller_id).ToArray();

        return players.Length == 0 ? null : players[0];
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
        else if(match_info != null)
        {
            matchMaker.DestroyMatch(match_info.networkId, 0, OnDestroyMatch);       // Online host.

            match_info = null;

            StopMatchMaker();
            StopHost();
        }
        else
        {
            StopHost();                                                             // Offline host.
        }
    }

    /// <summary>
    /// This is called on the server when the server is started - including when a host is started.
    /// </summary>

    public override void OnLobbyStartServer()
    {
        LobbyPlayer.PlayerJoinedEvent += OnPlayerJoined;
        LobbyPlayer.PlayerLeftEvent += OnPlayerLeft;
    }

    /// <summary>
    /// Called whenever the server is stopped.
    /// </summary>
    public override void OnStopServer()
    {
        LobbyPlayer.PlayerJoinedEvent -= OnPlayerJoined;
        LobbyPlayer.PlayerLeftEvent -= OnPlayerLeft;

        base.OnStopServer();
    }

    /// <summary>
    /// Called on the client when connected to a server.
    /// </summary>
    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);

        AddLocalPlayers();
    }

    /// <summary>
    /// Called on clients when disconnected from a server.
    /// </summary>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Clear();

        base.OnClientDisconnect(conn);
    }

    /// <summary>
    /// Called on the server when a client disconnects.
    /// </summary>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }

    /// <summary>
    /// Called on the client whenever the list of online matches is available.
    /// </summary>
    public override void OnMatchList(bool success, string extended_info, List<MatchInfoSnapshot> match_list)
    {
        this.match_list = new Stack<MatchInfoSnapshot>(match_list);

        ++match_list_page;

        if(match_list.Count == 0)
        {
            connection_attempts = 0;            // No compatible match found, exhaust connection attempts so a new lobby can be created on this client.
        }

        TryConnectToMatch();
    }

    /// <summary>
    /// Called on the host whenever a new match is created.
    /// </summary>
    public override void OnMatchCreate(bool success, string extended_info, MatchInfo match_info)
    {
        Debug.Log("OnMatchCreate");

        this.match_info = match_info;

        if (!success)
        {
            Debug.LogError("Could not create an online match.");
            this.match_info = null;
        }
        
        base.OnMatchCreate(success, extended_info, match_info);

        if(success)
        {
            PostStartServer();
        }
    }

    /// <summary>
    /// Called on the client whenever a new match is joined.
    /// </summary>
    public override void OnMatchJoined(bool success, string extended_info, MatchInfo match_info)
    {
        Debug.Log("OnMatchJoined");

        base.OnMatchJoined(success, extended_info, match_info);

        --connection_attempts;

        this.match_info = match_info;

        if (!success)
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

        match_info = null;
    }

    /// <summary>
    /// Called on the server when all the players in the lobby are ready.
    /// </summary>
    public override void OnLobbyServerPlayersReady()
    {
        if(countdown_prefab)
        {
            var countdown = Instantiate(countdown_prefab, Vector3.zero, Quaternion.identity).GetComponent<LobbyCountdown>();

            countdown.TimeOutEvent += OnCountdown;

            NetworkServer.Spawn(countdown.gameObject);
        }
        else
        {
            OnCountdown(null);
        }
    }

    /// <summary>
    /// Get the game player associated the connection.
    /// </summary>
    /// <returns>Returns the game player object to assign to the player.</returns>
    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection connection, short player_controller_id)
    {
        Debug.Log("OnLobbyServerCreateGamePlayer");

        var start_position = GetStartPosition();

        foreach(LobbyPlayer lobby_player in lobbySlots)
        {
            if(lobby_player.connectionToClient == connection && lobby_player.playerControllerId == player_controller_id)
            {
                // Find a matching prefab inside the list of registered prefabs.

                foreach(var spawn_prefab in spawnPrefabs)
                {
                    if(spawn_prefab.name == lobby_player.player_ship)
                    {
                        // Spawn the ship and link it to its lobby player.

                        var ship = (GameObject)Instantiate(spawn_prefab, start_position.position, start_position.rotation);

                        ship.GetComponent<Ship>().player_index = lobby_player.player_index;

                        return ship;
                    }
                }
            }
        }
        
        return null;
    }

    /// <summary>
    /// This is called on the client when the client stops.
    /// </summary>
    public override void OnLobbyStopClient()
    {
        base.OnLobbyStopClient();
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Start()
    {
        game_configuration = GetComponent<GameConfiguration>();

        original_lobby_scene = lobbyScene;

        original_components = new List<Component>(GetComponents<Component>());
    }

    public void Clear()
    {
        game_lobby.onlineScene = null;
        game_lobby.playScene = null;

        // Remove any component that is not part of the original set.

        var components = new List<Component>(GetComponents<Component>().Where(component => !original_components.Contains(component)));

        foreach(var component in components)
        {
            Destroy(component);
        }
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
            networkSceneName = null;

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

        if (game_configuration.game_type == GameType.Offline)
        {
            CreateOfflineLobby();                   // Offline games are just hosted games where other players are AIs.
        }
        else
        {
            connection_attempts = max_connection_attempts;
            match_list_page = 0;

            SearchLobby();                          // Attempt to join an existing lobby, create a brand new one when none is found.
        }
    }

    /// <summary>
    /// Create an offline lobby.
    /// </summary>
    private void CreateOfflineLobby()
    {
        networkAddress = "localhost";

        game_configuration.Randomize();

        StartHost();

        PostStartServer();
    }

    /// <summary>
    /// Join an existing lobby matching the current game configuration.
    /// </summary>
    private void SearchLobby()
    {
        StartMatchMaker();

        matchMaker.ListMatches(match_list_page, list_page_count, "", true, 0, 0, OnMatchList);
    }

    /// <summary>
    /// Attempt to connect to the next online match.
    /// If no other match is available create a new lobby.
    /// </summary>
    private void TryConnectToMatch()
    {
        if (connection_attempts == 0)
        {
            CreateOnlineLobby();                                                            // Enough attempts, just create a new one.
        }
        else
        {
            // Connect to the first compatible match.

            while (match_list.Count > 0)
            {
                var next_match = match_list.Pop();

                if (game_configuration.IsCompatible(next_match.name))
                {
                    matchMaker.JoinMatch(next_match.networkId, "", "", "", 0, 0, OnMatchJoined);
                    return;
                }
            }

            // List more lobbies.

            SearchLobby();
        }
    }

    /// <summary>
    /// Create an online lobby for other players to join via unity's matchmaking.
    /// </summary>
    private void CreateOnlineLobby()
    {
        StartMatchMaker();

        game_configuration.Randomize();
        
        matchMaker.CreateMatch(game_configuration.ToMatchName(), (uint) maxPlayers, true, "", "", "", 0, 0, OnMatchCreate);
    }

    /// <summary>
    /// Called after the server has been started and network properly configured.
    /// OnStartServer is useless since it gets called before the server
    /// is actually created, making impossible to spawn objects from there or doing anything meaningful.
    /// </summary>
    private void PostStartServer()
    {
        is_host = true;

        var game_mode = GameObject.Instantiate(game_configuration.game_mode);

        NetworkServer.Spawn(game_mode);
    }

    /// <summary>
    /// Add local players to the current match (either hosted or joined).
    /// </summary>
    private void AddLocalPlayers()
    {
        local_players = new List<PlayerConfiguration>(GetComponents<PlayerConfiguration>());

        for (short index = 0; index < local_players.Count; ++index)
        {
            ClientScene.AddPlayer(NetworkClient.allClients[0].connection, index);           // Can't use TryToAddPlayer since it will add a player with a wrong controller id.
        }
    }

    /// <summary>
    /// Add a new AI in an empty slot.
    /// </summary>
    private IEnumerator AddAI()
    {
        yield return new WaitForSeconds(ai_join_delay);

        // Choose a ship that hasn't been already chosen.
        
        var ai_ships = new List<GameObject>(ship_prefabs.Where(prefab => !lobbySlots.Any(slot => (slot != null) && ((LobbyPlayer)slot).player_ship == prefab.name)));

        var ai_configuration = gameObject.AddComponent<PlayerConfiguration>();

        ai_configuration.player_controller_id = local_players.Count;
        ai_configuration.ship_prefab = ai_ships[0];
        ai_configuration.is_human = false;

        local_players.Add(ai_configuration);

        TryToAddPlayer();
    }

    /// <summary>
    /// Called whenever a new player joins the lobby.
    /// </summary>
    private void OnPlayerJoined(LobbyPlayer lobby_player)
    {
        lobby_player.PlayerReadyEvent += OnPlayerReady;
    }

    /// <summary>
    /// Called whenever a player leaves the lobby.
    /// </summary>
    private void OnPlayerLeft(LobbyPlayer lobby_player)
    {
        lobby_player.PlayerReadyEvent -= OnPlayerReady;
    }

    /// <summary>
    /// Called whenever a player ready status changes.
    /// </summary>
    private void OnPlayerReady(LobbyPlayer lobby_player)
    {
        // Check if everyone is ready and fill the remaining slots with AIs.

        if(lobbySlots.All(slot => (slot == null) || slot.readyToBegin))                 // Everyone is ready.
        {
            LockMatch();                                                                // Prevent any other player from joining.

            if(lobbySlots.Count(slot => slot == null) > 0)
            {
                StartCoroutine(AddAI());
            }
        }
    }

    /// <summary>
    /// Called whenever the match countdown reaches 0.
    /// </summary>
    private void OnCountdown(GameTimer timer)
    {
        if(timer)
        {
            timer.TimeOutEvent -= OnCountdown;

            Destroy(timer.gameObject);
        }

        ServerChangeScene(game_configuration.arena);
    }

    /// <summary>
    /// Lock the match, preventing any other player from joining.
    /// </summary>
    private void LockMatch()
    {
        // Unlist the match so other players don't join it.

        if (match_info != null)
        {
            matchMaker.SetMatchAttributes(match_info.networkId, false, 0, OnSetMatchAttributes);
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
    private bool is_host = false;

    /// <summary>
    /// Current match info.
    /// </summary>
    private MatchInfo match_info = null;

    /// <summary>
    /// Number of host to list per page.
    /// </summary>
    private const int list_page_count = 8;

    /// <summary>
    /// Connection attempts left before giving up.
    /// </summary>
    private int connection_attempts = 0;

    /// <summary>
    /// Current match list page.
    /// </summary>
    private int match_list_page = 0;

    /// <summary>
    /// List of online matches to connect to.
    /// </summary>
    private Stack<MatchInfoSnapshot> match_list;

    /// <summary>
    /// The original lobby scene.
    /// </summary>
    private string original_lobby_scene;

    /// <summary>
    /// Original components on this game object.
    /// </summary>
    private List<Component> original_components;
}
