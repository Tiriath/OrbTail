using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple countdown before starting a match.
/// </summary>
public class LobbyCountdown : GameTimer
{
    public delegate void DelegateLobbyCountdownEvent(LobbyCountdown sender);

    public static event DelegateLobbyCountdownEvent LobbyCountdownStartedEvent;

    /// <summary>
    /// Called on each client after being activated.
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (LobbyCountdownStartedEvent != null)
        {
            LobbyCountdownStartedEvent(this);
        }
    }
}
