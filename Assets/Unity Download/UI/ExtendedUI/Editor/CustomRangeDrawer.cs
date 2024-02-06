using UnityEditor;
using UnityEngine;

namespace DIG.Tools
{
    [CustomPropertyDrawer(typeof(CustomRangeAttribute))]
    public class CustomRangeDrawer : PropertyDrawer
    {
      public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Float)
            {
                CustomRangeAttribute range = attribute as CustomRangeAttribute;

                SerializedProperty minProp = property.serializedObject.FindProperty(range.MinVariable);
                SerializedProperty maxProp = property.serializedObject.FindProperty(range.MaxVariable);

                if (minProp != null && maxProp != null && minProp.propertyType == SerializedPropertyType.Float && maxProp.propertyType == SerializedPropertyType.Float)
                {
                    EditorGUI.BeginProperty(position, label, property);

                    float minValue = minProp.floatValue;
                    float maxValue = maxProp.floatValue;

                    float value = EditorGUI.Slider(position, label, property.floatValue, minValue, maxValue);

                    EditorGUI.EndProperty();

                    property.floatValue = value;
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Invalid variable name(s).");
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use CustomRange with float.");
            }
        }
    }
}
