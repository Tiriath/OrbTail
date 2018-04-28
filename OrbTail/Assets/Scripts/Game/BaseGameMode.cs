using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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
    }

    /// <summary>
    /// Enable or disable the controls for all players.
    /// </summary>
    private void EnableControls(bool value)
    {
        foreach (GameObject ship in GameObject.FindGameObjectsWithTag(Tags.Ship))
        {
            var movement = ship.GetComponent<MovementController>();

            movement.enabled = value;
        }
    }

    /// <summary>
    /// Singleton instance of the game mode.
    /// </summary>
    private static BaseGameMode game_mode_instance;
}

