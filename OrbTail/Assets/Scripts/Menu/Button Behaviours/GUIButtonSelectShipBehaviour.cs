﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to select a ship.
/// </summary>
public class GUIButtonSelectShipBehaviour : GUIElement
{
    /// <summary>
    /// Ship to select.
    /// </summary>
    public GameObject ship_prefab;

    public override void OnInputConfirm()
    {
        base.OnInputConfirm();

        // #TODO Add support for local split-screen.

        var master = GameObject.FindGameObjectWithTag(Tags.Master);

        var player_configuration = master.AddComponent<PlayerConfiguration>();

        player_configuration.player_controller_id = 0;
        player_configuration.ship_prefab = ship_prefab;
        player_configuration.is_human = true;
    }
}
