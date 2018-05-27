using UnityEngine;
using System.Linq;

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
    /// Scale of the element when the player is ready.
    /// </summary>
    public float ready_scale = 1.0f;

    /// <summary>
    /// Scale of the element when the player is not ready.
    /// </summary>
    public float not_ready_scale = 0.5f;

    /// <summary>
    /// Alpha of the element when the player is ready.
    /// </summary>
    public float ready_alpha = 1.0f;

    /// <summary>
    /// Alpha of the element when the player is not ready.
    /// </summary>
    public float not_ready_alpha = 0.75f;

    /// <summary>
    /// Time needed to tween the element.
    /// </summary>
    public float tween_time = 0.2f;

    /// <summary>
    /// Default ship icon.
    /// </summary>
    public Sprite default_icon;

    /// <summary>
    /// Associate each ship to its icon.
    /// </summary>
    public ShipIcon[] ship_icons;

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

        var ship_icons = this.ship_icons.Where(ship_icon => ship_icon.ship.name == lobby_player.player_ship).ToArray();

        var ship_sprite = ship_icons.Length > 0 ? ship_icons[0].sprite : default_icon;

        GetComponent<SpriteRenderer>().sprite = ship_sprite;
    }

    /// <summary>
    /// Called whenever the the player ready status changes.
    /// </summary>
    private void OnPlayerReadyChanged(LobbyPlayer lobby_player)
    {
        Debug.Assert(lobby_player == bound_lobby_player);

        var target_scale = lobby_player.readyToBegin ? ready_scale : not_ready_scale;

        iTween.ScaleTo(gameObject, original_scale * target_scale, tween_time);

        iTween.FadeTo(gameObject, lobby_player.readyToBegin ? ready_alpha : not_ready_alpha, tween_time);
    }

    /// <summary>
    /// Called whenever a a player leaves the lobby.
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

/// <summary>
/// Associate a ship to its icon.
/// </summary>
[System.Serializable]
public class ShipIcon
{
    /// <summary>
    /// Ship this icon refers to.
    /// </summary>
    public GameObject ship;

    /// <summary>
    /// Actual sprite to display.
    /// </summary>
    public Sprite sprite;
}