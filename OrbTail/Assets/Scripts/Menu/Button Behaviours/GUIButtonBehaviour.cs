using UnityEngine;

/// <summary>
/// Base class for UI objects reacting to press input.
/// </summary>
public class GUIButtonBehaviour : MonoBehaviour
{
    /// <summary>
    /// Whether this button requires a double tap in order to be confirmed.
    /// </summary>
    public bool double_tap = false;

    /// <summary>
    /// Called whenever the input starts interacting with this button.
    /// </summary>
    public virtual void OnInputEnter() { }

    /// <summary>
    /// Called whenever the input stop interacting with this button.
    /// </summary>
    public virtual void OnInputLeave() { }

    /// <summary>
    /// Called whenever the input activates this button.
    /// </summary>
    public virtual void OnInputConfirm() { }
}


