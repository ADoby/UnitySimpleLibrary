using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

namespace SimpleLibrary
{
    [CustomPropertyDrawer(typeof(AttributeType))]
    public class AttributeCategory_PropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //We have to lines to draw
            return SimpleEditor.EditorLineHeight * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty category = property.FindPropertyRelative("category");
            SerializedProperty type = property.FindPropertyRelative("type");
            if (AttributeManager.Instance)
            {
                category.intValue = EditorGUI.Popup(position, "Attribute Category", category.intValue, AttributeManager.Instance.CategoryNames.ToArray<string>());
                type.intValue = EditorGUI.Popup(position, "Attribute Type", type.intValue, AttributeManager.Instance.Categories[category.intValue].Types.ToArray<string>());
            }
        }
    }

    [CustomPropertyDrawer(typeof(Attribute))]
    public class Attribute_PropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty foldOut = property.FindPropertyRelative("FoldOut");
            SerializedProperty point_FoldOut = property.FindPropertyRelative("Point_FoldOut");
            SerializedProperty value_FoldOut = property.FindPropertyRelative("Value_FoldOut");
            return foldOut.boolValue ? (EditorGUIUtility.singleLineHeight * 7f)
                + (point_FoldOut.boolValue ? EditorGUIUtility.singleLineHeight * 2f : 0f)
                + (value_FoldOut.boolValue ? EditorGUIUtility.singleLineHeight * 4f : 0f) : EditorGUIUtility.singleLineHeight;
        }

        GUIStyle boldFoldOut;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            boldFoldOut = new GUIStyle(EditorStyles.foldout);
            boldFoldOut.richText = true;

            SerializedProperty foldOut = property.FindPropertyRelative("FoldOut");
            SerializedProperty point_FoldOut = property.FindPropertyRelative("Point_FoldOut");
            SerializedProperty value_FoldOut = property.FindPropertyRelative("Value_FoldOut");

            //AttributeSettings
            SerializedProperty name = property.FindPropertyRelative("Name");
            SerializedProperty enabled = property.FindPropertyRelative("Enabled");
            SerializedProperty locked = property.FindPropertyRelative("Locked");

            //PointSettings
            SerializedProperty points = property.FindPropertyRelative("Points");
            SerializedProperty startPoints = property.FindPropertyRelative("StartPoints");
            SerializedProperty maxPoints = property.FindPropertyRelative("MaxPoints");

            //ValueSettings
            SerializedProperty valueInfo = property.FindPropertyRelative("ValueInfo");
            SerializedProperty FilterAttribute = property.FindPropertyRelative("FilterAttribute");

            SerializedProperty AttrType = property.FindPropertyRelative("AttrType");
            SerializedProperty category = AttrType.FindPropertyRelative("category");
            SerializedProperty type = AttrType.FindPropertyRelative("type");


            SerializedProperty SelectedName = property.FindPropertyRelative("SelectedName");
            SerializedProperty SelectedFilter = property.FindPropertyRelative("SelectedFilter");

            SerializedProperty value = valueInfo.FindPropertyRelative("Value");
            SerializedProperty startValue = valueInfo.FindPropertyRelative("StartValue");
            SerializedProperty valuePerPoint = valueInfo.FindPropertyRelative("ValuePerPoint");
            SerializedProperty valuePerPointMultipliedByCurrentPoints = valueInfo.FindPropertyRelative("ValuePerPointMultipliedByCurrentPoints");

            Rect rect1 = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect rect2 = rect1;
            rect2.y += EditorGUIUtility.singleLineHeight;
            Rect rect3 = rect2;
            rect3.y += EditorGUIUtility.singleLineHeight;
            Rect rect4 = rect3;
            rect4.y += EditorGUIUtility.singleLineHeight;
            Rect rect5 = rect4;
            rect5.y += EditorGUIUtility.singleLineHeight;
            Rect rect6 = rect5;
            rect6.y += EditorGUIUtility.singleLineHeight;
            Rect rect7 = rect6;
            rect7.y += EditorGUIUtility.singleLineHeight;
            Rect rect8 = rect7;
            rect8.y += EditorGUIUtility.singleLineHeight;
            Rect rect9 = rect8;
            rect9.y += EditorGUIUtility.singleLineHeight;
            Rect rect10 = rect9;
            rect10.y += EditorGUIUtility.singleLineHeight;
            Rect rect11 = rect10;
            rect11.y += EditorGUIUtility.singleLineHeight;
            Rect rect12 = rect11;
            rect12.y += EditorGUIUtility.singleLineHeight;
            Rect rect13 = rect12;
            rect13.y += EditorGUIUtility.singleLineHeight;

            if (!point_FoldOut.boolValue)
            {
                rect8.y -= EditorGUIUtility.singleLineHeight * 2f;
                rect9.y -= EditorGUIUtility.singleLineHeight * 2f;
                rect10.y -= EditorGUIUtility.singleLineHeight * 2f;
                rect11.y -= EditorGUIUtility.singleLineHeight * 2f;
                rect12.y -= EditorGUIUtility.singleLineHeight * 2f;
            }

            float progress = (points.intValue - startPoints.intValue) / (float)maxPoints.intValue;
            if (maxPoints.intValue == 0)
                progress = 1f;


            float progressWidth = 100f;

            Rect rect1_2 = rect1;
            rect1.width -= progressWidth;
            rect1_2.x = rect1.x + rect1.width;
            rect1_2.width = progressWidth;

            foldOut.boolValue = EditorGUI.Foldout(rect1, foldOut.boolValue, label, boldFoldOut);

            //category.intValue = EditorGUI.Popup(rect1_1, "Attribute Category", category.intValue, AttributeManager.Instance.CategoryNames.ToArray<string>());

            //SelectedName.intValue = EditorGUI.Popup(rect1_1, SelectedName.intValue, AttributeManager.Instance.AttributeNames.ToArray());
            /*string tempName = EditorGUI.TextField(rect1_1, name.stringValue);
            if (tempName != name.stringValue)
            {
                name.stringValue = tempName;
            }*/

            EditorGUI.ProgressBar(rect1_2, enabled.boolValue ? progress : 0f, string.Format("({0}/{1}) = {2:0.00}", points.intValue, maxPoints.intValue, value.floatValue));

            EditorGUI.indentLevel++;
            if (foldOut.boolValue)
            {
                if (AttributeManager.Instance.Categories.Count > 0)
                {
                    category.intValue = EditorGUI.Popup(rect2, "Category", category.intValue, AttributeManager.Instance.CategoryNames.ToArray<string>());
                    if (category.intValue < 0 || category.intValue >= AttributeManager.Instance.Categories.Count)
                        category.intValue = 0;
                    type.intValue = EditorGUI.Popup(rect3, "Type", type.intValue, AttributeManager.Instance.Categories[category.intValue].Types.ToArray<string>());
                }
                else
                {
                    EditorGUI.LabelField(rect2, "First: Create some Categories and Types");
                    EditorGUI.LabelField(rect3, "Create them in the AttributeManager");
                }

                //SelectedFilter.intValue = EditorGUI.Popup(rect2, "Filter:", SelectedFilter.intValue, AttributeManager.Instance.Filters.ToArray());

                enabled.boolValue = EditorGUI.Toggle(rect4, new GUIContent("Enabled", "Enables or Disables this Attribute in Game"), enabled.boolValue);
                locked.boolValue = EditorGUI.Toggle(rect5, new GUIContent("Locked", "A locked Attribute can't be changed in Editor or Game"), locked.boolValue);

                point_FoldOut.boolValue = EditorGUI.Foldout(rect6, point_FoldOut.boolValue, new GUIContent(string.Format("Points(<color=#2277ff>{0}</color>):", points.intValue)), boldFoldOut);

                if (point_FoldOut.boolValue)
                {
                    int newStartPoints = EditorGUI.IntField(rect7, new GUIContent("Start Points:", ""), startPoints.intValue);
                    if (points.intValue == startPoints.intValue)
                        points.intValue = newStartPoints;
                    startPoints.intValue = newStartPoints;

                    maxPoints.intValue = EditorGUI.IntField(rect8, new GUIContent("Max Points:", ""), maxPoints.intValue);
                }

                if (startPoints.intValue < 0)
                    startPoints.intValue = 0;

                if (maxPoints.intValue < startPoints.intValue)
                    maxPoints.intValue = startPoints.intValue;

                if (points.intValue < startPoints.intValue)
                    points.intValue = startPoints.intValue;
                if (points.intValue > maxPoints.intValue)
                    points.intValue = maxPoints.intValue;

                //AttributeValue
                value_FoldOut.boolValue = EditorGUI.Foldout(rect9, value_FoldOut.boolValue, new GUIContent(string.Format("Value(<color=#aa5522>{0:0.00}</color>):", value.floatValue)), boldFoldOut);

                if (value_FoldOut.boolValue)
                {
                    EditorGUI.LabelField(rect10, new GUIContent(string.Format("Calculation: {0} + {1}x{2} + {3}x({4} -> {5}) = {6}", startValue.floatValue, points.intValue, valuePerPoint.floatValue, valuePerPointMultipliedByCurrentPoints.floatValue, 1, points.intValue, value.floatValue)));

                    startValue.floatValue = EditorGUI.FloatField(rect11, new GUIContent("Start Value:", ""), startValue.floatValue);
                    valuePerPoint.floatValue = EditorGUI.FloatField(rect12, new GUIContent("Value per Point:", ""), valuePerPoint.floatValue);
                    valuePerPointMultipliedByCurrentPoints.floatValue = EditorGUI.FloatField(rect13, new GUIContent("Value per Point (multiplied by current Points):", ""), valuePerPointMultipliedByCurrentPoints.floatValue);
                }
            }
            EditorGUI.indentLevel--;
            //currentAttribute.RecalculateValue();
        }


        public void ProgressBar(float value, string label, float moveX = 0f, float moveY = 0f, float height = 0)
        {
            if (height == 0)
                height = 18;
            Rect rect = GUILayoutUtility.GetRect(18, height);
            rect.x += moveX;
            rect.width -= moveX;
            rect.y += moveY;
            rect.height -= moveY;

            EditorGUI.ProgressBar(rect, value, label);
        }
    }
}