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
    /// Post processing profiles, for each supported quality level.
    /// </summary>
    public PostProcessingProfile[] profiles;

    void Start ()
    {
        // Switch post-processing profile according to the current quality level.

        if(profiles != null && profiles.Length > 0)
        {
            int quality_level = Mathf.Clamp(QualitySettings.GetQualityLevel(), 0, profiles.Length - 1);

            profile = profiles[quality_level];
        }
    }
}
