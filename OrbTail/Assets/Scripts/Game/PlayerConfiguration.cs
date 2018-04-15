using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represent a player configuration.
/// </summary>
public class PlayerConfiguration : MonoBehaviour
{
    /// <summary>
    /// Id of the player controller this configuration refers to.
    /// </summary>
    public int player_controller_id = 0;

    /// <summary>
    /// Ship selected by this player.
    /// </summary>
    public GameObject ship_prefab;
}
