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
        LobbyPlayer.PlayerAuthorityEvent -= OnLobbyPlayerAuthority;

        OnLobbyPlayerLeft(bound_local_player);
    }

    public override void OnInputConfirm()
    {
        if(bound_local_player)
        {
            // Toggle ready status.

            if(bound_local_player.readyToBegin)
            {
                bound_local_player.SendNotReadyToBeginMessage();
            }
            else
            {
                bound_local_player.SendReadyToBeginMessage();
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
            Debug.Assert(bound_local_player == null, "A local player with id " + local_player_index + " was already bound!");

            bound_local_player = lobby_player;

            bound_local_player.PlayerReadyEvent += OnLobbyPlayerReady;
            bound_local_player.PlayerLeftEvent += OnLobbyPlayerLeft;

            OnLobbyPlayerReady(bound_local_player);
        }
    }

    /// <summary>
    /// Called whenever the ready status of the local player changes.
    /// </summary>
    private void OnLobbyPlayerReady(LobbyPlayer lobby_player)
    {
        GetComponent<TextMesh>().text = bound_local_player.readyToBegin ? ready_text : not_ready_text;
    }

    /// <summary>
    /// Called whenever the local player leaves.
    /// </summary>
    private void OnLobbyPlayerLeft(LobbyPlayer lobby_player)
    {
        if (bound_local_player && lobby_player == bound_local_player)
        {
            bound_local_player.PlayerReadyEvent -= OnLobbyPlayerReady;
            bound_local_player.PlayerLeftEvent -= OnLobbyPlayerLeft;

            bound_local_player = null;
        }
    }

    /// <summary>
    /// Local player this element is currently bound to.
    /// </summary>
    private LobbyPlayer bound_local_player = null;
}
