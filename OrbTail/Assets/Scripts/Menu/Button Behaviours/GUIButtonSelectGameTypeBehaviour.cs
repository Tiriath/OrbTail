using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to select the arena.
/// </summary>
public class GUIButtonSelectGameTypeBehaviour : GUIElement
{
    /// <summary>
    /// Game type to select.
    /// </summary>
    public GameType game_type;

    public override void OnInputConfirm()
    {
        base.OnInputConfirm();

        GameConfiguration.Instance.game_type = game_type;
    }
}
