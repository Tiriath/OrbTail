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

    public void Awake ()
    {
        text_mesh = GetComponent<TextMesh>();

        foreach (LobbyPlayer lobby_player in GameLobby.Instance.lobbySlots)
        {
            if(lobby_player && lobby_player.player_index == player_index)
            {
                this.lobby_player = lobby_player;
                break;
            }
        }

        if(lobby_player)
        {
            lobby_player.PlayerScoreEvent += OnPlayerScore;
            lobby_player.PlayerLeftEvent += OnPlayerLeft;

            text_mesh.color = lobby_player.Color;

            OnPlayerScore(lobby_player);
        }
        else
        {
            gameObject.SetActive(false);                        // Hide the element if no player can be found.
        }
    }

    public void OnDestroy()
    {
        OnPlayerLeft(lobby_player);
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
        gameObject.SetActive(false);

        this.lobby_player = null;

        if(lobby_player)
        {
            lobby_player.PlayerScoreEvent -= OnPlayerScore;
            lobby_player.PlayerLeftEvent -= OnPlayerLeft;
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
