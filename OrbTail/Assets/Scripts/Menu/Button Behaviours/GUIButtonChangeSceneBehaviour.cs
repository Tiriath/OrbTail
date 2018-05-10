using UnityEngine.SceneManagement;

/// <summary>
/// Causes a new scene to be loaded when the button is pressed.
/// </summary>
public class GUIButtonChangeSceneBehaviour : GUIElement
{
    /// <summary>
    /// Name of the scene to load when this button is pressed.
    /// </summary>
    public string scene;

    public override void OnInputConfirm()
    {
        if (scene.Length > 0)
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }
}
