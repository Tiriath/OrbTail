using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

/// <summary>
/// Game type.
/// </summary>
public enum GameType
{
    Offline = 0,
    Online = 1
}

/// <summary>
/// Game mode.
/// </summary>
public enum GameMode
{
    Arcade = 0,
    LongestTail = 1,
    Elimination = 2,
    Any = 3
}

/// <summary>
/// Represent a game configuration.
/// </summary>
public class GameConfiguration : MonoBehaviour
{
    /// <summary>
    /// Get the singleton instance.
    /// </summary>
    public static GameConfiguration Instance
    {
        get
        {
            return GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameConfiguration>();
        }
    }

    /// <summary>
    /// Lobby name.
    /// </summary>
    public string lobby_name = "orbtail";

    /// <summary>
    /// Game type.
    /// </summary>
    public GameType game_type;

    /// <summary>
    /// Game mode.
    /// </summary>
    public GameMode game_mode;

    /// <summary>
    /// Arena name. Empty if any arena.
    /// </summary>
    public string arena;

    /// <summary>
    /// Check whether this configuration is compatible with the provided one.
    /// A configuration is compatible if each field is equal or more specific than the equivalent field in the other.
    /// </summary>
    /// <param name="rhs">Other configuration to test against.</param>
    /// <returns>Returns true if this configuration is compatible with rhs, returns false otherwise.</returns>
    public bool IsCompatible(GameConfiguration rhs)
    {
        if(arena.Length > 0 && arena != rhs.arena)
        {
            return false;
        }

        if(game_mode != GameMode.Any && game_mode != rhs.game_mode)
        {
            return false;
        }

        return true;
    }

}
