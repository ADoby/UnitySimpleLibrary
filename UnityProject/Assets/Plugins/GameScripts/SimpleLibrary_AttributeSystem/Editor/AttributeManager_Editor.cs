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

		AttributeManager manager;

		SerializedProperty Attributes_FoldOut;
		SerializedProperty NameList_FoldOut;
		SerializedProperty Attributes_Debug;
		SerializedProperty SelectedFilterMode;
		SerializedProperty Filter;
		SerializedProperty Attributes;
        SerializedProperty Filters_FoldOut;
        SerializedProperty CurrentFilterMask;

        ReorderableList Names;
        ReorderableList Filters;

        int AttributeMarginX = 20;
        GUIStyle labelMargin, textMargin, buttonMargin, miniButton, boldLabel, boldFoldOut, boldMiniButton;
		bool initiated = false;
		char FilterSplitCharacter = ',';
        Attribute removeAttribute = null;
        int moveFrom = 0, moveTo = 0;

		void OnEnable()
		{
			initiated = false;

			Attributes = serializedObject.FindProperty("Attributes");
			Attributes_FoldOut = serializedObject.FindProperty("Attributes_FoldOut");
			NameList_FoldOut = serializedObject.FindProperty("NameList_FoldOut");
			Attributes_Debug = serializedObject.FindProperty("Attributes_Debug");
			SelectedFilterMode = serializedObject.FindProperty("SelectedFilterMode");
            Filters_FoldOut = serializedObject.FindProperty("Filters_FoldOut");
			Filter = serializedObject.FindProperty("Filter");
            CurrentFilterMask = serializedObject.FindProperty("CurrentFilterMask");

            Names = new ReorderableList(serializedObject, serializedObject.FindProperty("AttributeNames"), false, false, false, false);
            Names.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = Names.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 1f;
                    rect.height -= 2f;
                    EditorGUI.LabelField(rect, element.stringValue);
                };


            Filters = new ReorderableList(serializedObject, serializedObject.FindProperty("Filters"));

            Filters.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = Filters.serializedProperty.GetArrayElementAtIndex(index);
                    rect.y += 1f;
                    rect.height -= 2f;
                    element.stringValue = EditorGUI.TextField(rect, element.stringValue);
                };
            Filters.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Filters");
            };
            Filters.onChangedCallback = FiltersChanged;
		}

        public void FiltersChanged(ReorderableList list)
        {
            CurrentFilterMask.intValue = -1;
        }

        public bool FilterInFilters(int filter, int currentFilters)
        {
            return ((currentFilters & (1 << filter)) > 0);
        }

		public void ListIterator(SerializedProperty listProperty, ref SerializedProperty visible)
        {
            Filters_FoldOut.boolValue = EditorGUILayout.Foldout(Filters_FoldOut.boolValue, "Filters:", boldFoldOut);

            if (Filters_FoldOut.boolValue)
            {
                Filters.DoLayoutList();
            }
            CurrentFilterMask.intValue = EditorGUILayout.MaskField(new GUIContent("Filter:"), CurrentFilterMask.intValue, manager.Filters.ToArray());


			Attributes_Debug.boolValue = EditorGUILayout.Toggle(new GUIContent("Debug Mode", "Enables editing attribute points"), Attributes_Debug.boolValue);

			EditorGUILayout.BeginHorizontal();
			visible.boolValue = EditorGUILayout.Foldout(visible.boolValue, listProperty.name, boldFoldOut);
            if (GUILayout.Button("+", miniButton, GUILayout.Width(20)))
			{
				Undo.RecordObject(manager, "Before Attribute Added");
                manager.AddAttribute(new Attribute() { Name = string.Format("Attribute{0}", manager.AttributeCount) });
			}
			EditorGUILayout.EndHorizontal();

			if (visible.boolValue)
			{
				EditorGUI.indentLevel++;
				for (int i = 0; i < listProperty.arraySize; i++)
				{
					SerializedProperty attribute = listProperty.GetArrayElementAtIndex(i);
					DrawAttribute(attribute, i);
				}
				EditorGUI.indentLevel--;
			}

			if (removeAttribute != null)
			{
				Undo.RecordObject(manager, "Before Attribute Removed");
				manager.RemoveAttribute(removeAttribute);
				removeAttribute = null;
			}
			if(moveFrom >= 0 && moveTo >= 0 && moveFrom != moveTo)
			{
				Undo.RecordObject(manager, "Before Attribute Move");
				manager.Switch(moveFrom, moveTo);
			}
			moveFrom = -1;
			moveTo = -1;
		}

		public void DrawAttribute(SerializedProperty attribute, int index)
		{
			Attribute currentAttribute = manager.GetAttribute(index);
            if (!FilterInFilters(currentAttribute.SelectedFilter, CurrentFilterMask.intValue))
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
			SerializedProperty maxPoints = attribute.FindPropertyRelative("MaxPoints");

			//ValueSettings
			SerializedProperty valueInfo = attribute.FindPropertyRelative("ValueInfo");
			SerializedProperty FilterAttribute = attribute.FindPropertyRelative("FilterAttribute");


            SerializedProperty SelectedFilter = attribute.FindPropertyRelative("SelectedFilter");

			SerializedProperty value = valueInfo.FindPropertyRelative("Value");
			SerializedProperty startValue = valueInfo.FindPropertyRelative("StartValue");
			SerializedProperty valuePerPoint = valueInfo.FindPropertyRelative("ValuePerPoint");
			SerializedProperty valuePerPointMultipliedByCurrentPoints = valueInfo.FindPropertyRelative("ValuePerPointMultipliedByCurrentPoints");


			float progress = (points.intValue - startPoints.intValue) / (float)maxPoints.intValue;
			if(maxPoints.intValue == 0)
				progress = 1f;

			EditorGUILayout.BeginHorizontal();
            //<color=#008800>{1}</color>
			foldOut.boolValue = EditorGUILayout.Foldout(foldOut.boolValue, new GUIContent(string.Format("Attribute_<color=#5522aa>{0:00}</color>:", index)), boldFoldOut);

            string tempName = EditorGUILayout.TextField(name.stringValue);
            if (tempName != name.stringValue)
            {
                name.stringValue = tempName;
            }

            //name.stringValue = EditorGUILayout.TextField(name.stringValue);

			GUI.enabled = index != 0;
            if (GUILayout.Button("⤴", miniButton, GUILayout.Width(20)))
			{
				moveFrom = index;
				moveTo = index - 1;
			}
			GUI.enabled = index + 1 < Attributes.arraySize;
            if (GUILayout.Button("⤵", miniButton, GUILayout.Width(20)))
			{
				moveFrom = index;
				moveTo = index + 1;
			}
			GUI.enabled = true;
            if (GUILayout.Button("-", miniButton, GUILayout.Width(20)))
				removeAttribute = currentAttribute;
			EditorGUILayout.EndHorizontal();

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
                    if (GUILayout.Button("0", miniButton, GUILayout.Width(20)))
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
                /*
				string tempName = EditorGUILayout.TextField(new GUIContent("Name", "The Name of this Attribute"), name.stringValue);
				if (tempName != name.stringValue)
				{
					name.stringValue = tempName;
				}
                */

                SelectedFilter.intValue = EditorGUILayout.Popup("Filter:", SelectedFilter.intValue, manager.Filters.ToArray());

				enabled.boolValue = EditorGUILayout.Toggle(new GUIContent("Enabled", "Enables or Disables this Attribute in Game"), enabled.boolValue);
				locked.boolValue = EditorGUILayout.Toggle(new GUIContent("Locked", "A locked Attribute can't be changed in Editor or Game"), locked.boolValue);

				point_FoldOut.boolValue = EditorGUILayout.Foldout(point_FoldOut.boolValue, new GUIContent(string.Format("Points(<color=#2277ff>{0}</color>):", points.intValue)), boldFoldOut);

				if (point_FoldOut.boolValue)
				{
					//AttributePoints
					ProgressBar(progress, string.Format("Current Points: {0}/{1}", points.intValue, maxPoints.intValue, value.floatValue), 20, 0, 18);

					int newStartPoints = EditorGUILayout.IntField(new GUIContent("Start Points:", ""), startPoints.intValue);
					if (points.intValue == startPoints.intValue)
						points.intValue = newStartPoints;
					startPoints.intValue = newStartPoints;

					maxPoints.intValue = EditorGUILayout.IntField(new GUIContent("Max Points:", ""), maxPoints.intValue);
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
				value_FoldOut.boolValue = EditorGUILayout.Foldout(value_FoldOut.boolValue, new GUIContent(string.Format("Value(<color=#aa5522>{0:0.00}</color>):", value.floatValue)), boldFoldOut);

				if (value_FoldOut.boolValue)
				{
					EditorGUILayout.LabelField(new GUIContent(string.Format("Calculation: {0} + {1}x{2} + {3}x({4} -> {5}) = {6}", startValue.floatValue, points.intValue, valuePerPoint.floatValue, valuePerPointMultipliedByCurrentPoints.floatValue, 1, points.intValue, value.floatValue)));

					startValue.floatValue = EditorGUILayout.FloatField(new GUIContent("Start Value:", ""), startValue.floatValue);
					valuePerPoint.floatValue = EditorGUILayout.FloatField(new GUIContent("Value per Point:", ""), valuePerPoint.floatValue);
					valuePerPointMultipliedByCurrentPoints.floatValue = EditorGUILayout.FloatField(new GUIContent("Value per Point (multiplied by current Points):", ""), valuePerPointMultipliedByCurrentPoints.floatValue);
				}
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

			EditorGUI.ProgressBar (rect, value, label);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			manager = (AttributeManager)target;
            AttributeManager.Instance = manager;

			if (!initiated)
			{
				initiated = true;
				labelMargin = new GUIStyle(EditorStyles.label);
				textMargin = new GUIStyle(EditorStyles.textField);
				buttonMargin = new GUIStyle(GUI.skin.button);
				miniButton = new GUIStyle(EditorStyles.miniButton);
				boldLabel = new GUIStyle(EditorStyles.boldLabel);
				boldFoldOut = new GUIStyle(EditorStyles.foldout);
				boldMiniButton = new GUIStyle(EditorStyles.miniButton);

				labelMargin.margin = new RectOffset(AttributeMarginX, 0, 0, 0);
				textMargin.margin = new RectOffset(AttributeMarginX, 0, 0, 0);
				buttonMargin.margin = new RectOffset(AttributeMarginX, 0, 0, 0);

				boldFoldOut.fontStyle = FontStyle.Bold;
				boldFoldOut.richText = true;

				boldMiniButton.fontStyle = FontStyle.Bold;
				boldMiniButton.alignment = TextAnchor.MiddleCenter;
				boldMiniButton.padding = new RectOffset(0, 2, 0, 2);
				boldMiniButton.fontSize = 12;

				miniButton.alignment = TextAnchor.MiddleCenter;
				miniButton.padding = new RectOffset(0, 2, 0, 2);
				miniButton.fontSize = 12;
			}

			ListIterator(Attributes, ref Attributes_FoldOut);

            /*
			NameList_FoldOut.boolValue = EditorGUILayout.Foldout(NameList_FoldOut.boolValue, "NameList:", boldFoldOut);
            if (NameList_FoldOut.boolValue)
                Names.DoLayoutList();
            */

			serializedObject.ApplyModifiedProperties();

			if (GUI.changed)
			{
				manager.UpdateAttributeNamesList();
				EditorUtility.SetDirty(manager);
			}
		}
	}
}
