using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SimpleCamera))]
public class SimpleCamera_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.Space();
		SerializedProperty CurrentCameraMode = serializedObject.FindProperty("CurrentCameraMode");
		SerializedProperty ShowAllSettings = serializedObject.FindProperty("ShowAllSettings");

		EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(CurrentCameraMode);
		EditorGUILayout.PropertyField(ShowAllSettings);

		if (ShowAllSettings.boolValue || (SimpleCamera.CameraMode)CurrentCameraMode.intValue == SimpleCamera.CameraMode.FreeCam)
		{
			EditorGUILayout.Space();
			#region FREE
			SerializedProperty FreeHoldMouseButtonForRotation = serializedObject.FindProperty("FreeHoldMouseButtonForRotation");
			SerializedProperty FreeMouseButtonForRotation = serializedObject.FindProperty("FreeMouseButtonForRotation");
			SerializedProperty Free_HorizontalAxis = serializedObject.FindProperty("Free_HorizontalAxis");
			SerializedProperty FreeMovementXSpeed = serializedObject.FindProperty("FreeMovementXSpeed");
			SerializedProperty Free_UpDownAxis = serializedObject.FindProperty("Free_UpDownAxis");
			SerializedProperty FreeMovementYSpeed = serializedObject.FindProperty("FreeMovementYSpeed");
			SerializedProperty Free_VerticalAxis = serializedObject.FindProperty("Free_VerticalAxis");
			SerializedProperty FreeMovementZSpeed = serializedObject.FindProperty("FreeMovementZSpeed");
			SerializedProperty Free_RotateXAxis = serializedObject.FindProperty("Free_RotateXAxis");
			SerializedProperty FreeMouseXSpeed = serializedObject.FindProperty("FreeMouseXSpeed");
			SerializedProperty Free_RotateYAxis = serializedObject.FindProperty("Free_RotateYAxis");
			SerializedProperty FreeMouseYSpeed = serializedObject.FindProperty("FreeMouseYSpeed");

			EditorGUILayout.LabelField("Free Settings", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(Free_HorizontalAxis);
			if (!string.IsNullOrEmpty(Free_HorizontalAxis.stringValue))
				EditorGUILayout.PropertyField(FreeMovementXSpeed);
			EditorGUILayout.PropertyField(Free_UpDownAxis);
			if (!string.IsNullOrEmpty(Free_UpDownAxis.stringValue))
				EditorGUILayout.PropertyField(FreeMovementYSpeed);
			EditorGUILayout.PropertyField(Free_VerticalAxis);
			if (!string.IsNullOrEmpty(Free_VerticalAxis.stringValue))
				EditorGUILayout.PropertyField(FreeMovementZSpeed);

			EditorGUILayout.LabelField("Rotation Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(FreeHoldMouseButtonForRotation);
			if (FreeHoldMouseButtonForRotation.boolValue)
				EditorGUILayout.PropertyField(FreeMouseButtonForRotation);
			EditorGUILayout.PropertyField(Free_RotateXAxis);
			if (!string.IsNullOrEmpty(Free_RotateXAxis.stringValue))
				EditorGUILayout.PropertyField(FreeMouseXSpeed);
			EditorGUILayout.PropertyField(Free_RotateYAxis);
			if (!string.IsNullOrEmpty(Free_RotateYAxis.stringValue))
				EditorGUILayout.PropertyField(FreeMouseYSpeed);
			#endregion
		}
		if (ShowAllSettings.boolValue || (SimpleCamera.CameraMode)CurrentCameraMode.intValue == SimpleCamera.CameraMode.FixedRotation)
		{
			EditorGUILayout.Space();
			#region FIXED
			SerializedProperty Fixed_FreezeRotation = serializedObject.FindProperty("Fixed_FreezeRotation");
			SerializedProperty Fixed_SmoothToRotation = serializedObject.FindProperty("Fixed_SmoothToRotation");
			SerializedProperty Fixed_WantedRotation = serializedObject.FindProperty("Fixed_WantedRotation");
			SerializedProperty Fixed_SmoothToRotationSpeed = serializedObject.FindProperty("Fixed_SmoothToRotationSpeed");
			SerializedProperty Fixed_MaxSmoothRotationSpeed = serializedObject.FindProperty("Fixed_MaxSmoothRotationSpeed");
			SerializedProperty Fixed_MinAngleToFinishSmoothLookAt = serializedObject.FindProperty("Fixed_MinAngleToFinishSmoothLookAt");
			SerializedProperty Fixed_HeightAxisInput = serializedObject.FindProperty("Fixed_HeightAxisInput");
			SerializedProperty Fixed_HeightWorldSpace = serializedObject.FindProperty("Fixed_HeightWorldSpace");
			SerializedProperty Fixed_HeightAxis = serializedObject.FindProperty("Fixed_HeightAxis");
			SerializedProperty Fixed_HeightSpeed = serializedObject.FindProperty("Fixed_HeightSpeed");
			SerializedProperty Fixed_HeightAxisToMouse = serializedObject.FindProperty("Fixed_HeightAxisToMouse");
			SerializedProperty Fixed_PinchZoom = serializedObject.FindProperty("Fixed_PinchZoom");
			SerializedProperty Fixed_PinchZoomSpeed = serializedObject.FindProperty("Fixed_PinchZoomSpeed");
			SerializedProperty Fixed_TouchInput = serializedObject.FindProperty("Fixed_TouchInput");

			SerializedProperty Fixed_FloorPosition = serializedObject.FindProperty("Fixed_FloorPosition");
			SerializedProperty Fixed_FloorNormal = serializedObject.FindProperty("Fixed_FloorNormal");
			SerializedProperty FixedCheckSightDown = serializedObject.FindProperty("FixedCheckSightDown");
			SerializedProperty FixedCheckSightDownMask = serializedObject.FindProperty("FixedCheckSightDownMask");
			SerializedProperty FixedOversightDown = serializedObject.FindProperty("FixedOversightDown");
			SerializedProperty FixedOversightDownDistance = serializedObject.FindProperty("FixedOversightDownDistance");
			SerializedProperty FixedMinPosition = serializedObject.FindProperty("FixedMinPosition");
			SerializedProperty FixedMaxPosition = serializedObject.FindProperty("FixedMaxPosition");
			SerializedProperty Fixed_KeyboardInput = serializedObject.FindProperty("Fixed_KeyboardInput");
			SerializedProperty Fixed_XAxisInput = serializedObject.FindProperty("Fixed_XAxisInput");
			SerializedProperty Fixed_XWorldSpace = serializedObject.FindProperty("Fixed_XWorldSpace");
			SerializedProperty Fixed_XAxis = serializedObject.FindProperty("Fixed_XAxis");
			SerializedProperty Fixed_XSpeed = serializedObject.FindProperty("Fixed_XSpeed");
			SerializedProperty Fixed_ZAxisInput = serializedObject.FindProperty("Fixed_ZAxisInput");
			SerializedProperty Fixed_ZWorldSpace = serializedObject.FindProperty("Fixed_ZWorldSpace");
			SerializedProperty Fixed_ZAxis = serializedObject.FindProperty("Fixed_ZAxis");
			SerializedProperty Fixed_ZSpeed = serializedObject.FindProperty("Fixed_ZSpeed");
			SerializedProperty Fixed_Drag = serializedObject.FindProperty("Fixed_Drag");
			SerializedProperty Fixed_HoldMouseForDrag = serializedObject.FindProperty("Fixed_HoldMouseForDrag");
			SerializedProperty Fixed_HoldMouseButton = serializedObject.FindProperty("Fixed_HoldMouseButton");
			SerializedProperty FixedDragIn3D = serializedObject.FindProperty("FixedDragIn3D");
			SerializedProperty FixedDragOnRayCastLayer = serializedObject.FindProperty("FixedDragOnRayCastLayer");
			SerializedProperty Fixed_AbsoluteDrag = serializedObject.FindProperty("Fixed_AbsoluteDrag");

			EditorGUILayout.LabelField("Fixed Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(Fixed_TouchInput);

			EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(Fixed_KeyboardInput);
			if (Fixed_KeyboardInput.boolValue)
			{
				EditorGUILayout.PropertyField(Fixed_XAxisInput);
				if (!string.IsNullOrEmpty(Fixed_XAxisInput.stringValue))
				{
					EditorGUILayout.PropertyField(Fixed_XWorldSpace);
					EditorGUILayout.PropertyField(Fixed_XAxis);
					EditorGUILayout.PropertyField(Fixed_XSpeed);
				}
				EditorGUILayout.PropertyField(Fixed_ZAxisInput);
				if (!string.IsNullOrEmpty(Fixed_ZAxisInput.stringValue))
				{
					EditorGUILayout.PropertyField(Fixed_ZWorldSpace);
					EditorGUILayout.PropertyField(Fixed_ZAxis);
					EditorGUILayout.PropertyField(Fixed_ZSpeed);
				}
			}

			EditorGUILayout.PropertyField(Fixed_Drag);
			if (Fixed_Drag.boolValue)
			{
				EditorGUILayout.PropertyField(Fixed_HoldMouseForDrag);
				if (Fixed_HoldMouseForDrag.boolValue)
					EditorGUILayout.PropertyField(Fixed_HoldMouseButton);
				EditorGUILayout.PropertyField(FixedDragIn3D);
				if (FixedDragIn3D.boolValue)
					EditorGUILayout.PropertyField(FixedDragOnRayCastLayer);
				else
				{
					EditorGUILayout.PropertyField(Fixed_FloorPosition);
					EditorGUILayout.PropertyField(Fixed_FloorNormal);
				}
			}

			EditorGUILayout.LabelField("Zoom Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(Fixed_HeightAxisInput);
			if (!string.IsNullOrEmpty(Fixed_HeightAxisInput.stringValue))
			{
				EditorGUILayout.PropertyField(Fixed_HeightWorldSpace);
				EditorGUILayout.PropertyField(Fixed_HeightAxis);
				EditorGUILayout.PropertyField(Fixed_HeightSpeed);
			}
			EditorGUILayout.PropertyField(Fixed_HeightAxisToMouse);
			EditorGUILayout.PropertyField(Fixed_PinchZoom);
			if (Fixed_PinchZoom.boolValue)
			{
				EditorGUILayout.PropertyField(Fixed_PinchZoomSpeed);
			}

			EditorGUILayout.LabelField("Rotation Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(Fixed_SmoothToRotation);
			EditorGUILayout.PropertyField(Fixed_FreezeRotation);
			if (Fixed_SmoothToRotation.boolValue || Fixed_FreezeRotation.boolValue)
				EditorGUILayout.PropertyField(Fixed_WantedRotation);
			if (Fixed_SmoothToRotation.boolValue)
			{
				EditorGUILayout.PropertyField(Fixed_SmoothToRotationSpeed);
				EditorGUILayout.PropertyField(Fixed_MaxSmoothRotationSpeed);
				EditorGUILayout.PropertyField(Fixed_MinAngleToFinishSmoothLookAt);
			}

			EditorGUILayout.LabelField("Constraint Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(FixedMinPosition);
			EditorGUILayout.PropertyField(FixedMaxPosition);
			EditorGUILayout.PropertyField(FixedCheckSightDown);
			if (FixedCheckSightDown.boolValue)
			{
				EditorGUILayout.PropertyField(FixedCheckSightDownMask);
				EditorGUILayout.PropertyField(FixedOversightDown);
				EditorGUILayout.PropertyField(FixedOversightDownDistance);
			}

			#endregion
		}
		if (ShowAllSettings.boolValue || (SimpleCamera.CameraMode)CurrentCameraMode.intValue == SimpleCamera.CameraMode.Orbit)
		{

		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif
