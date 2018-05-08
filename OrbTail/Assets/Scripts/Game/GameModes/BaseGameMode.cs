using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class inherited by each game mode.
/// Exposes common game events.
/// </summary>
public abstract class BaseGameMode : NetworkBehaviour
{
    public delegate void DelegateGameModeEvent(BaseGameMode game_mode);

    public event DelegateGameModeEvent MatchSetupEvent;
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
        Ship.ShipLocalPlayerEvent += OnShipLocalPlayer;
        Ship.ShipCreatedEvent += OnShipCreated;
        Ship.ShipDestroyedEvent += OnShipDestroyed;
    }

    public void OnDestroy()
    {
        Ship.ShipLocalPlayerEvent -= OnShipLocalPlayer;
        Ship.ShipCreatedEvent -= OnShipCreated;
        Ship.ShipDestroyedEvent -= OnShipDestroyed;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Setup the current match.
    /// </summary>
    [ClientRpc]
    public void RpcMatchSetup()
    {
        OnMatchSetup();

        if (MatchSetupEvent != null)
        {
            MatchSetupEvent(this);
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
    /// Setup the current match.
    /// </summary>
    protected virtual void OnMatchSetup()
    {
        EnableControls(false);

        // Reset player scores.

        foreach (LobbyPlayer lobby_player in GameLobby.Instance.lobbySlots)
        {
            lobby_player.score = 0;
        }
    }

    /// <summary>
    /// Start the current match.
    /// </summary>
    protected virtual void OnMatchStart()
    {
        EnableControls(true);
    }

    /// <summary>
    /// End the current match.
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
    /// Called whenever a new local player ship is created.
    /// </summary>
    protected virtual void OnShipLocalPlayer(Ship ship)
    {
        // Spawn a follow camera for each active local player.

        var camera = Instantiate(follow_camera).GetComponent<FollowCamera>();

        camera.ViewTarget = ship.gameObject;
        camera.Owner = ship.LobbyPlayer;

        camera.Snap();

        // Attach tutorial HUD to the local player camera.

        if(tutorial)
        {
            Instantiate(tutorial).GetComponent<HUDPositionHandler>().Camera = camera;
        }

        // Attach game-mode HUD to the local player camera.

        if(hud)
        {
            Instantiate(hud).GetComponent<HUDPositionHandler>().Camera = camera;
        }

        // #WORKAROUND Each player should have its own GUI input handler.

        FindObjectOfType<GUIInputHandler>().OwningCamera = camera.GetComponentInChildren<Camera>();
    }

    /// <summary>
    /// Called whenever a new ship is created.
    /// </summary>
    /// <param name="ship"></param>
    protected virtual void OnShipCreated(Ship ship)
    {
        ships.Add(ship);
    }

    /// <summary>
    /// Called whenever a ship is destroyed.
    /// </summary>
    /// <param name="ship"></param>
    protected virtual void OnShipDestroyed(Ship ship)
    {
        ships.Remove(ship);
    }

    /// <summary>
    /// List of ships in the current match.
    /// </summary>
    protected List<Ship> ships = new List<Ship>();

    /// <summary>
    /// Singleton instance of the game mode.
    /// </summary>
    private static BaseGameMode game_mode_instance;
}

