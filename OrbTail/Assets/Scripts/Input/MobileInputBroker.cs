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
    /// Returns the acceleration command's status. 0 no acceleration, 1 maximum acceleration.
    /// </summary>
    public float Acceleration { get; private set; }

    /// <summary>
    /// Returns the steering command's status. -1 steer left, 0 no steering, 1 steer right
    /// </summary>
    public float Steering { get; private set; }

    /// <summary>
    /// Returns the fire input status. 0 not firing, 1 firing.
    /// </summary>
    public bool Fire { get; private set; }

    /// <summary>
    /// Returns the fire special input status. 0 not firing, 1 firing.
    /// </summary>
    public bool FireSpecial { get; private set; }

    /// <summary>
    /// Returns the accelerometers' offset on z Axis (Acceleration)
    /// </summary>
    public float ZOffsetAcceleration { get; private set; }

    public MobileInputBroker()
    {

        //Standard position, with the phone in landscape position and the bottom on the right.
        ZOffsetAcceleration = 0f;
        Calibrate();
        HUDButtonsHandler hudButtonsHandler = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<HUDButtonsHandler>();
        hudButtonsHandler.EventOnMissileButtonSelect += OnPowerButtonSelect;

    }

    /// <summary>
    /// Set the actual offset used for the accelerometer's calculation
    /// </summary>
    public void Calibrate()
    {
        ZOffsetAcceleration = Mathf.Clamp(-Input.acceleration.z, -kMaxOffsetAcceleration, kMaxOffsetAcceleration);
    }

    public void Update()
    {
        //var delta = Vector3.Cross(AccelerometerOffset, Input.acceleration.normalized);
        //Acceleration = Mathf.Clamp(delta.x * kAccelerationExponent, -1f, 1f);
        Acceleration = Mathf.Clamp((-Input.acceleration.z - ZOffsetAcceleration) * kAccelerationExponent, -1f, 1f);
        Steering = Mathf.Clamp(Input.acceleration.x * kSteeringExponent, -1f, 1f);
        //From -1.0f to 1.0f.
        //Steering = Mathf.Pow( Mathf.Clamp01( Mathf.Abs( direction.x ) ), 
        //                                      kSteeringExponent) * Mathf.Sign(direction.x);
        //Steering = Mathf.Clamp(delta.z * kSteeringExponent, -1f, 1f);

        Fire = false;

        // TODO: to enhance 
        if (Input.acceleration.sqrMagnitude > kBoostThreshold)
        {
            FireSpecial = true;
        }
    }

    private void OnPowerButtonSelect(object sender, GameObject button)
    {
        Fire = true;
    }

    private const float kAccelerationExponent = 4f;

    private const float kSteeringExponent = 2f;

    private const float kBoostThreshold = 3.0f;

    private const float kMaxOffsetAcceleration = 0.6f;
}
