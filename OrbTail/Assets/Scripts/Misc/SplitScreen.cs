using UnityEngine;

/// <summary>
/// Exposes utility functions to handle split-screen content.
/// </summary>
static class SplitScreen
{
    private static readonly Rect[][] kViewports = new Rect[][]
    {
        new Rect[]{ new Rect(0.0f, 0.0f, 1.0f, 1.0f) },
        new Rect[]{ new Rect(0.0f, 0.0f, 0.5f, 1.0f), new Rect(0.5f, 0.0f, 0.5f, 1.0f)},
        new Rect[]{ new Rect(0.0f, 0.0f, 0.5f, 1.0f), new Rect(0.5f, 0.5f, 0.5f, 0.5f), new Rect(0.5f, 0.0f, 0.5f, 0.5f)},
        new Rect[]{ new Rect(0.0f, 0.5f, 0.5f, 0.5f), new Rect(0.5f, 0.5f, 0.5f, 0.5f), new Rect(0.0f, 0.0f, 0.5f, 0.5f), new Rect(0.5f, 0.0f, 0.5f, 0.5f) }
    };

    /// <summary>
    /// Get the camera viewport for a given player considering the total amount of local players.
    /// </summary>
    /// <param name="player_index">Index of the player to get the viewport of.</param>
    /// <param name="local_humans">Number of local human players.</param>
    /// <returns></returns>
    public static Rect GetCameraViewport(int player_index, int local_humans)
    {
        return kViewports[local_humans - 1][player_index];
    }
}