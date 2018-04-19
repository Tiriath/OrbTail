using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game type.
/// </summary>
public enum GameType
{
    Offline,
    Online
}

/// <summary>
/// Game mode.
/// </summary>
public enum GameMode
{
    Arcade,
    LongestTail,
    Elimination,
    Any
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
    /// Game type.
    /// </summary>
    public GameType game_type = GameType.Offline;

    /// <summary>
    /// Game mode.
    /// </summary>
    public GameMode game_mode = GameMode.Any;

    /// <summary>
    /// Arena name. Empty if any arena.
    /// </summary>
    public string arena;
}
