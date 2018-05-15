using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to show the status of a lobby player inside the lobby scene.
/// </summary>
public class GUILobbyPlayer : MonoBehaviour
{
    /// <summary>
    /// Index of the player this GUI element should react to.
    /// </summary>
    public int index = -1;

    /// <summary>
    /// Scale of the element when the player it represents is ready.
    /// </summary>
    public float ready_scale = 1.0f;

    /// <summary>
    /// Scale of the element when the player it represents is not ready.
    /// </summary>
    public float not_ready_scale = 0.5f;

    /// <summary>
    /// Time needed to tween the element.
    /// </summary>
    public float tween_time = 0.2f;

    public void Start ()
    {
        LobbyPlayer.PlayerJoinedEvent += OnLobbyPlayerJoined;
        LobbyPlayer.PlayerLeftEvent += OnLobbyPlayerLeft;

        original_scale = gameObject.transform.localScale;

        gameObject.transform.localScale = Vector3.zero;
    }

    public void OnDestroy()
    {
        LobbyPlayer.PlayerJoinedEvent -= OnLobbyPlayerJoined;
        LobbyPlayer.PlayerLeftEvent -= OnLobbyPlayerLeft;

        if(bound_lobby_player)
        {
            bound_lobby_player.PlayerIndexChangedEvent -= OnPlayerIndexChanged;
            bound_lobby_player.PlayerShipChangedEvent -= OnPlayerShipChanged;
            bound_lobby_player.PlayerReadyEvent -= OnPlayerReadyChanged;
        }
    }

    /// <summary>
    /// Called whenever a new player joins the lobby.
    /// </summary>
    private void OnLobbyPlayerJoined(LobbyPlayer lobby_player)
    {
        lobby_player.PlayerIndexChangedEvent += OnPlayerIndexChanged;

        OnPlayerIndexChanged(lobby_player);
    }

    /// <summary>
    /// Called whenever a player index changes.
    /// </summary>
    private void OnPlayerIndexChanged(LobbyPlayer lobby_player)
    {
        if(lobby_player.player_index == index)
        {
            Debug.Assert(bound_lobby_player == null, "The element cannot be bound to more than one player!");

            bound_lobby_player = lobby_player;

            lobby_player.PlayerShipChangedEvent += OnPlayerShipChanged;
            lobby_player.PlayerReadyEvent += OnPlayerReadyChanged;

            OnPlayerShipChanged(lobby_player);
            OnPlayerReadyChanged(lobby_player);
        }
    }

    /// <summary>
    /// Called whenever a player ship changes.
    /// </summary>
    private void OnPlayerShipChanged(LobbyPlayer lobby_player)
    {
        Debug.Assert(lobby_player == bound_lobby_player);

        // #TODO Change the icon!
    }

    /// <summary>
    /// Called whenever the the player ready status changes.
    /// </summary>
    private void OnPlayerReadyChanged(LobbyPlayer lobby_player)
    {
        Debug.Assert(lobby_player == bound_lobby_player);

        var target_scale = lobby_player.readyToBegin ? ready_scale : not_ready_scale;

        iTween.ScaleTo(gameObject, original_scale * target_scale, tween_time);
    }

    /// <summary>
    /// Called whenever a a player lefts the lobby.
    /// </summary>
    private void OnLobbyPlayerLeft(LobbyPlayer lobby_player)
    {
        if (bound_lobby_player && lobby_player == bound_lobby_player)
        {
            lobby_player.PlayerIndexChangedEvent -= OnPlayerIndexChanged;
            lobby_player.PlayerShipChangedEvent -= OnPlayerShipChanged;
            lobby_player.PlayerReadyEvent -= OnPlayerReadyChanged;

            bound_lobby_player = null;

            iTween.ScaleTo(gameObject, Vector3.zero, tween_time);
        }
    }

    /// <summary>
    /// Lobby player this element is currently bound to.
    /// </summary>
    private LobbyPlayer bound_lobby_player = null;

    /// <summary>
    /// Original element scale.
    /// </summary>
    private Vector3 original_scale;
}
