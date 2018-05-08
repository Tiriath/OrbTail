using UnityEngine;

/// <summary>
/// Script used to remove elements based on the current platform.
/// </summary>
public class GUIPlatform : MonoBehaviour
{
    /// <summary>
    /// Platforms this GUI element should exist on.
    /// </summary>
    public DeviceType[] platforms;

    // Use this for initialization
    void Awake()
    {
        foreach(var platform in platforms)
        {
            if(platform == SystemInfo.deviceType)
            {
                return;
            }
        }

        DestroyObject(gameObject);
    }
}