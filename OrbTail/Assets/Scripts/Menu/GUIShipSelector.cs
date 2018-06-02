using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GUI element used to display the ship being selected by each player.
/// </summary>
public class GUIShipSelector : GUIElement
{
    /// <summary>
    /// Index of the player this selector refers to.
    /// </summary>
    public int player_index = 0;

    /// <summary>
    /// Fade duration, in seconds.
    /// </summary>
    public float fade_duration = 0.2f;

    /// <summary>
    /// Whether the player joined the match.
    /// </summary>
    private bool PlayerJoined
    {
        get
        {
            return player_joined;
        }
        set
        {
            if(player_joined != value)
            {
                player_joined = value;

                iTween.FadeTo(gameObject, player_joined ? 1.0f : 0.0f, fade_duration);
            }
        }
    }

    public void Awake()
    {
        ships = FindObjectsOfType<GUIButtonSelectShipBehaviour>();

        local_position = transform.localPosition;

        player_joined = (player_index == 0);

        // Initial material status.

        var material = GetComponent<MeshRenderer>().material;

        var color = material.color;

        color.a = player_joined ? 1.0f : 0.0f;

        material.color = color;
    }

    public void Update()
    {

    }

    /// <summary>
    /// Whether the player joined the match.
    /// </summary>
    private bool player_joined;

    /// <summary>
    /// List of selectable ships.
    /// </summary>
    private GUIButtonSelectShipBehaviour[] ships;

    /// <summary>
    /// Local offset of this element relative to its parent.
    /// </summary>
    private Vector3 local_position;
}
