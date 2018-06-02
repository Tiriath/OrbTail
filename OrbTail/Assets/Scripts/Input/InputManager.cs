
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Interface used to read user inputs.
/// </summary>
public class InputManager
{
    // Prefixes (device and player index)

    public const string kKeyboardPrefix = "K1_";
    public static readonly string[] kJoystickPrefix = new string[] { "J1_", "J2_", "J3_", "J4_" };

    // Menu action names.

    public const string kJoin = "Join";
    public const string kConfirm = "Confirm";
    public const string kCancel = "Cancel";
    public const string kHorizontal = "Horizontal";
    public const string kVertical = "Vertical";

    // In-game action names.

    public const string kThrottle = "Throttle";
    public const string kSteer = "Steer";
    public const string kPowerUp = "PowerUp";

    /// <summary>
    /// Get join input status.
    /// </summary>
    public bool Join
    {
        get
        {
            return join_buttons.Any(join_button => Input.GetButtonDown(join_button));
        }
    }

    /// <summary>
    /// Get confirm input status.
    /// </summary>
    public bool Confirm
    {
        get
        {
            return confirm_buttons.Any(confirm_button => Input.GetButtonDown(confirm_button));
        }
    }

    /// <summary>
    /// Get cancel input status.
    /// </summary>
    public bool Cancel
    {
        get
        {
            return cancel_buttons.Any(cancel_button => Input.GetButtonDown(cancel_button));
        }
    }

    /// <summary>
    /// Get horizontal input status.
    /// </summary>
    public float Horizontal
    {
        get
        {
            return Mathf.Clamp(horizontal_axes.Sum(horizontal_axis => Input.GetAxis(horizontal_axis)), -1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Get vertical input status.
    /// </summary>
    public float Vertical
    {
        get
        {
            return Mathf.Clamp(vertical_axes.Sum(vertical_axis => Input.GetAxis(vertical_axis)), -1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Get throttle input status.
    /// </summary>
    public float Throttle
    {
        get
        {
            return Mathf.Clamp(throttle_axes.Sum(throttle_axis => Input.GetAxis(throttle_axis)), -1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Get steer input status.
    /// </summary>
    public float Steer
    {
        get
        {
            return Mathf.Clamp(steer_axes.Sum(steer_axis => Input.GetAxis(steer_axis)), -1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Get powerup input status.
    /// </summary>
    public bool PowerUp
    {
        get
        {
            return powerup_buttons.Any(powerup_button => Input.GetButtonDown(powerup_button));
        }
    }

    /// <summary>
    /// Create a new input manager for the provided player.
    /// </summary>
    public InputManager(short player_index)
    {
        // Keyboard available only for the first player.

        if(player_index == 0)
        {
            join_buttons.Add(kKeyboardPrefix + kJoin);
            confirm_buttons.Add(kKeyboardPrefix + kConfirm);
            cancel_buttons.Add(kKeyboardPrefix + kCancel);
            horizontal_axes.Add(kKeyboardPrefix + kHorizontal);
            vertical_axes.Add(kKeyboardPrefix + kVertical);

            powerup_buttons.Add(kKeyboardPrefix + kPowerUp);
            throttle_axes.Add(kKeyboardPrefix + kThrottle);
            steer_axes.Add(kKeyboardPrefix + kSteer);
        }

        // Joystick for each of the 4 players (maximum).

        var joystick_prefix = kJoystickPrefix[player_index];

        join_buttons.Add(joystick_prefix + kJoin);
        confirm_buttons.Add(joystick_prefix + kConfirm);
        cancel_buttons.Add(joystick_prefix + kCancel);
        horizontal_axes.Add(joystick_prefix + kHorizontal);
        vertical_axes.Add(joystick_prefix + kVertical);

        powerup_buttons.Add(joystick_prefix + kPowerUp);
        throttle_axes.Add(joystick_prefix + kThrottle);
        steer_axes.Add(joystick_prefix + kSteer);
    }

    private List<string> join_buttons = new List<string>();
    private List<string> confirm_buttons = new List<string>();
    private List<string> cancel_buttons = new List<string>();
    private List<string> horizontal_axes = new List<string>();
    private List<string> vertical_axes = new List<string>();

    private List<string> powerup_buttons = new List<string>();
    private List<string> throttle_axes = new List<string>();
    private List<string> steer_axes = new List<string>();
}