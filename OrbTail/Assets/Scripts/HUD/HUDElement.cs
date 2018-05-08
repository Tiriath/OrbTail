using UnityEngine;


/// <summary>
/// Represents a base class for each HUD elements.
/// </summary>
public class HUDElement : MonoBehaviour
{
    /// <summary>
    /// Get or set the owner of this HUD element.
    /// </summary>
    public LobbyPlayer Owner
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
    /// Called whenever the owner of this element changes.
    /// </summary>
    protected virtual void OnOwnerChanged()
    {

    }

    /// <summary>
    /// Player owning this HUD element.
    /// </summary>
    private LobbyPlayer owner;
}