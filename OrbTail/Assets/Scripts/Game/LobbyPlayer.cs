using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Represents a player configuration in a lobby.
/// </summary>
public class LobbyPlayer : NetworkLobbyPlayer
{
    /// <summary>
    /// Player ship.
    /// </summary>
    [SyncVar(hook = "OnShipChanged")]
    public string player_ship = null;

    /// <summary>
    /// Player index, relative to the match.
    /// </summary>
    [SyncVar(hook = "OnPlayerIndexChanged")]
    public int player_index = -1;

    /// <summary>
    /// Called on server and client when the player joins the lobby.
    /// </summary>
    public override void OnClientEnterLobby()
    {
        Debug.Log("OnClientEnterLobby");

        base.OnClientEnterLobby();
    }

    /// <summary>
    /// Called on server and client when the player leaves the lobby.
    /// </summary>
    public override void OnClientExitLobby()
    {
        Debug.Log("OnClientExitLobby");

        base.OnClientExitLobby();
    }

    /// <summary>
    /// Called on clients when the lobby player switches between ready and not ready.
    /// </summary>
    /// <param name="ready_state"></param>
    public override void OnClientReady(bool ready_state)
    {
        Debug.Log("OnClientReady");

        base.OnClientReady(ready_state);
    }

    /// <summary>
    /// Called whenever the player ship changes.
    /// </summary>
    /// <param name="player_ship">Selected ship name.</param>
    private void OnShipChanged(string player_ship)
    {
        Debug.Log("OnShipChanged");

        this.player_ship = player_ship;
    }

    /// <summary>
    /// Called whenever the player color changes.
    /// </summary>
    /// <param name="player_index">Player index</param>
    private void OnPlayerIndexChanged(int player_index)
    {
        Debug.Log("OnPlayerIndexChanged");

        this.player_index = player_index;
    }

    /// <summary>
    /// Called on the local version of this behaviour.
    /// </summary>
    public override void OnStartAuthority()
    {
        Debug.Log("OnStartAuthority (controller: " + playerControllerId + ")");

        // #TODO Handle multiple local players.

        var player_configuration = GameLobby.Instance.GetLocalPlayer(playerControllerId);

        player_ship = player_configuration.ship_prefab.name;
    }
}
