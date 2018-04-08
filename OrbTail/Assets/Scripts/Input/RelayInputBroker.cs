using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Relays the input
/// </summary>
public class RelayInputBroker: IInputBroker
{
    /// <summary>
    /// Returns the acceleration command status. 0 no acceleration, 1 maximum acceleration.
    /// </summary>
    public float Acceleration
    {
        get
        {
            return acceleration_;
        }
        set
        {
            acceleration_ = Mathf.Clamp(value, -1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the steering command status. -1 steer left, 0 no steering, 1 steer right.
    /// </summary>
    public float Steering
    {
        get
        {
            return steering_;
        }
        set
        {
            steering_ = Mathf.Clamp(value, -1.0f, 1.0f);
        }
    }

    /// <summary>
    /// Returns the fire input status. 0 not firing, 1 firing.
    /// </summary>
    public bool Fire { get; set; }

    /// <summary>
    /// Returns the fire special input status. 0 not firing, 1 firing.
    /// </summary>
    public bool FireSpecial { get; set; }

    public RelayInputBroker()
    {
        acceleration_ = 0;
        steering_ = 0;
    }

    public void Update() {}

    /// <summary>
    /// The acceleration
    /// </summary>
    private float acceleration_;

    /// <summary>
    /// The steering
    /// </summary>
    private float steering_;
}