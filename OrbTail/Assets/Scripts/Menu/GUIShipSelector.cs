using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// GUI element used to display the ship being selected by each player.
/// </summary>
public class GUIShipSelector : GUIElement
{
    public delegate void DelegateShipSelectorEvent(GUIShipSelector sender);

    public static event DelegateShipSelectorEvent ShipSelectedEvent;

    /// <summary>
    /// Index of the player this selector refers to.
    /// </summary>
    public int player_index = 0;

    /// <summary>
    /// Alpha of the element when unconfirmed.
    /// </summary>
    public float unconfirmed_alpha = 0.5f;

    /// <summary>
    /// Fade duration, in seconds.
    /// </summary>
    public float fade_duration = 0.2f;

    /// <summary>
    /// Get or set whether the player joined the match.
    /// </summary>
    public bool PlayerJoined
    {
        get
        {
            return player_joined;
        }
        set
        {
            player_joined = value;

            Confirmed = false;
        }
    }

    /// <summary>
    /// Get or set whether the selection was confirmed.
    /// </summary>
    public bool Confirmed
    {
        get
        {
            return selection_confirmed;
        }
        set
        {
            selection_confirmed = value && PlayerJoined;

            Fade();

            if(ShipSelectedEvent != null)
            {
                ShipSelectedEvent(this);
            }
        }
    }

    /// <summary>
    /// Get the selected ship prefab.
    /// </summary>
    public GameObject ShipPrefab
    {
        get
        {
            return selection.ship_prefab;
        }
    }

    /// <summary>
    /// Get or set the current selection.
    /// </summary>
    public GUIButtonSelectShipBehaviour Selection
    {
        get
        {
            return selection;
        }
        set
        {
            if (!selection || (PlayerJoined && !Confirmed))
            {
                selection = value;

                gameObject.transform.SetParent(selection.transform, false);
            }
        }
    }

    public void Awake()
    {
        input_manager = new InputManager((short)player_index);

        selections = FindObjectsOfType<GUIButtonSelectShipBehaviour>();

        Selection = selections[0];

        PlayerJoined = (player_index == 0);
    }

    public void Update()
    {
        // Toggle join status.

        if (input_manager.Join)
        {
            PlayerJoined = !PlayerJoined;
        }

        // Confirm the selection.

        if (input_manager.Confirm)
        {
            Confirmed = !Confirmed;
        }

        // Move the selection around.

        Vector2 movement = new Vector2(input_manager.Horizontal, input_manager.Vertical);                           // Direction of the input.

        if (movement.sqrMagnitude <= 0.2f)
        {
            enable_movement = true;
        }
        else if (enable_movement)
        {
            enable_movement = false;

            movement.Normalize();

            var target_selections = selections                                                                      // Sort the other GUI elements the cursor can jump to by distance.
                .Where(s => s != this.selection)
                .OrderBy(s => (s.transform.position - this.selection.transform.position).sqrMagnitude)
                .ToArray();

            foreach (var target_selection in target_selections)                                                     // Find the best GUI element to jump to by direction (near elements are favored).
            {
                Vector2 direction = target_selection.transform.position - this.selection.transform.position;

                if (Vector2.Dot(direction.normalized, movement) >= 0.8f)
                {
                    Selection = target_selection;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Fade the element according to its status.
    /// </summary>
    private void Fade()
    {
        var fade = PlayerJoined ? (Confirmed ? 1.0f : unconfirmed_alpha) : 0.0f;

        iTween.FadeTo(gameObject, fade, fade_duration);
    }

    /// <summary>
    /// Whether the player joined the match.
    /// </summary>
    private bool player_joined = false;

    /// <summary>
    /// Whether the selection was confirmed.
    /// </summary>
    private bool selection_confirmed = false;

    /// <summary>
    /// Whether the selection movement is enabled.
    /// </summary>
    private bool enable_movement = true;

    /// <summary>
    /// Current selection.
    /// </summary>
    private GUIButtonSelectShipBehaviour selection;

    /// <summary>
    /// List of available selections.
    /// </summary>
    private GUIButtonSelectShipBehaviour[] selections;

    /// <summary>
    /// Input manager for the player this selector refers to.
    /// </summary>
    private InputManager input_manager;
}
