﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Linq;

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
    public GameObject tutorial_hud;

    /// <summary>
    /// HUD prefab.
    /// </summary>
    public GameObject game_hud;

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
    /// Number of orbs in the match.
    /// </summary>
    private int orb_count = 28;

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

    /// <summary>
    /// Get the current ranks.
    /// </summary>
    public List<LobbyPlayer> Ranks
    {
        get
        {
            var ranks = new List<LobbyPlayer>(GameLobby.Instance.lobbySlots.Where(player => player).Select(slot => (LobbyPlayer)slot));

            ranks.Sort((first, second) => second.score.CompareTo(first.score));

            return ranks;
        }
    }

    /// <summary>
    /// Get the current winner.
    /// </summary>

    public LobbyPlayer Winner
    {
        get
        {
            var ranks = Ranks;

            if(ranks.Count == 1)                    // Single player, must be the winner.
            {
                return ranks.First();
            }
            else if(ranks.Count > 1 && ranks[0].score > ranks[1].score)
            {
                return ranks.First();               // The first player score is strictly greater than second's one.
            }
            else
            {
                return null;                        // Tie or empty match: no winner.
            }
        }
    }

    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        Ship.ShipLocalPlayerEvent += OnShipLocalPlayer;
        Ship.ShipCreatedEvent += OnShipCreated;
        Ship.ShipDestroyedEvent += OnShipDestroyed;

        OrbController.OrbCreatedEvent += OnOrbCreated;

        timer = GetComponent<GameTimer>();
    }

    public void OnDestroy()
    {
        Ship.ShipDestroyedEvent -= OnShipDestroyed;
        Ship.ShipCreatedEvent -= OnShipCreated;
        Ship.ShipLocalPlayerEvent -= OnShipLocalPlayer;

        OrbController.OrbCreatedEvent -= OnOrbCreated;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Called whenever the match is created.
    /// Synchronizes the game mode until each ship has been created and each player is ready.
    /// This event is started autonomously.
    /// </summary>
    protected virtual void OnMatchSetup()
    {
        Debug.Log("BaseGameMode::OnMatchSetup");

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

        timer.TimeOutEvent += OnEndCountdown;

        timer.duration = countdown;
        timer.enabled = true;
    }

    /// <summary>
    /// Called whenever the match starts.
    /// </summary>
    protected virtual void OnMatchStart()
    {
        EnableControls(true);

        timer.TimeOutEvent += OnEndTime;

        timer.duration = duration;
        timer.enabled = true;
    }

    /// <summary>
    /// Called whenever the match ends.
    /// </summary>
    protected virtual void OnMatchEnd()
    {
        EnableControls(false);

        // Snap each local camera on the winner (if any).

        var winner = Winner;

        if (winner)
        {
            foreach(var camera in FindObjectsOfType<FollowCamera>())
            {
                camera.ViewTarget = ships[winner.player_index].gameObject;

                //camera.Snap();
            }
        }
    }

    /// <summary>
    /// Called whenever a new local player ship is created.
    /// </summary>
    protected virtual void OnShipLocalPlayer(Ship ship)
    {
        Debug.Log("BaseGameMode::OnShipLocalPlayer");

        if(ship.LobbyPlayer.is_human)
        {
            // Spawn a follow camera for each active local human player.

            var camera = Instantiate(follow_camera).GetComponent<FollowCamera>();

            camera.ViewTarget = ship.gameObject;

            camera.Snap();

            // Attach the tutorial HUD to the local player camera.

            if (tutorial_hud)
            {
                var hud = Instantiate(tutorial_hud).GetComponent<HUDHandler>();

                hud.Camera = camera.GetComponentInChildren<Camera>();
                hud.Owner = ship.gameObject;
            }

            // Attach the game HUD to the local player camera.

            if (game_hud)
            {
                var hud = Instantiate(game_hud).GetComponent<HUDHandler>();

                hud.Camera = camera.GetComponentInChildren<Camera>();
                hud.Owner = ship.gameObject;
            }

            // #WORKAROUND Each player should have its own GUI input handler.

            FindObjectOfType<GUIInputHandler>().OwningCamera = camera.GetComponentInChildren<Camera>();
        }
    }

    /// <summary>
    /// Called whenever a new ship is created.
    /// </summary>
    /// <param name="ship"></param>
    protected virtual void OnShipCreated(Ship ship)
    {
        Debug.Log("BaseGameMode::OnShipCreated");

        ships.Add(ship);

        EnableControls(ship, false);

        CheckCountdownConditions();

        ship.ShipReadyEvent += OnShipReady;
    }

    /// <summary>
    /// Called whenever a ship is destroyed.
    /// </summary>
    /// <param name="ship"></param>
    protected virtual void OnShipDestroyed(Ship ship)
    {
        Debug.Log("BaseGameMode::OnShipDestroyed");

        ship.ShipReadyEvent -= OnShipReady;

        ships.Remove(ship);

        CheckCountdownConditions();
    }

    /// <summary>
    /// Called whenever an orb is created.
    /// </summary>
    /// <param name="orb"></param>
    protected virtual void OnOrbCreated(OrbController orb)
    {
        orbs.Add(orb);

        CheckCountdownConditions();
    }

    /// <summary>
    /// Start the initial countdown.
    /// </summary>
    [ClientRpc]
    private void RpcMatchCountdown()
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
    private void RpcMatchStart()
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
    private void RpcMatchEnd()
    {
        OnMatchEnd();

        if (MatchEndEvent != null)
        {
            MatchEndEvent(this);
        }
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
    /// Called whenever a ship is ready.
    /// </summary>
    private void OnShipReady(Ship ship)
    {
        CheckCountdownConditions();
    }

    /// <summary>
    /// Check whether the countdown conditions are met.
    /// </summary>
    private void CheckCountdownConditions()
    {
        if (isServer)
        {
            // Start the match when each ship is ready.

            foreach (var ship in ships)
            {
                if (!ship.is_ready)
                {
                    return;
                }
            }

            // Start the match when each orb has been spawned on the server.

            if(orbs.Count < orb_count)
            {
                return;
            }

            RpcMatchCountdown();
        }
    }

    /// <summary>
    /// Called whenever the countdown timer ends.
    /// </summary>
    private void OnEndCountdown(GameTimer timer)
    {
        timer.TimeOutEvent -= OnEndCountdown;

        if (isServer)
        {
            RpcMatchStart();
        }
    }

    /// <summary>
    /// Called whenever the countdown timer ends.
    /// </summary>
    private void OnEndTime(GameTimer timer)
    {
        timer.TimeOutEvent -= OnEndTime;

        if (isServer)
        {
            RpcMatchEnd();
        }
    }

    /// <summary>
    /// Enable or disable the controls for all players.
    /// </summary>
    private void EnableControls(bool value)
    {
        foreach (var ship in ships)
        {
            EnableControls(ship, value);
        }
    }

    /// <summary>
    /// Enable or disable the controls on the provided ship.
    /// </summary>
    private void EnableControls(Ship ship, bool value)
    {
        var movement = ship.GetComponent<MovementController>();
        var power = ship.GetComponent<PowerController>();

        movement.enabled = value;
        power.enabled = value;
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

