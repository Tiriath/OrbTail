using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Reads the data from a mobile platform
/// </summary>
public class MobileInputBroker: IInputBroker
{
    /// <summary>
    /// Returns the throttle input status. -1 backwards, 0 still, +1 maximum throttle.
    /// </summary>
    public float ThrottleInput { get; private set; }

    /// <summary>
    /// Returns the steer input status. -1 steer left, +1 steer right.
    /// </summary>
    public float SteerInput { get; private set; }

    /// <summary>
    /// Returns the fire input status.
    /// </summary>
    public bool FireInput { get; private set; }

    /// <summary>
    /// Returns the special input status.
    /// </summary>
    public bool SpecialInput { get; private set; }

    public MobileInputBroker()
    {
        //Standard position, with the phone in landscape position and the bottom on the right.
        Calibrate();
        
        var handler = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<HUDButtonsHandler>();

        handler.EventOnMissileButtonSelect += OnPowerButtonSelect;
    }

    public void UpdateInput()
    {
        FireInput = false;

        // #TODO Review this.

        ThrottleInput = Mathf.Clamp((-Input.acceleration.z - ZOffset) * kThrottleFactor, -1.0f, 1.0f);
        SteerInput = Mathf.Clamp(Input.acceleration.x * kSteerFactor, -1.0f, 1.0f);

        if (Input.acceleration.sqrMagnitude > kBoostThreshold)
        {
            SpecialInput = true;
        }
    }

    /// <summary>
    /// Calibrate device inclination.
    /// </summary>
    private void Calibrate()
    {
        ZOffset = Mathf.Clamp(-Input.acceleration.z, -kMaxZOffset, kMaxZOffset);
    }

    /// <summary>
    /// Called whenever the power button is pressed.
    /// </summary>
    private void OnPowerButtonSelect(object sender, GameObject button)
    {
        FireInput = true;
    }

    private const float kThrottleFactor = 4f;

    private const float kSteerFactor = 2f;

    private const float kBoostThreshold = 3.0f;

    /// <summary>
    /// Maximum allowed inclination to use for calibration.
    /// </summary>
    private const float kMaxZOffset = 0.6f;

    /// <summary>
    /// Offset value used to compensate for initial device inclination.
    /// </summary>
    private float ZOffset = 0.0f;
}
