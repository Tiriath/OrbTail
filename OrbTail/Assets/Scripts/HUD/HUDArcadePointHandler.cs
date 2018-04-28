using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Widget used to show players' score.
/// </summary>
public class HUDArcadePointHandler : MonoBehaviour
{
    public void Start ()
    {
        foreach (LobbyPlayer lobby_player in GameLobby.Instance.lobbySlots)
        {
            if(lobby_player != null)
            {
                AddScoreIndicator(lobby_player);
            }
        }
    }

    public void OnDestroy()
    {
        foreach (LobbyPlayer lobby_player in GameLobby.Instance.lobbySlots)
        {
            if(lobby_player != null)
            {
                lobby_player.PlayerScoreEvent -= OnPlayerScore;
            }
        }
    }

    /// <summary>
    /// Create a new score indicator for the provided lobby player.
    /// </summary>
    private void AddScoreIndicator(LobbyPlayer lobby_player)
    {
        var textPoint = (GameObject) GameObject.Instantiate(Resources.Load(pathPointsPrefab));
        var textMesh = textPoint.GetComponent<TextMesh>();

        textPoint.transform.parent = gameObject.transform;
        textPoint.transform.localPosition = new Vector3(xOrigin + distanceBetweenScoresX * gameIdentityTextMeshes.Count, yOrigin, 0f);
        
        textMesh.color = lobby_player.Color;
        textMesh.text = string.Format("{0:0000}", lobby_player.score);

        gameIdentityTextMeshes.Add(lobby_player, textMesh);

        lobby_player.PlayerScoreEvent += OnPlayerScore;
    }

    /// <summary>
    /// Called whenever a player score changes.
    /// </summary>
    private void OnPlayerScore(LobbyPlayer lobby_player)
    {
        gameIdentityTextMeshes[lobby_player].text = string.Format("{0:0000}", lobby_player.score);
    }

    private const float xOrigin = -6f;
    private const float yOrigin = 5f;
    private const float distanceBetweenScoresY = 0.5f;
    private const float distanceBetweenScoresX = 2.5f;
    private const string pathPointsPrefab = "Prefabs/HUD/PointText";
    private Dictionary<LobbyPlayer, TextMesh> gameIdentityTextMeshes = new Dictionary<LobbyPlayer, TextMesh>();
}
