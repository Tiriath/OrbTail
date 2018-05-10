using UnityEngine.SceneManagement;

/// <summary>
/// Causes the parent of this object to be destroyed when the button is pressed.
/// </summary>
public class GUIButtonDestroyParentBehaviour : GUIElement
{
    public override void OnInputConfirm()
    {
        Destroy(transform.parent.gameObject);
    }
}
