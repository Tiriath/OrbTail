using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour used to take screenshot from a camera.
/// </summary>
public class Screenshot : MonoBehaviour
{
    /// <summary>
    /// How many screenshot to take.
    /// </summary>
    public int screenshots = 6;

    /// <summary>
    /// Camera movement after each successful screenshot.
    /// </summary>
    public Vector3 step = new Vector3(10.0f, 0.0f, 0.0f);

    /// <summary>
    /// Texture format.
    /// </summary>
    public TextureFormat color_format = TextureFormat.ARGB32;

    /// <summary>
    /// Path for the screenshots.
    /// </summary>
    public string path = "./Screenshots/screenshot_{0}.png";

    public int width = 1024;

    public int height = 1024;

    void Start ()
    {
        screenshot_camera = GetComponent<Camera>();

        screenshots_left = screenshots;
    }

    //private void OnRenderImage(RenderTexture source, RenderTexture destination)
    void Update()
    {
        if (screenshots_left > 0)
        {
            --screenshots_left;

            RenderTexture render_target = new RenderTexture(width, height, 24);

            var screenshot = new Texture2D(width, height, color_format, false);

            // Take the screenshot.

            screenshot_camera.targetTexture = render_target;

            screenshot_camera.Render();

            // Read texture data.

            RenderTexture.active = render_target;

            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

            RenderTexture.active = null;

            screenshot_camera.targetTexture = null;

            // Save the screenshot

            var bytes = screenshot.EncodeToPNG();

            var screenshot_path = string.Format(path, screenshots_left);

            System.IO.File.WriteAllBytes(screenshot_path, bytes);

            Debug.Log(string.Format("Took screenshot '{0}'", screenshot_path));

            RenderTexture.active = null;

            // Move
            transform.position += step;
        }
    }

    /// <summary>
    /// Camera used to perform the screenshot.
    /// </summary>
    Camera screenshot_camera = null;

    /// <summary>
    /// Number of screenshot yet to take.
    /// </summary>
    private int screenshots_left = 0;
}
