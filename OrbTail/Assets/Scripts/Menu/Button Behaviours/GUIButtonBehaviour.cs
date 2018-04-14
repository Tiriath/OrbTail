using UnityEngine;

/// <summary>
/// Base class for UI objects reacting to press input.
/// </summary>
public class GUIButtonBehaviour : MonoBehaviour
{
    /// <summary>
    /// Called whenever the input starts interacting with this button.
    /// </summary>
    public virtual void OnInputInteract() { }

    /// <summary>
    /// Called whenever the input stop interacting with this button.
    /// </summary>
    public virtual void OnInputLeave() { }

    /// <summary>
    /// Called whenever the input activates this button.
    /// </summary>
    public virtual void OnInputConfirm() { }
}


