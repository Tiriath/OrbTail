using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toggle the local player ready status when pressed.
/// </summary>
public class GUIButtonToggleReadyBehaviour : GUIButtonBehaviour
{
    /// <summary>
    /// Text to display when the local player is ready.
    /// </summary>
    public string ready_text = "ready";

    /// <summary>
    /// Text to display when the local player is not ready.
    /// </summary>
    public string not_ready_text = "not ready";

    /// <summary>
    /// Index of the local player controller to bound to.
    /// </summary>
    public int local_player_index = 0;

    public void Start()
    {
        LobbyPlayer.PlayerAuthorityEvent += OnLobbyPlayerAuthority;

        GetComponent<TextMesh>().text = "";
    }

    public void OnDestroy()
    {
        // Unbind from everything!

        LobbyPlayer.PlayerJoinedEvent -= OnLobbyPlayerAuthority;

        if(local_player)
        {
            local_player.PlayerReadyEvent -= OnLobbyPlayerReady;
        }
    }

    public override void OnInputConfirm()
    {
        if(local_player)
        {
            // Toggle ready status.

            if(local_player.readyToBegin)
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
    /// Called whenever a new player joins the lobby.
    /// </summary>
    private void OnLobbyPlayerAuthority(LobbyPlayer lobby_player)
    {
        if(lobby_player.playerControllerId == local_player_index)
        {
            Debug.Assert(local_player == null, "A local player with id " + local_player_index + " was already bound!");

            local_player = lobby_player;
            
            local_player.PlayerReadyEvent += OnLobbyPlayerReady;

            OnLobbyPlayerReady(local_player);
        }
    }

    /// <summary>
    /// Called whenever the ready status of the local player changes.
    /// </summary>
    /// <param name="sender"></param>
    private void OnLobbyPlayerReady(LobbyPlayer lobby_player)
    {
        GetComponent<TextMesh>().text = local_player.readyToBegin ? ready_text : not_ready_text;
    }

    /// <summary>
    /// Local player this element is currently bound to.
    /// </summary>
    private LobbyPlayer local_player = null;
}
