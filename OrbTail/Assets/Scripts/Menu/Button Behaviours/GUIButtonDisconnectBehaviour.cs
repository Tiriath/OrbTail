using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to disconnect from the current lobby.
/// </summary>
public class GUIButtonDisconnectBehaviour : GUIButtonBehaviour
{
    public override void OnInputConfirm()
    {
        base.OnInputConfirm();

        GameLobby.Instance.DisconnectLobby();
    }
}
