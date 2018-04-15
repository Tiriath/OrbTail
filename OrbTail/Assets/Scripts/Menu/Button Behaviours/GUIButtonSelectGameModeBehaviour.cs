using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to select the game mode.
/// </summary>
public class GUIButtonSelectGameModeBehaviour : GUIButtonBehaviour
{
    /// <summary>
    /// Game mode to select.
    /// </summary>
    public GameMode game_mode;

    public override void OnInputConfirm()
    {
        base.OnInputConfirm();

        GameConfiguration.Instance.game_mode = game_mode;
    }
}
