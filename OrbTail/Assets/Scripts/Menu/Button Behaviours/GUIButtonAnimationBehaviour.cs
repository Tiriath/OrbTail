using UnityEngine;

/// <summary>
/// Causes the button to appear pressed when the input starts interacting with the button.
/// </summary>
public class GUIButtonAnimationBehaviour : GUIButtonBehaviour
{
    /// <summary>
    /// Scale factor when the button is pressed.
    /// </summary>
    public float scale = 0.85f;

    /// <summary>
    /// Time needed to press the button completely.
    /// </summary>
    public float time = 0.2f;

    public override void OnInputInteract()
    {
        iTween.ScaleTo(gameObject, original_scale * scale, time);
    }

    public override void OnInputLeave()
    {
        iTween.ScaleTo(gameObject, original_scale, time);
    }

    void Start()
    {
        original_scale = gameObject.transform.localScale;
    }

    /// <summary>
    /// Original button scale.
    /// </summary>
    private Vector3 original_scale;
}
