#if UNITY_EDITOR

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleLibrary
{
	[CustomEditor(typeof(AttributeManager))]
	public class AttributeManager_Editor : Editor
	{
		const float AddAttributeButtonWidth = 95f;

        const float CategoryFoldOutWidth = 95f;
        const int CategorySmallButtonCount = 3;
        const float TypeFoldOutWidth = 90f;
        const int TypeSmallButtonCount = 3;
        const float SmallButtonWidth = 20f;

        const float AddCategoryButtonWidth = 100f;
        const float AddTypeButtonWidth = 70f;

		AttributeManager manager;

		SerializedProperty Attributes_Debug;

		SerializedProperty Attributes;
        SerializedProperty CurrentFilterMask;

        SerializedProperty EditorUIState;

        ReorderableList CategoryNames;


        int AttributeMarginX = 20;
        GUIStyle labelMargin, textMargin, buttonMargin, miniButton, boldFoldOut, boldMiniButton;
        Attribute removeAttribute = null;
        int moveFrom = 0, moveTo = 0;

        public void FiltersChanged(ReorderableList list)
        {
            CurrentFilterMask.intValue = -1;
        }

        public bool FilterInFilters(int filter, int currentFilters)
        {
            return ((currentFilters & (1 << filter)) > 0);
        }

		public void ListIterator(SerializedProperty listProperty)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Categories"))
            {
                EditorUIState.intValue = 0;
            }
            if (GUILayout.Button("Attributes"))
            {
                EditorUIState.intValue = 1;
            }
            EditorGUILayout.EndHorizontal();

            //Show Categories
            if (EditorUIState.intValue == 0)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("New Category", GUILayout.Width(AddCategoryButtonWidth)))
                {
                    Undo.RecordObject(manager, "Before New Category");
                    manager.AddCategory();
                    EditorUtility.SetDirty(manager);
                }
                EditorGUILayout.EndHorizontal();

                int catIndex = 0;
                moveFrom = -1;
                moveTo = -1;
                int deleteCat = -1;

                EditorGUI.indentLevel++;
                foreach (var attrCategory in manager.Categories)
                {
                    Rect rect = GUILayoutUtility.GetRect(18, SimpleEditor.EditorLineHeight);
                    Rect rect1 = rect;
                    rect1.width = CategoryFoldOutWidth;
                    Rect rect2 = rect1;
                    rect2.width = rect.width - CategoryFoldOutWidth - CategorySmallButtonCount * SmallButtonWidth - AddTypeButtonWidth;
                    rect2.x += CategoryFoldOutWidth;

                    Rect rect3 = rect2;
                    rect3.width = SmallButtonWidth;
                    rect3.x += rect2.width;
                    Rect rect4 = rect3;
                    rect4.x += SmallButtonWidth;
                    Rect rect5 = rect4;
                    rect5.x += SmallButtonWidth;
                    Rect rect6 = rect5;
                    rect6.width = AddTypeButtonWidth;
                    rect6.x += SmallButtonWidth;


                    attrCategory.foldOut = EditorGUI.Foldout(rect1, attrCategory.foldOut, new GUIContent(string.Format("Category_{0:00}:", catIndex)), boldFoldOut);

                    string tempName = EditorGUI.TextField(rect2, attrCategory.name);
                    if (tempName != attrCategory.name)
                    {
                        Undo.RecordObject(manager, "Before Category Name Changed");
                        manager.RenameCategory(catIndex, tempName);
                        EditorUtility.SetDirty(manager);
                    }

                    GUI.enabled = catIndex != 0;
                    if (GUI.Button(rect3, "⤴", miniButton))
                    {
                        moveFrom = catIndex;
                        moveTo = catIndex - 1;
                    }
                    GUI.enabled = catIndex + 1 < manager.Categories.Count;
                    if (GUI.Button(rect4, "⤵", miniButton))
                    {
                        moveFrom = catIndex;
                        moveTo = catIndex + 1;
                    }
                    GUI.enabled = true;
                    if (GUI.Button(rect5, "-", miniButton))
                        deleteCat = catIndex;

                    if (GUI.Button(rect6, "Add Type", miniButton))
                    {
                        Undo.RecordObject(manager, "Before New Type");
						manager.AddType(catIndex, "");
                        EditorUtility.SetDirty(manager);
                    }

                    if (attrCategory.foldOut)
                    {
                        int moveTypeFrom = -1;
                        int moveTypeTo = -1;
                        int typeDelete = -1;
                        int typeIndex = 0;
                        //Draw List

                        EditorGUI.indentLevel++;
                        foreach (var attrType in attrCategory.Types)
                        {
                            rect = GUILayoutUtility.GetRect(18, SimpleEditor.EditorLineHeight);
                                
                            rect1 = rect;
                            rect1.width = TypeFoldOutWidth;

                            rect2 = rect;
                            rect2.width -= TypeFoldOutWidth + TypeSmallButtonCount * SmallButtonWidth;
                            rect2.x += TypeFoldOutWidth;

                            rect3 = rect2;
                            rect3.width = SmallButtonWidth;
                            rect3.x += rect2.width;
                            rect4 = rect3;
                            rect4.x += SmallButtonWidth;
                            rect5 = rect4;
                            rect5.x += SmallButtonWidth;


                            EditorGUI.LabelField(rect1, new GUIContent(string.Format("Type_{0:00}:", typeIndex)));

                            tempName = EditorGUI.TextField(rect2, attrType);
                            if (tempName != attrType)
                            {
                                Undo.RecordObject(manager, "Before Rename Type");
								manager.RenameType(catIndex, typeIndex, tempName);
                                EditorUtility.SetDirty(manager);
                            }

                            GUI.enabled = typeIndex != 0;
                            if (GUI.Button(rect3, "⤴", miniButton))
                            {
                                moveTypeFrom = typeIndex;
                                moveTypeTo = typeIndex - 1;
                            }
                            GUI.enabled = typeIndex + 1 < attrCategory.Types.Count;
                            if (GUI.Button(rect4, "⤵", miniButton))
                            {
                                moveTypeFrom = typeIndex;
                                moveTypeTo = typeIndex + 1;
                            }
                            GUI.enabled = true;
                            if (GUI.Button(rect5, "-", miniButton))
                                typeDelete = typeIndex;

                            typeIndex++;
                        }
                        EditorGUI.indentLevel--;

                        if (typeDelete >= 0)
                        {
                            Undo.RecordObject(manager, "Before Remove Type");
							manager.RemoveType(catIndex, typeDelete);
                            EditorUtility.SetDirty(manager);
                        }
                        if (moveTypeFrom >= 0 && moveTypeTo >= 0)
                        {
                            Undo.RecordObject(manager, "Before Move Type");
							manager.MoveType(catIndex, moveTypeFrom, moveTypeTo);
                            EditorUtility.SetDirty(manager);
                        }
                    }

                    catIndex++;
                }

                EditorGUI.indentLevel--;

                if (deleteCat >= 0)
                {
                    Undo.RecordObject(manager, "Before Attribute AddPoint");
                    manager.RemoveCategory(deleteCat);
                    EditorUtility.SetDirty(manager);
                }
                if (moveFrom >= 0 && moveTo >= 0)
                {
                    Undo.RecordObject(manager, "Before Attribute AddPoint");
                    manager.MoveCategory(moveFrom, moveTo);
                    EditorUtility.SetDirty(manager);
                }

                
                moveFrom = -1;
                moveTo = -1;
            }
            else if (EditorUIState.intValue == 1)
            {
                CurrentFilterMask.intValue = EditorGUILayout.MaskField(new GUIContent("Show Categories:"), CurrentFilterMask.intValue, manager.CategoryNames.ToArray<string>());


                Attributes_Debug.boolValue = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Enables editing attribute points"), Attributes_Debug.boolValue);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("New Attribute", GUILayout.Width(AddAttributeButtonWidth)))
                {
                    Undo.RecordObject(manager, "Before Attribute Added");
                    manager.AddAttribute(new Attribute() { Name = string.Format("Attribute{0}", manager.AttributeCount) });
                }
                EditorGUILayout.EndHorizontal();

                //EditorGUI.indentLevel++;
                for (int i = 0; i < listProperty.arraySize; i++)
                {
                    SerializedProperty attribute = listProperty.GetArrayElementAtIndex(i);
                    DrawAttribute(attribute, i);
                }
                //EditorGUI.indentLevel--;
                

                if (removeAttribute != null)
                {
                    Undo.RecordObject(manager, "Before Attribute Removed");
                    manager.RemoveAttribute(removeAttribute);
                    removeAttribute = null;
                }
                if (moveFrom >= 0 && moveTo >= 0 && moveFrom != moveTo)
                {
                    Undo.RecordObject(manager, "Before Attribute Move");
                    manager.MoveAttribute(moveFrom, moveTo);
                }
                moveFrom = -1;
                moveTo = -1;
            }
		}

		public void DrawAttribute(SerializedProperty attribute, int index)
		{
			Attribute currentAttribute = manager.GetAttributeByIndex(index);
            if (!FilterInFilters(currentAttribute.AttrInfo.category, CurrentFilterMask.intValue))
                return;
			
			bool enabledBefore = GUI.enabled;

			SerializedProperty foldOut = attribute.FindPropertyRelative("FoldOut");
			SerializedProperty point_FoldOut = attribute.FindPropertyRelative("Point_FoldOut");
			SerializedProperty value_FoldOut = attribute.FindPropertyRelative("Value_FoldOut");

			//AttributeSettings
			SerializedProperty name = attribute.FindPropertyRelative("Name");
			SerializedProperty enabled = attribute.FindPropertyRelative("Enabled");
			SerializedProperty locked = attribute.FindPropertyRelative("Locked");

			//PointSettings
			SerializedProperty points = attribute.FindPropertyRelative("Points");
			SerializedProperty startPoints = attribute.FindPropertyRelative("StartPoints");
			SerializedProperty minPoints = attribute.FindPropertyRelative("MinPoints");
			SerializedProperty maxPoints = attribute.FindPropertyRelative("MaxPoints");

			//ValueSettings
			SerializedProperty valueInfo = attribute.FindPropertyRelative("ValueInfo");

			SerializedProperty AttrInfo = attribute.FindPropertyRelative("AttrInfo");
            SerializedProperty category = AttrInfo.FindPropertyRelative("category");
            SerializedProperty type = AttrInfo.FindPropertyRelative("type");

			SerializedProperty value = valueInfo.FindPropertyRelative("Value");
			SerializedProperty startValue = valueInfo.FindPropertyRelative("StartValue");
			SerializedProperty valuePerPoint = valueInfo.FindPropertyRelative("ValuePerPoint");
			SerializedProperty valuePerPointMultipliedByCurrentPoints = valueInfo.FindPropertyRelative("ValuePerPointMultipliedByCurrentPoints");


			float progress = (points.intValue - minPoints.intValue) / (float)(maxPoints.intValue - minPoints.intValue);
			if(maxPoints.intValue == 0)
				progress = 1f;

            float foldOutWidth = 95f;
            float buttonWidth = 20f;
            int buttonCount = 3;

            Rect rect = GUILayoutUtility.GetRect(18, SimpleEditor.EditorLineHeight);
            Rect rect1 = rect;
            rect1.width = foldOutWidth;
            Rect rect2 = rect1;
            rect2.width = rect.width - foldOutWidth - buttonCount * buttonWidth;
            rect2.x += foldOutWidth;

            Rect rect3 = rect2;
            rect3.width = buttonWidth;
            rect3.x += rect2.width;
            Rect rect4 = rect3;
            rect4.x += buttonWidth;
            Rect rect5 = rect4;
            rect5.x += buttonWidth;
            Rect rect6 = rect5;
            rect6.x += buttonWidth;

			foldOut.boolValue = EditorGUI.Foldout(rect1, foldOut.boolValue, new GUIContent(string.Format("Attribute_{0:00}:", index)), boldFoldOut);

            string tempName = EditorGUI.TextField(rect2, name.stringValue);
            if (tempName != name.stringValue)
            {
                name.stringValue = tempName;
            }

			GUI.enabled = index != 0;
            if (GUI.Button(rect3, "⤴", miniButton))
			{
				moveFrom = index;
				moveTo = index - 1;
			}
			GUI.enabled = index + 1 < Attributes.arraySize;
            if (GUI.Button(rect4, "⤵", miniButton))
			{
				moveFrom = index;
				moveTo = index + 1;
			}
			GUI.enabled = true;
            if (GUI.Button(rect5, "-", miniButton))
				removeAttribute = currentAttribute;

			EditorGUILayout.BeginHorizontal();

			GUI.enabled = enabled.boolValue;
			ProgressBar(enabled.boolValue ? progress : 0f, string.Format("Progress: ({0}/{1}) = {2:0.00}", points.intValue, maxPoints.intValue, value.floatValue), 20, 0, Attributes_Debug.boolValue ? 18 : 5);

			if (Attributes_Debug.boolValue)
			{
				if (GUI.enabled)
					GUI.enabled = !locked.boolValue;
				if (maxPoints.intValue == 0)
					GUI.enabled = false;
				if (!enabled.boolValue)
				{
                    if (GUILayout.Button("Disabled", miniButton, GUILayout.Width(107)))
					{
					}
				}
				else if (locked.boolValue)
				{
                    if (GUILayout.Button("Locked", miniButton, GUILayout.Width(107)))
					{
					}
				}
				else
				{
                    if (GUILayout.Button("-", miniButton, GUILayout.Width(20)))
					{
						Undo.RecordObject(manager, "Before Attribute RemovePoint");
						currentAttribute.RemovePoint();
					}
                    if (GUILayout.Button("+", miniButton, GUILayout.Width(20)))
					{
						Undo.RecordObject(manager, "Before Attribute AddPoint");
						currentAttribute.AddPoint();
					}
                    if (GUILayout.Button("reset", miniButton, GUILayout.Width(35)))
					{
						Undo.RecordObject(manager, "Before Attribute ResetPoint");
						currentAttribute.ResetPoints();
					}
                    if (GUILayout.Button("max", miniButton, GUILayout.Width(35)))
					{
						Undo.RecordObject(manager, "Before Attribute MaximizePoints");
						currentAttribute.MaximizePoints();
					}
				}
			}
			GUI.enabled = true;


			EditorGUILayout.EndHorizontal();

			if (foldOut.boolValue)
			{
                EditorGUI.indentLevel++;
                if (AttributeManager.Instance.Categories.Count > 0)
                {
                    category.intValue = EditorGUILayout.Popup("Category", category.intValue, AttributeManager.Instance.CategoryNames.ToArray<string>());
                    if (category.intValue < 0 || category.intValue >= AttributeManager.Instance.Categories.Count)
                        category.intValue = 0;
                    type.intValue = EditorGUILayout.Popup("Type", type.intValue, AttributeManager.Instance.Categories[category.intValue].Types.ToArray<string>());
                }
                else
                {
                    EditorGUILayout.LabelField("First: Create some Categories and Types");
                }
				enabled.boolValue = EditorGUILayout.Toggle(new GUIContent("Enabled", "Enables or Disables this Attribute in Game"), enabled.boolValue);
				locked.boolValue = EditorGUILayout.Toggle(new GUIContent("Locked", "A locked Attribute can't be changed in Editor or Game"), locked.boolValue);

				point_FoldOut.boolValue = EditorGUILayout.Foldout(point_FoldOut.boolValue, new GUIContent(string.Format("Points(<color=#2277ff>{0}</color>):", points.intValue)), boldFoldOut);

				if (point_FoldOut.boolValue)
				{
					//AttributePoints
					ProgressBar(progress, string.Format("Current Points: {0}/{1}", points.intValue, maxPoints.intValue, value.floatValue), 20, 0, 18);
					
					minPoints.intValue = EditorGUILayout.IntField(new GUIContent("Min Points:", ""), minPoints.intValue);
					maxPoints.intValue = EditorGUILayout.IntField(new GUIContent("Max Points:", ""), maxPoints.intValue);

					int newStartPoints = EditorGUILayout.IntSlider(new GUIContent("Start Points:", ""), startPoints.intValue, minPoints.intValue, maxPoints.intValue);

					if (points.intValue == startPoints.intValue)
						points.intValue = newStartPoints;
					startPoints.intValue = newStartPoints;
				}

				if (minPoints.intValue < 0)
					minPoints.intValue = 0;

				if (maxPoints.intValue < minPoints.intValue)
					maxPoints.intValue = minPoints.intValue;


				value_FoldOut.boolValue = EditorGUILayout.Foldout(value_FoldOut.boolValue, new GUIContent(string.Format("Value(<color=#aa5522>{0:0.00}</color>):", value.floatValue)), boldFoldOut);

				if (value_FoldOut.boolValue)
				{
					EditorGUILayout.LabelField(new GUIContent(string.Format("Calculation: {0} + {1}x{2} + {3}x({4} -> {5}) = {6}", startValue.floatValue, points.intValue, valuePerPoint.floatValue, valuePerPointMultipliedByCurrentPoints.floatValue, 1, points.intValue, value.floatValue)));

					startValue.floatValue = EditorGUILayout.FloatField(new GUIContent("Start Value:", ""), startValue.floatValue);
					valuePerPoint.floatValue = EditorGUILayout.FloatField(new GUIContent("Value per Point:", ""), valuePerPoint.floatValue);
					valuePerPointMultipliedByCurrentPoints.floatValue = EditorGUILayout.FloatField(new GUIContent("Value per Point (multiplied by current Points):", ""), valuePerPointMultipliedByCurrentPoints.floatValue);
				}
                EditorGUI.indentLevel--;
			}

			currentAttribute.RecalculateValue();
			GUI.enabled = enabledBefore;
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

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			manager = (AttributeManager)target;
			AttributeManager.Instance = manager;

			EditorUIState = serializedObject.FindProperty("EditorUIState");

			Attributes_Debug = serializedObject.FindProperty("Attributes_Debug");

			Attributes = serializedObject.FindProperty("Attributes");
			CurrentFilterMask = serializedObject.FindProperty("CurrentFilterMask");



			CategoryNames = new ReorderableList(serializedObject, serializedObject.FindProperty("CategoryNames"));

			CategoryNames.drawElementCallback =
				(Rect rect, int index, bool isActive, bool isFocused) =>
				{
					var element = CategoryNames.serializedProperty.GetArrayElementAtIndex(index);
					rect.y += 1f;
					rect.height -= 2f;
					element.stringValue = EditorGUI.TextField(rect, element.stringValue);
				};
			CategoryNames.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "Filters");
			};
			CategoryNames.onChangedCallback = FiltersChanged;

			labelMargin = new GUIStyle(EditorStyles.label);
			textMargin = new GUIStyle(EditorStyles.textField);
			buttonMargin = new GUIStyle(GUI.skin.button);
			miniButton = new GUIStyle(EditorStyles.miniButton);
			boldFoldOut = new GUIStyle(EditorStyles.foldout);
			boldMiniButton = new GUIStyle(EditorStyles.miniButton);

			labelMargin.margin = new RectOffset(AttributeMarginX, 0, 0, 0);
			textMargin.margin = new RectOffset(AttributeMarginX, 0, 0, 0);
			buttonMargin.margin = new RectOffset(AttributeMarginX, 0, 0, 0);

			boldFoldOut.fontStyle = FontStyle.Bold;
			boldFoldOut.richText = true;

			boldMiniButton.alignment = TextAnchor.MiddleCenter;
			boldMiniButton.padding = new RectOffset(0, 2, 0, 2);

			miniButton.alignment = TextAnchor.MiddleCenter;
			miniButton.padding = new RectOffset(0, 2, 0, 2);

			ListIterator(Attributes);

			serializedObject.ApplyModifiedProperties();

			if (GUI.changed)
			{
				EditorUtility.SetDirty(manager);
			}
		}
	}
}
#endif