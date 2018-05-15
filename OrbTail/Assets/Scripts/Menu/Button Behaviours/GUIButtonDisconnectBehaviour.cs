using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to disconnect from the current lobby and return to another scene.
/// </summary>
public class GUIButtonDisconnectBehaviour : GUIElement
{
    /// <summary>
    /// Name of the scene to load after being disconnected from the lobby.
    /// </summary>
    public SceneField scene;

    public override void OnInputConfirm()
    {
        base.OnInputConfirm();

        var game_lobby = GameLobby.Instance;

        game_lobby.disconnected_scene = scene;

        game_lobby.Clear();

        game_lobby.DisconnectLobby();
    }
}
