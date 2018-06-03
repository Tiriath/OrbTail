using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Script used to select a ship.
/// </summary>
public class GUIButtonSelectShipBehaviour : GUIElement
{
    /// <summary>
    /// Ship to select.
    /// </summary>
    public GameObject ship_prefab;

    public void Awake()
    {
        selector = FindObjectsOfType<GUIShipSelector>().Where(s => s.player_index == 0).First();
    }

    public override void OnInputConfirm()
    {
        base.OnInputConfirm();

        selector.PlayerJoined = true;
        selector.Selection = this;
        selector.Confirmed = true;
    }

    /// <summary>
    /// Underlying selector.
    /// </summary>
    private GUIShipSelector selector;
}
