using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to prevent a device from ever turning off the screen.
/// </summary>
public class ScreenDimmer : MonoBehaviour
{
    void Start ()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
