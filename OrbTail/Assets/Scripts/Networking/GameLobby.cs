using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to create and handle the game lobby.
/// </summary>
public class GameLobby : NetworkLobbyManager
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        game_configuration = GetComponent<GameConfiguration>();
    }

    /// <summary>
    /// Called whenever a new scene is loaded.
    /// </summary>
    /// <param name="scene">Scene being loaded.</param>
    /// <param name="mode">Scene loading mode.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == lobbyScene)
        {
            if(game_configuration.game_type == GameType.Offline)
            {
                CreateLobby();          // Offline games are just hosted games where other players are AIs.
            }
            else if(game_configuration.game_type == GameType.Online)
            {
                SearchLobby();          // Attempt to join an existing lobby, if none is found a new one is created.
            }
        }

    }

    /// <summary>
    /// Create the lobby using the current game configuration.
    /// </summary>
    private void CreateLobby()
    {
        
    }

    /// <summary>
    /// Join an existing lobby matching the current game configuration.
    /// </summary>
    private void SearchLobby()
    {

    }

    /// <summary>
    /// Current game configuration component.
    /// </summary>
    private GameConfiguration game_configuration = null;
}
