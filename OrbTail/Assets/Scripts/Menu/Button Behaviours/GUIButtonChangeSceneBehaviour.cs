using UnityEngine.SceneManagement;

/// <summary>
/// Causes a new scene to be loaded when the button is pressed.
/// </summary>
public class GUIButtonChangeSceneBehaviour : GUIElement
{
    /// <summary>
    /// Name of the scene to load when this button is pressed.
    /// </summary>
    public SceneField scene;

    public override void OnInputConfirm()
    {
        if (scene.IsValid)
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }
}
