using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class inherited by each game mode.
/// Exposes common game events and game states.
/// </summary>
public abstract class BaseGameMode : NetworkBehaviour
{
    public delegate void DelegateGameModeEvent(BaseGameMode game_mode);

    public event DelegateGameModeEvent MatchSetupEvent;
    public event DelegateGameModeEvent MatchCountdownEvent;
    public event DelegateGameModeEvent MatchStartEvent;
    public event DelegateGameModeEvent MatchEndEvent;

    /// <summary>
    /// Tutorial prefab.
    /// </summary>
    public GameObject tutorial;

    /// <summary>
    /// HUD prefab.
    /// </summary>
    public GameObject hud;

    /// <summary>
    /// Prefab representing the follow camera.
    /// </summary>
    public GameObject follow_camera;

    /// <summary>
    /// Match duration, in seconds.
    /// </summary>
    public int duration = 120;

    /// <summary>
    /// Countdown duration, in seconds.
    /// </summary>
    public int countdown = 3;

    /// <summary>
    /// Get the game mode instance.
    /// </summary>
    public static BaseGameMode Instance
    {
        get
        {
            if (game_mode_instance == null)
            {
                game_mode_instance = FindObjectOfType<BaseGameMode>();
            }

            return game_mode_instance;
        }
    }

    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        Ship.ShipLocalPlayerEvent += OnShipLocalPlayer;
        Ship.ShipCreatedEvent += OnShipCreated;
        Ship.ShipDestroyedEvent += OnShipDestroyed;

        timer = GetComponent<GameTimer>();
    }

    public void OnDestroy()
    {
        Ship.ShipDestroyedEvent -= OnShipDestroyed;
        Ship.ShipCreatedEvent -= OnShipCreated;
        Ship.ShipLocalPlayerEvent -= OnShipLocalPlayer;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Start the initial countdown.
    /// </summary>
    [ClientRpc]
    public void RpcMatchCountdown()
    {
        OnMatchCountdown();

        if (MatchCountdownEvent != null)
        {
            MatchCountdownEvent(this);
        }
    }

    /// <summary>
    /// Start the current match.
    /// </summary>
    [ClientRpc]
    public void RpcMatchStart()
    {
        OnMatchStart();

        if (MatchStartEvent != null)
        {
            MatchStartEvent(this);
        }
    }

    /// <summary>
    /// End the current match.
    /// </summary>
    [ClientRpc]
    public void RpcMatchEnd()
    {
        OnMatchEnd();

        if (MatchEndEvent != null)
        {
            MatchEndEvent(this);
        }
    }

    /// <summary>
    /// Called whenever the match is created.
    /// Synchronizes the game mode until each ship has been created and each player is ready.
    /// This event is started autonomously.
    /// </summary>
    protected virtual void OnMatchSetup()
    {
        Debug.Log("BaseGameMode::OnMatchSetup");

        orbs.AddRange(FindObjectsOfType<OrbController>());

        foreach (LobbyPlayer lobby_player in GameLobby.Instance.lobbySlots)
        {
            if(lobby_player)
            {
                lobby_player.score = 0;
            }
        }
    }

    /// <summary>
    /// Called whenever the match countdown starts.
    /// Ships are guaranteed to exist at this point.
    /// </summary>
    protected virtual void OnMatchCountdown()
    {
        EnableControls(false);
    }

    /// <summary>
    /// Called whenever the match starts.
    /// </summary>
    protected virtual void OnMatchStart()
    {
        EnableControls(true);
    }

    /// <summary>
    /// Called whenever the match ends.
    /// </summary>
    protected virtual void OnMatchEnd()
    {
        EnableControls(false);

        //if (winner != null)
        //{
        //    distanceSmooth = finalSmooth;
        //    Camera.LookAt(winner);
        //}
    }

    /// <summary>
    /// Called whenever a new local player ship is created.
    /// </summary>
    protected virtual void OnShipLocalPlayer(Ship ship)
    {
        Debug.Log("BaseGameMode::OnShipLocalPlayer");

        // Spawn a follow camera for each active local player.

        var camera = Instantiate(follow_camera).GetComponent<FollowCamera>();

        camera.ViewTarget = ship.gameObject;
        camera.Owner = GameLobby.Instance.lobbySlots[ship.player_index] as LobbyPlayer;

        camera.Snap();

        // Attach tutorial HUD to the local player camera.

        if (tutorial)
        {
            Instantiate(tutorial).GetComponent<HUDPositionHandler>().Camera = camera;
        }

        // Attach game-mode HUD to the local player camera.

        //         if(hud)
        //         {
        //             Instantiate(hud).GetComponent<HUDPositionHandler>().Camera = camera;
        //         }

        // #WORKAROUND Each player should have its own GUI input handler.

        FindObjectOfType<GUIInputHandler>().OwningCamera = camera.GetComponentInChildren<Camera>();
    }

    /// <summary>
    /// Called whenever a new ship is created.
    /// </summary>
    /// <param name="ship"></param>
    protected virtual void OnShipCreated(Ship ship)
    {
        Debug.Log("BaseGameMode::OnShipCreated");

        ships.Add(ship);
    }

    /// <summary>
    /// Called whenever a ship is destroyed.
    /// </summary>
    /// <param name="ship"></param>
    protected virtual void OnShipDestroyed(Ship ship)
    {
        Debug.Log("BaseGameMode::OnShipDestroyed");

        ships.Remove(ship);
    }

    /// <summary>
    /// Check whether the end conditions for this game mode are met.
    /// </summary>
    /// <returns>Returns true if the end conditions are met and the match should be terminated, returns false otherwise.</returns>
    protected virtual bool CheckEndConditions()
    {
        // Either the time's up OR there are less than one ship in the game.

        return false;
    }

    /// <summary>
    /// Called whenever a new scene is loaded.
    /// </summary>
    /// <param name="scene">Scene being loaded.</param>
    /// <param name="mode">Scene loading mode.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnMatchSetup();

        if (MatchSetupEvent != null)
        {
            MatchSetupEvent(this);
        }
    }

    /// <summary>
    /// Enable or disable the controls for all players.
    /// </summary>
    private void EnableControls(bool value)
    {
        foreach (var ship in ships)
        {
            var movement = ship.GetComponent<MovementController>();
            var power = ship.GetComponent<PowerController>();

            movement.enabled = value;
            power.enabled = value;
        }
    }

    /// <summary>
    /// List of ships in the current match.
    /// </summary>
    protected List<Ship> ships = new List<Ship>();

    /// <summary>
    /// List of orbs in the current match.
    /// </summary>
    protected List<OrbController> orbs = new List<OrbController>();

    /// <summary>
    /// Singleton instance of the game mode.
    /// </summary>
    private static BaseGameMode game_mode_instance;

    /// <summary>
    /// Game timer.
    /// </summary>
    private GameTimer timer;
}

