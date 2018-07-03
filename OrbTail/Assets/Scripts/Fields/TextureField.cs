using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

/// <summary>
/// Field used to reference a texture via the editor.
/// Thanks random citizen @ https://answers.unity.com/questions/242794/inspector-field-for-scene-asset.html#answer-1204071.
/// </summary>
[System.Serializable]
public class TextureField
{
    /// <summary>
    /// Get the texture name.
    /// </summary>
    public string TextureName
    {
        get
        {
            return texture_name;
        }
    }

    /// <summary>
    /// Check whether this object points to a valid texture.
    /// </summary>
    public bool IsValid
    {
        get
        {
            return texture_name.Length > 0;
        }
    }

    /// <summary>
    /// Convert the texture field implicitly to the texture name.
    /// </summary>
    /// <param name="texture_field">Texture field to convert.</param>
    public static implicit operator string(TextureField texture_field)
    {
        return texture_field.TextureName;
    }

    /// <summary>
    /// Actual texture asset. This will be null on builds.
    /// </summary>
    [SerializeField]
    private Object texture_asset;

    /// <summary>
    /// Texture name.
    /// </summary>
    [SerializeField]
    private string texture_name = "";
}


#if UNITY_EDITOR

/// <summary>
/// Custom property drawer for texture fields.
/// </summary>
[CustomPropertyDrawer(typeof(TextureField))]
public class TextureFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        const string kResourcesPrefix = "Assets/Resources/";

        EditorGUI.BeginProperty(position, GUIContent.none, property);

        SerializedProperty texture_asset = property.FindPropertyRelative("texture_asset");
        SerializedProperty texture_name = property.FindPropertyRelative("texture_name");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        if (texture_asset != null)
        {
            texture_asset.objectReferenceValue = EditorGUI.ObjectField(position, texture_asset.objectReferenceValue, typeof(Texture), false);
            if (texture_asset.objectReferenceValue != null)
            {
                var texture_resource = texture_asset.objectReferenceValue as Texture;

                var path = AssetDatabase.GetAssetPath(texture_resource);

                if(path.StartsWith(kResourcesPrefix))
                {
                    // Strip away both the prefix and the extension. Kinda hacky, but it'll do the trick.

                    texture_name.stringValue = path.Substring(kResourcesPrefix.Length, path.LastIndexOf('.') - kResourcesPrefix.Length);
                }
                else
                {
                    texture_name.stringValue = path;
                }
            }
        }

        EditorGUI.EndProperty();
    }
}

#endif