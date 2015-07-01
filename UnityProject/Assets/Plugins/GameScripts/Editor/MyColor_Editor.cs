using UnityEngine;
using System.Collections;
using UnityEditor;
using SimpleLibrary;

[CustomPropertyDrawer(typeof(MyColor))]
public class MyColor_Editor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return  SimpleEditor.EditorLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.serializedObject.Update();

        SerializedProperty r = property.FindPropertyRelative("r");
        SerializedProperty g = property.FindPropertyRelative("g");
        SerializedProperty b = property.FindPropertyRelative("b");
        SerializedProperty a = property.FindPropertyRelative("a");

        Color color = new Color(r.intValue / 255f, g.intValue / 255f, b.intValue / 255f, a.intValue / 255f);
        color = EditorGUI.ColorField(position, label, color);

        r.intValue = (int)(color.r* 255f);
        g.intValue = (int)(color.g * 255f);
        b.intValue = (int)(color.b * 255f);
        a.intValue = (int)(color.a * 255f);


        property.serializedObject.ApplyModifiedProperties();
    }
}
