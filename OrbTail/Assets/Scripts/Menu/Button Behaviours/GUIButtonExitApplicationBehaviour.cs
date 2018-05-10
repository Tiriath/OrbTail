using UnityEngine;

/// <summary>
/// Causes the application to exit when the button pressed.
/// </summary>
public class GUIButtonExitApplicationBehaviour : GUIElement
{
    public override void OnInputConfirm()
    {
        Application.Quit();
    }
}