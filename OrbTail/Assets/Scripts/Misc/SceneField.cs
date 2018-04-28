using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

/// <summary>
/// Field used to reference a scene via the editor.
/// Thanks random citizen @ https://answers.unity.com/questions/242794/inspector-field-for-scene-asset.html#answer-1204071.
/// </summary>
[System.Serializable]
public class SceneField
{
    /// <summary>
    /// Get the scene name.
    /// </summary>
    public string SceneName
    {
        get
        {
            return scene_name;
        }
    }

    /// <summary>
    /// Check whether this object points to a valid arena.
    /// </summary>
    public bool IsValid
    {
        get
        {
            return scene_name.Length > 0;
        }
    }

    /// <summary>
    /// Convert the scene field implicitly to the scene name.
    /// </summary>
    /// <param name="scene_field">Scene field to convert.</param>
    public static implicit operator string(SceneField scene_field)
    {
        return scene_field.SceneName;
    }

    /// <summary>
    /// Actual scene asset. This will be null on builds.
    /// </summary>
    [SerializeField]
    private Object scene_asset;

    /// <summary>
    /// Scene name.
    /// </summary>
    [SerializeField]
    private string scene_name = "";
}


#if UNITY_EDITOR

/// <summary>
/// Custom property drawer for scene fields.
/// </summary>
[CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);

        SerializedProperty scene_asset = property.FindPropertyRelative("scene_asset");
        SerializedProperty scene_name = property.FindPropertyRelative("scene_name");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        if (scene_asset != null)
        {
            scene_asset.objectReferenceValue = EditorGUI.ObjectField(position, scene_asset.objectReferenceValue, typeof(SceneAsset), false);
            if (scene_asset.objectReferenceValue != null)
            {
                scene_name.stringValue = (scene_asset.objectReferenceValue as SceneAsset).name;
            }
        }

        EditorGUI.EndProperty();
    }
}

#endif