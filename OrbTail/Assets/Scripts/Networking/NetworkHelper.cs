using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Utility class for networking.
/// </summary>
public class NetworkHelper
{
    /// <summary>
    /// Returns true if this is the server or the device is disconnected
    /// </summary>
    /// <returns></returns>
    public static bool IsServerSide()
    {

        return Network.isServer ||
               Network.peerType == NetworkPeerType.Disconnected;

    }

    /// <summary>
    /// Returns true if this device is either a server or a client
    /// </summary>
    public static bool IsConnected()
    {

        return Network.peerType != NetworkPeerType.Disconnected;

    }
}

