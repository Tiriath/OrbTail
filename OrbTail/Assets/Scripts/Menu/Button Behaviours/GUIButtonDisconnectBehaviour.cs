using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script used to disconnect from the current lobby and return to another scene.
/// </summary>
public class GUIButtonDisconnectBehaviour : GUIElement
{
    /// <summary>
    /// Name of the scene to load after being disconnected from the lobby.
    /// </summary>
    public SceneField scene;

    public void Start()
    {
        GameLobby.Instance.offlineScene = scene;
    }
    public override void OnInputConfirm()
    {
        base.OnInputConfirm();

        var game_lobby = GameLobby.Instance;

        game_lobby.Clear();

        game_lobby.DisconnectLobby();

        SceneManager.LoadSceneAsync(scene);
    }
}
