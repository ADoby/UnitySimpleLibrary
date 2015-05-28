#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

namespace SimpleLibrary
{
    [CustomPropertyDrawer(typeof(AttributeInfo))]
    public class AttributeCategory_PropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SerializedProperty FoldOut = property.FindPropertyRelative("FoldOut");
			return FoldOut.boolValue ? SimpleEditor.EditorLineHeight * 3 : SimpleEditor.EditorLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			property.serializedObject.Update();

			SerializedProperty FoldOut = property.FindPropertyRelative("FoldOut");
            SerializedProperty category = property.FindPropertyRelative("category");
            SerializedProperty type = property.FindPropertyRelative("type");
            if (AttributeManager.Instance)
            {
				position.height = SimpleEditor.EditorLineHeight;

				float value = 0f;
				if (AttributeManager.IGetAttribute(category.intValue, type.intValue) != null)
					value = AttributeManager.IGetAttribute(category.intValue, type.intValue).Value;

				FoldOut.boolValue = EditorGUI.Foldout(position, FoldOut.boolValue, 
					string.Format("{0}: {1}_{2}: {3}", 
					label.text, 
					AttributeManager.Instance.GetCategoryName(category.intValue), 
					AttributeManager.Instance.GetAttributeType(category.intValue, type.intValue),
					value));
				if (FoldOut.boolValue)
				{
					EditorGUI.indentLevel++;
					position.y += SimpleEditor.EditorLineHeight;
					category.intValue = EditorGUI.Popup(position, "Attribute Category", category.intValue, AttributeManager.Instance.CategoryNames.ToArray<string>());
					position.y += SimpleEditor.EditorLineHeight;
					type.intValue = EditorGUI.Popup(position, "Attribute Type", type.intValue, AttributeManager.Instance.Categories[category.intValue].Types.ToArray<string>());
					EditorGUI.indentLevel--;
				}
            }

			property.serializedObject.ApplyModifiedProperties();
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
            return foldOut.boolValue ? (EditorGUIUtility.singleLineHeight * 8f)
                + (point_FoldOut.boolValue ? EditorGUIUtility.singleLineHeight * 3f : 0f)
                + (value_FoldOut.boolValue ? EditorGUIUtility.singleLineHeight * 4f : 0f) : EditorGUIUtility.singleLineHeight;
        }

		GUIStyle boldFoldOut, miniButton;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			property.serializedObject.Update();

			miniButton = new GUIStyle(EditorStyles.miniButton);
			miniButton.alignment = TextAnchor.MiddleCenter;
			miniButton.padding = new RectOffset(0, 2, 0, 2);
			boldFoldOut = new GUIStyle(EditorStyles.foldout);
			boldFoldOut.richText = true;

			Attribute attribute;
			try
			{
				attribute = (Attribute)fieldInfo.GetValue(property.serializedObject.targetObject);
			}
			catch (System.Exception)
			{
				EditorGUI.LabelField(position, "Editor could not be shown");
				return;
			}


            //UI specific variables
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
			SerializedProperty minPoints = property.FindPropertyRelative("MinPoints");
            SerializedProperty maxPoints = property.FindPropertyRelative("MaxPoints");


			SerializedProperty DebugMode = property.FindPropertyRelative("DebugMode");

            //ValueSettings
            SerializedProperty valueInfo = property.FindPropertyRelative("ValueInfo");

			SerializedProperty AttrInfo = property.FindPropertyRelative("AttrInfo");
			SerializedProperty category = AttrInfo.FindPropertyRelative("category");
			SerializedProperty type = AttrInfo.FindPropertyRelative("type");


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
            Rect rect14 = rect13;
            rect14.y += EditorGUIUtility.singleLineHeight;
            Rect rect15 = rect14;
            rect15.y += EditorGUIUtility.singleLineHeight;

            if (!point_FoldOut.boolValue)
			{
				int lineCount = 3;
				rect11.y -= EditorGUIUtility.singleLineHeight * lineCount;
				rect12.y -= EditorGUIUtility.singleLineHeight * lineCount;
				rect13.y -= EditorGUIUtility.singleLineHeight * lineCount;
				rect14.y -= EditorGUIUtility.singleLineHeight * lineCount;
				rect15.y -= EditorGUIUtility.singleLineHeight * lineCount;
            }

			float progress = (points.intValue - minPoints.intValue) / (float)(maxPoints.intValue - minPoints.intValue);
			if (maxPoints.intValue == 0)
				progress = 1f;

            float progressWidth = 100f;

			rect1.width -= progressWidth;
			if (DebugMode.boolValue)
				rect1.width -= (20 + 20 + 40 + 40);

            Rect rect1_2 = rect1;
            rect1_2.x += rect1.width;
            rect1_2.width = progressWidth;

			Rect rect1_3 = rect1_2;
			rect1_3.x += rect1_2.width + 1f;
			rect1_3.width = 20f;
			Rect rect1_4 = rect1_3;
			rect1_4.x += 20f;
			Rect rect1_5 = rect1_4;
			rect1_5.x += 20f;
			rect1_5.width = 40f;
			Rect rect1_6 = rect1_5;
			rect1_6.x += 40f;

            foldOut.boolValue = EditorGUI.Foldout(rect1, foldOut.boolValue, string.Format("Attribute:{0}", name.stringValue), boldFoldOut);

            EditorGUI.ProgressBar(rect1_2, enabled.boolValue ? progress : 0f, string.Format("({0}/{1}) = {2:0.00}", points.intValue, maxPoints.intValue, value.floatValue));

			
			if (DebugMode.boolValue)
			{
				if (GUI.Button(rect1_3, "-", miniButton))
				{
					Undo.RecordObject(property.serializedObject.targetObject, "Before Attribute RemovePoint");
					attribute.RemovePoint();
					EditorUtility.SetDirty(property.serializedObject.targetObject);
				}
				if (GUI.Button(rect1_4, "+", miniButton))
				{
					Undo.RecordObject(property.serializedObject.targetObject, "Before Attribute AddPoint");
					attribute.AddPoint();
					EditorUtility.SetDirty(property.serializedObject.targetObject);
				}
				if (GUI.Button(rect1_5, "reset", miniButton))
				{
					Undo.RecordObject(property.serializedObject.targetObject, "Before Attribute ResetPoints");
					attribute.ResetPoints();
					EditorUtility.SetDirty(property.serializedObject.targetObject);
				}
				if (GUI.Button(rect1_6, "max", miniButton))
				{
					Undo.RecordObject(property.serializedObject.targetObject, "Before Attribute MaximizePoints");
					attribute.MaximizePoints();
					EditorUtility.SetDirty(property.serializedObject.targetObject);
				}
			}
			

            EditorGUI.indentLevel++;
            if (foldOut.boolValue)
            {
                name.stringValue = EditorGUI.TextField(rect2, "Name:", name.stringValue);

                if (AttributeManager.Instance.Categories.Count > 0)
                {
                    category.intValue = EditorGUI.Popup(rect3, "Category", category.intValue, AttributeManager.Instance.CategoryNames.ToArray<string>());
                    if (category.intValue < 0 || category.intValue >= AttributeManager.Instance.Categories.Count)
                        category.intValue = 0;
                    type.intValue = EditorGUI.Popup(rect4, "Type", type.intValue, AttributeManager.Instance.Categories[category.intValue].Types.ToArray<string>());
                }
                else
                {
                    EditorGUI.LabelField(rect3, "First: Create some Categories and Types");
                    EditorGUI.LabelField(rect4, "Create them in the AttributeManager");
                }

                enabled.boolValue = EditorGUI.Toggle(rect5, new GUIContent("Enabled", "Enables or Disables this Attribute in Game"), enabled.boolValue);
                locked.boolValue = EditorGUI.Toggle(rect6, new GUIContent("Locked", "A locked Attribute can't be changed in Editor or Game"), locked.boolValue);

                point_FoldOut.boolValue = EditorGUI.Foldout(rect7, point_FoldOut.boolValue, new GUIContent(string.Format("Points(<color=#2277ff>{0}</color>):", points.intValue)), boldFoldOut);

                if (point_FoldOut.boolValue)
                {
					minPoints.intValue = EditorGUI.IntField(rect8, new GUIContent("Min Points:", ""), minPoints.intValue);
                    maxPoints.intValue = EditorGUI.IntField(rect9, new GUIContent("Max Points:", ""), maxPoints.intValue);


					int newStartPoints = EditorGUI.IntSlider(rect10, new GUIContent("Start Points:", ""), startPoints.intValue, minPoints.intValue, maxPoints.intValue);

					/*
					if (newStartPoints != startPoints.intValue && points.intValue == startPoints.intValue)
					{
						Undo.RecordObject(property.serializedObject.targetObject, "Before Attribute ResetPoints");

						points.intValue = newStartPoints;
						Debug.Log("Chaning");
						attribute.Points = newStartPoints;

						value.floatValue = attribute.Value;

						attribute.ResetPoints();
						EditorUtility.SetDirty(property.serializedObject.targetObject);
					}*/

					if (points.intValue == startPoints.intValue)
					{
						points.intValue = newStartPoints;
						value.floatValue = attribute.GetCalculatedValue(points.intValue);
					}
					startPoints.intValue = newStartPoints;
                }

                if (startPoints.intValue < 0)
                    startPoints.intValue = 0;

				if (maxPoints.intValue < minPoints.intValue)
					maxPoints.intValue = minPoints.intValue;


                //AttributeValue
                value_FoldOut.boolValue = EditorGUI.Foldout(rect11, value_FoldOut.boolValue, new GUIContent(string.Format("Value(<color=#aa5522>{0:0.00}</color>):", value.floatValue)), boldFoldOut);

                if (value_FoldOut.boolValue)
                {
                    EditorGUI.LabelField(rect12, new GUIContent(string.Format("Calculation: {0} + {1}x{2} + {3}x({4} -> {5}) = {6}", startValue.floatValue, points.intValue, valuePerPoint.floatValue, valuePerPointMultipliedByCurrentPoints.floatValue, 1, points.intValue, value.floatValue)));

                    startValue.floatValue = EditorGUI.FloatField(rect13, new GUIContent("Start Value:", ""), startValue.floatValue);
                    valuePerPoint.floatValue = EditorGUI.FloatField(rect14, new GUIContent("Value per Point:", ""), valuePerPoint.floatValue);
                    valuePerPointMultipliedByCurrentPoints.floatValue = EditorGUI.FloatField(rect15, new GUIContent("Value per Point (multiplied by current Points):", ""), valuePerPointMultipliedByCurrentPoints.floatValue);
                }
            }
            EditorGUI.indentLevel--;

			property.serializedObject.ApplyModifiedProperties();
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
#endif