using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

/// <summary>
/// Post processing component that can be disabled according to current quality levels.
/// </summary>
public class ScalablePostProcessing : PostProcessingBehaviour
{
    /// <summary>
    /// Quality level below of which post processing is disabled.
    /// </summary>
    int quality_level_threshold = 0;

    void Start ()
    {
        // Kill any post process when the quality level is low enough (like on mobile)

        int quality_level = QualitySettings.GetQualityLevel();

        enabled = (quality_level > quality_level_threshold);
    }
}
