using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace SimpleLibrary
{
    [CustomPropertyDrawer(typeof(Timer))]
    public class Timer_Editor : PropertyDrawer
    {
        //The height of this property is based on how many child-properties are gonna be drawn
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lineCount = 1;

            SerializedProperty Foldout = property.FindPropertyRelative("Foldout");
            if (Foldout.boolValue)
            {
                lineCount += 1;

                SerializedProperty MyType = property.FindPropertyRelative("MyType");

                switch ((Timer.TimerType)MyType.enumValueIndex)
                {
                    case Timer.TimerType.CONST:
                        lineCount += 1;
                        break;
                    case Timer.TimerType.RANDOM_CURVE:
                        lineCount += 2;
                        break;
                    case Timer.TimerType.RANDOM_BETWEEN_TWO_CONSTANTS:
                        lineCount += 2;
                        break;
                    case Timer.TimerType.RANDOM_BETWEEN_TWO_CURVES:
                        lineCount += 3;
                        break;
                    default:
                        break;
                }
            }

            return SimpleEditor.EditorLineHeight * lineCount;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            
            SerializedProperty Foldout = property.FindPropertyRelative("Foldout");

            SerializedProperty MyType = property.FindPropertyRelative("MyType");

            SerializedProperty Time1 = property.FindPropertyRelative("Time1");
            SerializedProperty Time2 = property.FindPropertyRelative("Time2");
            SerializedProperty Curve1 = property.FindPropertyRelative("Curve1");
            SerializedProperty Curve2 = property.FindPropertyRelative("Curve2");

            SerializedProperty ValueMultiplier = property.FindPropertyRelative("ValueMultiplier");

            SerializedProperty timer = property.FindPropertyRelative("timer");
            SerializedProperty CurrentTimeValue = property.FindPropertyRelative("CurrentTimeValue");

            float procentage = 0f;
            if (CurrentTimeValue.floatValue > 0f)
                procentage = Mathf.Clamp01(timer.floatValue / CurrentTimeValue.floatValue);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            string foldOutLabel = string.Format("Name: {0}", label.text);

            float foldOutNameWidth= foldOutLabel.Length * 7f;
            Rect rectFoldout = new Rect(rect.x, rect.y, foldOutNameWidth, SimpleEditor.EditorLineHeight);
            Rect rectProgressbar = new Rect(rect.x + foldOutNameWidth, rect.y, rect.width - foldOutNameWidth, SimpleEditor.EditorLineHeight);

            Foldout.boolValue = EditorGUI.Foldout(rectFoldout, Foldout.boolValue, new GUIContent(foldOutLabel, label.image, label.tooltip));
            EditorGUI.ProgressBar(rectProgressbar, procentage, string.Format("{0:0.0} => {1:0.0}", timer.floatValue, CurrentTimeValue.floatValue));

            if (Foldout.boolValue)
            {
                Rect rect1 = new Rect(rect.x, rect.y + SimpleEditor.EditorLineHeight, rect.width, SimpleEditor.EditorLineHeight);
                Rect rect2 = new Rect(rect.x, rect.y + 2f * SimpleEditor.EditorLineHeight, rect.width, SimpleEditor.EditorLineHeight);
                Rect rect3 = new Rect(rect.x, rect.y + 3f * SimpleEditor.EditorLineHeight, rect.width, SimpleEditor.EditorLineHeight);
                Rect rect4 = new Rect(rect.x, rect.y + 4f * SimpleEditor.EditorLineHeight, rect.width, SimpleEditor.EditorLineHeight);

                EditorGUI.PropertyField(rect1, MyType);

                switch ((Timer.TimerType)MyType.enumValueIndex)
                {
                    case Timer.TimerType.CONST:
                        EditorGUI.PropertyField(rect2, Time1);
                        break;
                    case Timer.TimerType.RANDOM_CURVE:
                        EditorGUI.PropertyField(rect2, Curve1);
                        EditorGUI.PropertyField(rect3, ValueMultiplier);
                        break;
                    case Timer.TimerType.RANDOM_BETWEEN_TWO_CONSTANTS:
                        EditorGUI.PropertyField(rect2, Time1);
                        EditorGUI.PropertyField(rect3, Time2);
                        break;
                    case Timer.TimerType.RANDOM_BETWEEN_TWO_CURVES:
                        EditorGUI.PropertyField(rect2, Curve1);
                        EditorGUI.PropertyField(rect3, Curve2);
                        EditorGUI.PropertyField(rect4, ValueMultiplier);
                        break;
                    default:
                        break;
                }
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}