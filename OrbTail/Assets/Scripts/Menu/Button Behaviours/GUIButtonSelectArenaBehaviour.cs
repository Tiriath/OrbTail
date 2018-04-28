using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to select the game type.
/// </summary>
public class GUIButtonSelectArenaBehaviour : GUIButtonBehaviour
{
    /// <summary>
    /// Arena to select.
    /// </summary>
    public SceneField arena;

    public override void OnInputConfirm()
    {
        base.OnInputConfirm();

        GameConfiguration.Instance.arena = arena;
    }
}
