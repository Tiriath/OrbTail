using UnityEngine;

/// <summary>
/// Causes the application to exit when the button pressed.
/// </summary>
public class GUIButtonExitApplicationBehaviour : GUIButtonBehaviour
{
    public override void OnInputConfirm()
    {
        Application.Quit();
    }
}