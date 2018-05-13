using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Script used to keep track of game time in a networked game.
/// </summary>
public class GameTimer : NetworkBehaviour
{
    public delegate void DelegateTimerEvent(GameTimer sender);

    /// <summary>
    /// Timer duration in seconds.
    /// </summary>
    public int duration = 60;

    /// <summary>
    /// Current time.
    /// </summary>
    [SyncVar]
    public int time = 0;

    public event DelegateTimerEvent TickEvent;
    public event DelegateTimerEvent TimeOutEvent;

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

        SetTime(duration - Mathf.FloorToInt(Time.realtimeSinceStartup - timestamp));
    }

    /// <summary>
    /// Stop the timer right away.
    /// </summary>
    public void Stop()
    {
        SetTime(0);
    }

    /// <summary>
    /// Tick the clock.
    /// </summary>
    private void SetTime(int time)
    {
        time = Mathf.Max(time, 0);

        if (this.time != time)
        {
            this.time = time;

            if (TickEvent != null)
            {
                TickEvent(this);
            }

            if (time == 0)
            {
                enabled = false;

                if (TimeOutEvent != null)
                {
                    TimeOutEvent(this);
                }
            }
        }
    }

    /// <summary>
    /// Timestamp when the timer is activated.
    /// </summary>
    private float timestamp = 0.0f;
}
