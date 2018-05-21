using UnityEngine;
using System.Collections;

/// <summary>
/// List of layers used to categorize collisions and hit events.
/// </summary>
public class Layers
{
    public const int Field = 1 << 8;
    public const int Obstacles = 1 << 9;

    public const int FieldAndObstacles = Field | Obstacles;

    public const int MenuButton = 1 << 10;

    /// <summary>
    /// Get the layer associated to a specific player.
    /// Used to filter out things that can only be seen by a given player (such as HUD, ecc.)
    /// </summary>
    public static int GetPlayerLayer(LobbyPlayer player)
    {
        return LayerMask.NameToLayer(string.Format("Player{0}Only", player.player_index + 1));
    }

    /// <summary>
    /// Get the layer bitmask associated to a specific player.
    /// Used to filter out things that can only be seen by a given player (such as HUD, ecc.)
    /// </summary>
    public static int GetPlayerLayerBitmask(LobbyPlayer player)
    {
        return 1 << GetPlayerLayer(player);
    }

    /// <summary>
    /// Set the layer of a game object and each of its children.
    /// </summary>
    public static void SetLayerRecursive(GameObject game_object, int layer)
    {
        foreach (var transform in game_object.GetComponentsInChildren<Transform>(true))
        {
            transform.gameObject.layer = layer;
        }
    }
}
