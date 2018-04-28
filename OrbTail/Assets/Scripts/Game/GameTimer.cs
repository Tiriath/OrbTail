using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Script used to keep track of game time in a networked game.
/// </summary>
public class GameTimer : NetworkBehaviour
{
    /// <summary>
    /// Timer duration in seconds.
    /// </summary>
    public int duration = 60;

    /// <summary>
    /// Current time.
    /// </summary>
    [SyncVar]
    public int time = 0;

    public void Start()
    {
        // #TODO Bind to game event. Call OnTimerStart.
    }

    public void Update ()
    {
        if(is_active)
        {
            // Update the timer on each client, the server will overwrite the timer status during synchronization.

            int new_time = Mathf.Min(Mathf.FloorToInt(Time.realtimeSinceStartup - timestamp), duration);

            if(time != new_time)
            {
                time = new_time;
            }

            if(time == 0)
            {
                is_active = false;

                // #TODO Transition!
            }
        }
    }

    /// <summary>
    /// Start the timer.
    /// </summary>
    private void OnTimerStart(BaseGameMode game_mode)
    {
        timestamp = Time.realtimeSinceStartup;

        time = 0;

        is_active = true;
    }

    /// <summary>
    /// Whether the timer is active.
    /// </summary>
    private bool is_active = false;

    /// <summary>
    /// Timestamp when the game first began.
    /// </summary>
    private float timestamp = 0.0f;
}
