﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Script used to keep track of game time in a networked game.
/// </summary>
public class GameTimer : NetworkBehaviour
{
    public delegate void DelegateTick(GameTimer sender);

    /// <summary>
    /// Timer duration in seconds.
    /// </summary>
    public int duration = 60;

    /// <summary>
    /// Current time.
    /// </summary>
    [SyncVar]
    public int time = 0;

    public event DelegateTick TickEvent;

    public void Awake()
    {
        time = duration;
    }

    public void OnEnable()
    {
        timestamp = Time.realtimeSinceStartup;
    }

    public void Update ()
    {
        // Update the timer on each client, the server will overwrite the timer status during synchronization.

        int new_time = Mathf.Max(duration - Mathf.FloorToInt(Time.realtimeSinceStartup - timestamp), 0);

        if(time != new_time)
        {
            time = new_time;

            if(TickEvent != null)
            {
                TickEvent(this);
            }
        }

        if(time == 0)
        {
            enabled = false;

            // #TODO Transition!
        }
    }

    /// <summary>
    /// Timestamp when the timer is activated.
    /// </summary>
    private float timestamp = 0.0f;
}
