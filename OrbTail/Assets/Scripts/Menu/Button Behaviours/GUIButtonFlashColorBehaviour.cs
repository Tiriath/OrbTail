using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Causes the component supporting the "color" property to flash over time when pressed.
/// </summary>
public class GUIButtonFlashColorBehaviour : GUIElement
{
    /// <summary>
    /// Flash color.
    /// </summary>
    public Color flash_color;

    /// <summary>
    /// Tween duration, in seconds.
    /// </summary>
    public float duration;

    public void Awake()
    {
        foreach(var text in GetComponentsInChildren<TextMesh>())
        {
            texts.Add(text, text.color);
        }

        foreach(var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprites.Add(sprite, sprite.color);
        }
    }

    public override void OnInputConfirm()
    {
        iTween.ValueTo(this.gameObject, iTween.Hash(
            "from", 0.0f,
            "to", 1.0f,
            "time", duration,
            "onUpdate", "OnUpdate"));
    }

    /// <summary>
    /// Called whenever the progress of the flash changes.
    /// </summary>
    private void OnUpdate(float alpha)
    {
        // Cubic ease-out

        alpha--;
        alpha = alpha * alpha * alpha + 1.0f;

        foreach(var text in texts)
        {
            text.Key.color = Color.Lerp(flash_color, text.Value, alpha);
        }

        foreach (var sprite in sprites)
        {
            sprite.Key.color = Color.Lerp(flash_color, sprite.Value, alpha);
        }
    }

    /// <summary>
    /// List of text meshs handled by this behaviour.
    /// </summary>
    private Dictionary<TextMesh, Color> texts = new Dictionary<TextMesh, Color>();

    /// <summary>
    /// List of sprite renderers handled by this behaviour.
    /// </summary>
    private Dictionary<SpriteRenderer, Color> sprites = new Dictionary<SpriteRenderer, Color>();
}
