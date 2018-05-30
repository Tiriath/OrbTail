using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Toggle the local players ready status when pressed.
/// </summary>
public class GUIButtonToggleReadyBehaviour : GUIElement
{
    /// <summary>
    /// Text to display when the local players are ready.
    /// </summary>
    public string ready_text = "ready";

    /// <summary>
    /// Text to display when the local player are ready.
    /// </summary>
    public string not_ready_text = "not ready";

    public void Awake()
    {
        LobbyPlayer.LocalPlayerJoinedEvent += OnLocalPlayerJoined;

        LobbyCountdown.LobbyCountdownStartedEvent += OnLobbyCountdownStarted;

        GetComponent<TextMesh>().text = "";
    }

    public void OnDestroy()
    {
        LobbyPlayer.LocalPlayerJoinedEvent -= OnLocalPlayerJoined;

        LobbyCountdown.LobbyCountdownStartedEvent -= OnLobbyCountdownStarted;

        foreach (var local_player in local_players)
        {
            local_player.PlayerReadyEvent -= OnLobbyPlayerReady;
        }

        local_players.Clear();
    }

    public override void OnInputConfirm()
    {
        foreach (var local_player in local_players)
        {
            if (local_player.readyToBegin)
            {
                local_player.SendNotReadyToBeginMessage();
            }
            else
            {
                local_player.SendReadyToBeginMessage();
            }
        }
    }

    /// <summary>
    /// Called whenever a new local player joins the lobby.
    /// </summary>
    private void OnLocalPlayerJoined(LobbyPlayer lobby_player)
    {
        if(local_players.Add(lobby_player))
        {
            lobby_player.PlayerReadyEvent += OnLobbyPlayerReady;

            OnLobbyPlayerReady(lobby_player);
        }
    }

    /// <summary>
    /// Called whenever the ready status of the local player changes.
    /// </summary>
    private void OnLobbyPlayerReady(LobbyPlayer lobby_player)
    {
        GetComponent<TextMesh>().text = local_players.All(local_player => local_player.readyToBegin) ? ready_text : not_ready_text;
    }

    /// <summary>
    /// Called whenever the lobby countdown starts.
    /// </summary>
    public void OnLobbyCountdownStarted(LobbyCountdown timer)
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Local players this element is currently bound to.
    /// </summary>
    private HashSet<LobbyPlayer> local_players = new HashSet<LobbyPlayer>();
}
