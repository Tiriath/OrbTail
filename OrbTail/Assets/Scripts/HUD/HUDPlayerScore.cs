using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script used to show a player score.
/// </summary>
public class HUDPlayerScore : MonoBehaviour
{
    /// <summary>
    /// Index of the player this element refers to.
    /// </summary>
    public int player_index = -1;

    /// <summary>
    /// Fade duration when this element fades out, in seconds.
    /// </summary>
    public float fade_duration = 0.4f;

    public void Awake ()
    {
        text_mesh = GetComponent<TextMesh>();

        text_mesh.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        LobbyPlayer.PlayerLeftEvent += OnPlayerLeft;

        foreach (LobbyPlayer lobby_player in GameLobby.Instance.lobbySlots)
        {
            if(lobby_player && lobby_player.player_index == player_index)
            {
                this.lobby_player = lobby_player;

                lobby_player.PlayerScoreEvent += OnPlayerScore;

                OnPlayerScore(lobby_player);

                break;
            }
        }

        BaseGameMode.Instance.MatchCountdownEvent += OnMatchCountdown;
    }

    public void OnDestroy()
    {
        var game_mode = BaseGameMode.Instance;

        if(game_mode)
        {
            game_mode.MatchCountdownEvent -= OnMatchCountdown;
        }

        OnPlayerLeft(lobby_player);

        LobbyPlayer.PlayerLeftEvent -= OnPlayerLeft;
    }

    /// <summary>
    /// Called whenever the game mode countdown starts.
    /// </summary>
    private void OnMatchCountdown(BaseGameMode game_mode)
    {
        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchCountdown;

        if(lobby_player)
        {
            text_mesh.color = lobby_player.Color;
        }
    }

    /// <summary>
    /// Called whenever a player score changes.
    /// </summary>
    private void OnPlayerScore(LobbyPlayer lobby_player)
    {
        text_mesh.text = string.Format("{0:0000}", lobby_player.score);
    }

    /// <summary>
    /// Called whenever a player leaves.
    /// </summary>
    private void OnPlayerLeft(LobbyPlayer lobby_player)
    {
        if(lobby_player && lobby_player.player_index == player_index)
        {
            iTween.FadeTo(gameObject, 0.0f, fade_duration);

            lobby_player.PlayerScoreEvent -= OnPlayerScore;

            this.lobby_player = null;
        }
    }

    /// <summary>
    /// Player this element is bound to.
    /// </summary>
    private LobbyPlayer lobby_player;

    /// <summary>
    /// Element used to displayer player's score.
    /// </summary>
    private TextMesh text_mesh;
}
