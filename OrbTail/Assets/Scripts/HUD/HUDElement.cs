using UnityEngine;


/// <summary>
/// A HUD element that reacts to the input of a single player.
/// </summary>
public class HUDElement : GUIElement
{
    /// <summary>
    /// Get or set the owner of this HUD element.
    /// </summary>
    public GameObject Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;

            OnOwnerChanged();
        }
    }

    /// <summary>
    /// Get or set the lobby player this HUD element belongs to.
    /// </summary>
    public LobbyPlayer LobbyPlayer { get; set; }

    /// <summary>
    /// Called whenever the owner of this element changes.
    /// </summary>
    protected virtual void OnOwnerChanged()
    {

    }

    /// <summary>
    /// Owner of this HUD element.
    /// </summary>
    private GameObject owner;
}