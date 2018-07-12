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
    /// List of all possible game modes.
    /// </summary>
    public GameObject[] game_modes;

    /// <summary>
    /// List of all possible arenas.
    /// </summary>
    public SceneField[] arenas;

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
    public GameObject game_mode;

    /// <summary>
    /// Arena name. Empty if any arena.
    /// </summary>
    public SceneField arena;

    /// <summary>
    /// Randomize each unsolved configuration.
    /// </summary>
    public void Randomize()
    {
        Debug.Assert(game_modes.Length > 0, "No supported game mode.");
        Debug.Assert(arenas.Length > 0, "No supported arena.");

        if(game_mode == null)
        {
            game_mode = game_modes[UnityEngine.Random.Range(0, game_modes.Length)];
        }

        if(!arena.IsValid)
        {
            arena = arenas[UnityEngine.Random.Range(0, arenas.Length)];
        }
    }

    /// <summary>
    /// Check whether this configuration is compatible with the provided match name.
    /// A configuration is compatible if each field is equal or more specific than the equivalent field in the other.
    /// </summary>
    /// <param name="match_name">Match name to test against.</param>
    /// <returns>Returns true if this configuration is compatible with the provided match name, returns false otherwise.</returns>
    public bool IsCompatible(string match_name)
    {
        var fields = match_name.Split(';');

        if (game_mode != null && !game_mode.name.Equals(fields[1]))
        {
            return false;
        }

        if (arena.IsValid && !arena.SceneName.Equals(fields[2]))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Convert this game configuration to a match name.
    /// </summary>
    /// <returns>Returns the match name associated to this configuration.</returns>
    public string ToMatchName()
    {
        return string.Format("{0};{1};{2}", lobby_name, game_mode.name, arena.SceneName);
    }
}
