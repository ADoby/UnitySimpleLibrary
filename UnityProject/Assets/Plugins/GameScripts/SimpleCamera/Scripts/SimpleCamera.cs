using UnityEngine;
using System.Collections;

public class SimpleCamera : MonoBehaviour
{
	Camera cam;
	Transform trans;

	public enum CameraMode
	{
		FreeCam,
		FixedRotation,
		Orbit
	}

	public bool ShowAllSettings = true;

	//[Header("General Settings")]
	public CameraMode CurrentCameraMode;
	CameraMode LastCameraMode;

	//[Header("Free Cam Settings")]
	public bool FreeHoldMouseButtonForRotation = true;
	public int FreeMouseButtonForRotation = 1;
	public string Free_HorizontalAxis = "Horizontal";
	public float FreeMovementXSpeed = 1f;
	public string Free_UpDownAxis = "Mouse ScrollWheel";
	public float FreeMovementYSpeed = 20f;
	public string Free_VerticalAxis = "Vertical";
	public float FreeMovementZSpeed = 1f;
	public string Free_RotateXAxis = "Mouse X";
	public float FreeMouseXSpeed = 10f;
	public string Free_RotateYAxis = "Mouse Y";
	public float FreeMouseYSpeed = 10f;

	//[Header("Fixed Rotation Settings")]
	public bool Fixed_FreezeRotation = true;
	public bool Fixed_SmoothToRotation = true;
	public Vector3 Fixed_WantedRotation;
	public float Fixed_SmoothToRotationSpeed = 0.1f;
	public float Fixed_MaxSmoothRotationSpeed = 0.5f;
	public float Fixed_MinAngleToFinishSmoothLookAt = 0.1f;

	//[Header("Fixed Height Settings")]
	public string Fixed_HeightAxisInput = "Mouse ScrollWheel";
	public bool Fixed_HeightWorldSpace = true;
	public Vector3 Fixed_HeightAxis = Vector3.up;
	public float Fixed_HeightSpeed = -50f;

	public bool Fixed_HeightAxisToMouse = true;
	public bool Fixed_PinchZoom = true;
	public float Fixed_PinchZoomSpeed = 1f;

	public bool Fixed_TouchInput = false;
	public Vector3 Fixed_FloorPosition;
	public Vector3 Fixed_FloorNormal = Vector3.up;
	Plane FloorPlane;
	int LastTouchCount = 0;
	bool TouchHad = false;
	Vector2 LastMousePosition;
	Vector3 Last3DPosition;

	Vector3 procentValues;
	Vector3 savedMovement;

	//[Header("Fixed Constraint Settings")]
	public bool FixedCheckSightDown = true;
	public LayerMask FixedCheckSightDownMask;
	public float FixedOversightDown = 10f;
	public float FixedOversightDownDistance = 1f;

	public Vector3 FixedMinPosition;
	public Vector3 FixedMaxPosition = Vector3.one * 100f;

	//[Header("Fixed Movement Settings")]
	public bool Fixed_KeyboardInput = true;
	public string Fixed_XAxisInput = "Horizontal";
	public bool Fixed_XWorldSpace = false;
	public Vector3 Fixed_XAxis = Vector3.right;
	public float Fixed_XSpeed = 1f;
	public string Fixed_ZAxisInput = "Vertical";
	public bool Fixed_ZWorldSpace = false;
	public Vector3 Fixed_ZAxis = Vector3.forward;
	public float Fixed_ZSpeed = 1f;

	//[Header("Fixed Drag Settings")]
	public bool Fixed_Drag = true;
	public bool Fixed_HoldMouseForDrag = true;
	public int Fixed_HoldMouseButton = 0;
	public bool FixedDragIn3D = false;
	public LayerMask FixedDragOnRayCastLayer;
	public bool Fixed_AbsoluteDrag = false;

	public enum OrbitPositionMode
	{
		POSITIONBASED,
		ROTATIONBASED
	}

	//[Header("Orbit Settings")]
	public Vector3 OrbitCenter;
	public OrbitPositionMode CurrentOrbitPositionMode = OrbitPositionMode.ROTATIONBASED;
	[Range(0, 1f)]
	public float Orbit_RotationBasedSpeed = 0.1f;
	[Range(0, 1f)]
	public float Orbit_RotationBasedDamping = 0.1f;
	public string Orbit_HorizontalAxis = "Mouse X";
	public float OrbitMouseXSpeed = 10f;
	public string Orbit_VerticalAxis = "Mouse Y";
	public float OrbitMouseYSpeed = 10f;

	//[Header("Orbit Position Constraint Settings")]
	public float MinOrbitY = -float.MaxValue;
	public bool OrbitCheckSightDown = true;
	public LayerMask CheckSightDownMask;
	public float OversightDown = 10f;
	public float OversightDownDistance = 1f;

	//[Header("Orbit Zoom Settings")]
	public string Orbit_ZoomAxis = "Mouse ScrollWheel";
	public float OrbitMouseZSpeed = 40f;
	public float MinOrbitDistance = 2f;
	public float MaxOrbitDistance = 50f;
	public bool OrbitZoomSpeedBasedOnDistance = true;
	public float OrbitDistanceZoomMult = 3f;
	public float OrbitMinZoomSpeed = 0.05f;
	public float OrbitMaxZoomSpeed = 5f;

	//[Header("Orbit Zoom Constraint Settings")]
	public bool OrbitCheckSight = true;
	public LayerMask CheckSightMask;
	public float Oversight = 10f;

	//[Header("Orbit Look Rotation Settings")]
	public bool OrbitHoldMouseButtonForRotation = true;
	public int OrbitMouseButtonForRotation = 1;
	public bool OrbitSmoothStartLookAt = true;
	public float OrbitLookAtSpeed = 0.2f;
	public float Orbit_MaxSmoothRotationSpeed = 0.5f;
	public float OrbitMinAngleToFinishSmoothLookAt = 0.1f;

	bool LookAtFinished = false;
	float delta = 0f;
	Vector3 Difference;
	float distance = 0;

	Vector3 currentRotation = Vector3.zero;
	Vector3 wantedRotation = Vector3.zero;
	Vector3 angularVelocity = Vector3.zero;

	void Awake()
	{
		cam = GetComponent<Camera>();
		trans = GetComponent<Transform>();
		SetWantedPosition(trans.position);

		FloorPlane = new Plane(Fixed_FloorPosition, Fixed_FloorNormal);
	}

	void Update()
	{
		delta = Time.deltaTime / 0.016f;

		if (LastCameraMode != CurrentCameraMode)
		{
			//Mode changed
			if (CurrentCameraMode == CameraMode.Orbit)
			{
				Difference = trans.position - OrbitCenter;
				LookAtFinished = false;
				wantedRotation = Quaternion.LookRotation(Difference).eulerAngles;
				angularVelocity = Vector3.zero;
			}
			else if (CurrentCameraMode == CameraMode.FixedRotation)
			{
				LookAtFinished = false;
			}
		}
		LastCameraMode = CurrentCameraMode;

		UpdateWantedPosition();
		UpdateMovement();
		UpdateRotation();
	}

	void UpdateWantedPosition()
	{
		if (CurrentCameraMode == CameraMode.FreeCam)
		{
			float inputX = Input.GetAxis(Free_HorizontalAxis);
			float inputY = Input.GetAxis(Free_UpDownAxis);
			float inputZ = Input.GetAxis(Free_VerticalAxis);

			wantedPosition += delta * trans.right * inputX * FreeMovementXSpeed;
			wantedPosition += delta * Vector3.up * inputY * FreeMovementYSpeed;
			wantedPosition += delta * trans.forward * inputZ * FreeMovementZSpeed;
		}
		else if (CurrentCameraMode == CameraMode.FixedRotation)
		{
			float inputX = Input.GetAxis(Fixed_XAxisInput);
			float inputY = Input.GetAxis(Fixed_HeightAxisInput);
			float inputZ = Input.GetAxis(Fixed_ZAxisInput);

			Vector3 move = Vector3.zero;

			if (Fixed_KeyboardInput)
			{
				move = Fixed_XAxis * delta * inputX * Fixed_XSpeed;
				if (!Fixed_XWorldSpace)
					move = trans.TransformDirection(move);
				move.y = 0;
				wantedPosition += move;

				move = Fixed_ZAxis * delta * inputZ * Fixed_ZSpeed;
				if (!Fixed_ZWorldSpace)
					move = trans.TransformDirection(move);
				move.y = 0;
				wantedPosition += move;
			}

			if (Fixed_Drag)
			{
				Vector2 touchCenter = Input.mousePosition;
				if (Fixed_TouchInput)
				{
					//Touch Input
					if (Input.touchCount >= 1)
					{
						touchCenter = Vector2.zero;
						foreach (var touch in Input.touches)
						{
							touchCenter += touch.position;
						}
						touchCenter /= Input.touchCount;
						TouchHad = true;

						if (Input.touchCount != LastTouchCount)
							TouchHad = false;
						LastTouchCount = Input.touchCount;
					}
					else
					{
						TouchHad = false;
					}
				}
				else
				{
					if (Fixed_HoldMouseForDrag && Input.GetMouseButton(Fixed_HoldMouseButton))
					{
						TouchHad = true;
						if (1 != LastTouchCount)
							TouchHad = false;
						LastTouchCount = 1;
					}
					else
					{
						TouchHad = false;
						LastTouchCount = 0;
					}
				}

				FloorPlane.SetNormalAndPosition(Fixed_FloorNormal, Fixed_FloorPosition);

				Ray ray = cam.ScreenPointToRay(touchCenter);

				Vector3 current3DPosition = Last3DPosition;
				if (!FixedDragIn3D)
				{
					float hitDistance = wantedPosition.y;
					FloorPlane.Raycast(ray, out hitDistance);

					current3DPosition = ray.GetPoint(hitDistance) - trans.position;
				}
				else
				{
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, float.MaxValue, FixedDragOnRayCastLayer))
					{
						current3DPosition = hit.point - trans.position;
					}
					else
					{
						current3DPosition = Last3DPosition;
					}
				}

				move = Last3DPosition - current3DPosition;
				move.y = 0;

				if (!TouchHad)
					move = Vector3.zero;

				if (Fixed_AbsoluteDrag)
					savedMovement += move;

				wantedPosition += move;
				
				LastMousePosition = touchCenter;
				Last3DPosition = current3DPosition;
			}

			

			if (Fixed_HeightAxisToMouse)
			{
				Vector2 touchCenter = Input.mousePosition;
				float zoomInput = 0f;

				if (Fixed_TouchInput)
				{
					if (Input.touchCount >= 1)
					{
						touchCenter = Vector2.zero;
						foreach (var touch in Input.touches)
						{
							touchCenter += touch.position;
						}
						touchCenter /= Input.touchCount;
						TouchHad = true;
					}
					else
					{
						TouchHad = false;
					}

					//Touch Input
					if (Input.touchCount == 2)
					{
						Touch touchZero = Input.GetTouch(0);
						Touch touchOne = Input.GetTouch(1);

						// Find the position in the previous frame of each touch.
						Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
						Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

						touchCenter = (touchZero.position + touchOne.position) / 2f;

						// Find the magnitude of the vector (the distance) between the touches in each frame.
						float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
						float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

						if (TouchHad)
						{
							// Find the difference in the distances between each frame.
							zoomInput = (prevTouchDeltaMag - touchDeltaMag) * Fixed_PinchZoomSpeed * delta;
						}
						TouchHad = true;
					}
					else
					{
						TouchHad = false;
					}
				}
				else
				{
					//Axis Input
					zoomInput += delta * inputY * Fixed_HeightSpeed;
					touchCenter = Input.mousePosition;
				}

				Ray ray = cam.ScreenPointToRay(touchCenter);

				move = ray.direction * zoomInput;

				wantedPosition -= move;

				RaycastHit hit;
				if (FixedCheckSightDown && Physics.Raycast(wantedPosition + Vector3.up * FixedOversightDown, Vector3.down, out hit, FixedOversightDown + FixedOversightDownDistance, FixedCheckSightDownMask))
				{
					wantedPosition.y = hit.point.y + FixedOversightDownDistance;
				}
			}
			else
			{
				move = Fixed_HeightAxis * delta * inputY * Fixed_HeightSpeed;
				if (!Fixed_HeightWorldSpace)
					move = trans.InverseTransformPoint(move);
				wantedPosition += move;
			}
			wantedPosition.y = Mathf.Clamp(wantedPosition.y, FixedMinPosition.y, FixedMaxPosition.y);
		}
		else if (CurrentCameraMode == CameraMode.Orbit)
		{
			float inputX = Input.GetAxis(Orbit_HorizontalAxis) * OrbitMouseXSpeed * delta;
			float inputY = -Input.GetAxis(Orbit_VerticalAxis) * OrbitMouseYSpeed * delta;
			float inputZ = -Input.GetAxis(Orbit_ZoomAxis) * OrbitMouseZSpeed * delta;

			Difference = wantedPosition - OrbitCenter;

			if (OrbitZoomSpeedBasedOnDistance)
			{
				float zoomSpeed = Mathf.Clamp(Difference.magnitude, MinOrbitDistance, MaxOrbitDistance) / MaxOrbitDistance;
				zoomSpeed = Mathf.Clamp(zoomSpeed * OrbitDistanceZoomMult, OrbitMinZoomSpeed, OrbitMaxZoomSpeed);
				inputZ *= zoomSpeed;
			}

			float minDistance = MinOrbitDistance;
			RaycastHit hit;
			if (Physics.Raycast(trans.position - trans.forward * Oversight, trans.forward, out hit, MaxOrbitDistance + Oversight, CheckSightMask))
				minDistance = (OrbitCenter - hit.point).magnitude + MinOrbitDistance;
			if (Difference.magnitude + inputZ < minDistance)
				Difference = Difference.normalized * minDistance;
			else
				Difference = Vector3.ClampMagnitude(Difference + Difference.normalized * inputZ, MaxOrbitDistance);
			distance = Difference.magnitude;

			wantedPosition = OrbitCenter + Difference;

			if (!OrbitHoldMouseButtonForRotation || Input.GetMouseButton(OrbitMouseButtonForRotation))
			{
				wantedPosition = RotateAroundPoint(wantedPosition, OrbitCenter, Quaternion.Euler(Vector3.up * inputX + trans.right * inputY));
			}
			wantedPosition.y = Mathf.Max(wantedPosition.y, OrbitCenter.y + MinOrbitY);
			if (Physics.Raycast(wantedPosition + Vector3.up * OversightDown, Vector3.down, out hit, OversightDown + OversightDownDistance, CheckSightDownMask))
			{
				wantedPosition.y = hit.point.y + OversightDownDistance;
			}

			if (CurrentOrbitPositionMode == OrbitPositionMode.POSITIONBASED)
			{
				
			}
			else if (CurrentOrbitPositionMode == OrbitPositionMode.ROTATIONBASED)
			{
				if (!OrbitHoldMouseButtonForRotation || Input.GetMouseButton(OrbitMouseButtonForRotation))
				{
					/*Difference = wantedPosition - OrbitCenter;
					wantedRotation = Quaternion.LookRotation(Difference).eulerAngles;
					wantedRotation += Vector3.up * inputX;
					wantedRotation += trans.right * inputY;*/

					angularVelocity += Vector3.up * inputX;
					angularVelocity += trans.right * inputY;
				}
			}
		}
	}

	public Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
	{
		return angle * ( point - pivot) + pivot;
	}

	private float computeSpeed(float input, float procent, float multiply)
	{
		if ((input > 0 && procent < 0.5f) || (input < 0 && procent > 0.5f))
		{
			return input;
		}
		else
		{
			return input * multiply;
		}
	}

	private float computeBorderSpeed(float targetPos, float minPos, float maxPos, float borderSize)
	{
		float output = 0.0f;

		if (targetPos < (minPos + borderSize))
		{
			//Objekt ist auf Linker Seite (output 0 -> bordersize)
			output = Mathf.Clamp(targetPos - (minPos + borderSize), -borderSize, 0) + borderSize;
		}
		else if (targetPos > (maxPos - borderSize))
		{
			//Objekt ist auf Rechter Seite (output 0 -> bordersize)
			output = Mathf.Clamp(targetPos - (maxPos - borderSize), 0, borderSize) - borderSize;
		}
		else
		{
			output = borderSize;
		}
		output = Mathf.Clamp((Mathf.Abs(output) / borderSize), 0, 1);

		return output;
	}

	private void UpdateProcentages(Vector3 pos)
	{
		procentValues.x = (pos.x - FixedMinPosition.x) / (FixedMaxPosition.x - FixedMinPosition.x);
		procentValues.y = (pos.y - FixedMinPosition.y) / (FixedMaxPosition.y - FixedMinPosition.y);
		procentValues.z = (pos.z - FixedMinPosition.z) / (FixedMaxPosition.z - FixedMinPosition.z);
	}

	void UpdateRotation()
	{
		if (CurrentCameraMode == CameraMode.FreeCam)
		{
			float inputX = Input.GetAxis(Free_RotateXAxis);
			float inputY = Input.GetAxis(Free_RotateYAxis);
			if (FreeHoldMouseButtonForRotation && !Input.GetMouseButton(FreeMouseButtonForRotation))
				return;
			trans.Rotate(trans.InverseTransformDirection(Vector3.up), delta * FreeMouseXSpeed * inputX);
			trans.Rotate(Vector3.right, -delta * FreeMouseYSpeed * inputY);
		}
		else if (CurrentCameraMode == CameraMode.FixedRotation)
		{
			if (LookAtFinished)
			{
				if (Fixed_FreezeRotation)
				{
					trans.rotation = Quaternion.Euler(Fixed_WantedRotation);
				}
			}
			else
			{
				if (Fixed_SmoothToRotation)
				{
					Quaternion targetRotation = Quaternion.Euler(Fixed_WantedRotation);
					trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, Mathf.Min(Fixed_SmoothToRotationSpeed * delta, Fixed_MaxSmoothRotationSpeed));
					if (Quaternion.Angle(targetRotation, trans.rotation) < Fixed_MinAngleToFinishSmoothLookAt)
						LookAtFinished = true;
				}
			}
		}
		else if (CurrentCameraMode == CameraMode.Orbit)
		{
			if (LookAtFinished)
			{
				trans.LookAt(OrbitCenter);
			}
			else
			{
				if (OrbitSmoothStartLookAt)
				{
					Quaternion targetRotation = Quaternion.LookRotation(OrbitCenter - trans.position);
					trans.rotation = Quaternion.Slerp(trans.rotation, targetRotation, Mathf.Min(OrbitLookAtSpeed * delta, Orbit_MaxSmoothRotationSpeed));
					if (Quaternion.Angle(targetRotation, trans.rotation) < OrbitMinAngleToFinishSmoothLookAt)
						LookAtFinished = true;
				}
			}
			
			
		}
	}

	#region Movement

	Vector3 wantedPosition;

	public enum MovementMode
	{
		LERP,
		VELOCITY
	}

	[Header("Camera Movement Settings")]
	public MovementMode Mode = MovementMode.VELOCITY;

	[Header("Lerp Settings")]
	[Range(0.01f, 5f)]
	public float LerpSpeed = 1f;

	[Header("Velocity Settings")]
	[Range(0.01f, 5f)]
	public float Force = 2f;
	[Range(0f, 1f)]
	public float Dampening = 0.6f;
	[Range(0.01f, 25f)]
	public float MaximumSpeed = 5f;

	[Range(0f, 1000f)]
	public float SlowDownDistance = 20f;
	Vector3 currentVelocity;

	public void SetWantedPosition(Vector3 position)
	{
		wantedPosition = position;
	}

	void UpdateMovement()
	{
		Vector3 position = trans.position;
		if (CurrentCameraMode == CameraMode.FixedRotation && Fixed_AbsoluteDrag)
		{
			Vector3 currentMovement = savedMovement * Mathf.Clamp(Time.deltaTime * 60.0f, 0f, 1f);
			savedMovement -= currentMovement;

			UpdateProcentages(position);
			float speedX = computeBorderSpeed(position.x, FixedMinPosition.x, FixedMaxPosition.x, 1f);
			float speedY = computeBorderSpeed(position.y, FixedMinPosition.y, FixedMaxPosition.y, 1f);
			float speedZ = computeBorderSpeed(position.z, FixedMinPosition.z, FixedMaxPosition.z, 1f);

			currentMovement.x = computeSpeed(currentMovement.x, procentValues.x, speedX);
			currentMovement.y = computeSpeed(currentMovement.y, procentValues.y, speedY);
			currentMovement.z = computeSpeed(currentMovement.z, procentValues.z, speedZ);


			position += currentMovement;

			position.x = Mathf.Clamp(position.x, FixedMinPosition.x, FixedMaxPosition.x);
			//newPos.y = Mathf.Clamp(newPos.y, minPosition.Value.y, maxPosition.Value.y);

			position.z = Mathf.Clamp(position.z, FixedMinPosition.z, FixedMaxPosition.z);

			//position = wantedPosition;
		}
		else if (CurrentCameraMode == CameraMode.Orbit)
		{
			if (CurrentOrbitPositionMode == OrbitPositionMode.ROTATIONBASED)
			{
				Vector3 rotDiff = Vector3.zero;

				//angularVelocity -= Vector3.ClampMagnitude(angularVelocity * delta * Orbit_RotationBasedDamping, angularVelocity.magnitude);
				rotDiff = angularVelocity * delta * Orbit_RotationBasedSpeed;
				angularVelocity -= rotDiff;

				Difference = position - OrbitCenter;
				float length = Mathf.Lerp(Difference.magnitude, distance, Mathf.Clamp01(delta * Orbit_RotationBasedSpeed));
				Difference = Difference.normalized * length;

				position = OrbitCenter + Difference;

				position = RotateAroundPoint(position, OrbitCenter, Quaternion.Euler(rotDiff));
				position.y = Mathf.Max(position.y, OrbitCenter.y + MinOrbitY);

				RaycastHit hit;
				if (Physics.Raycast(position + Vector3.up * OversightDown, Vector3.down, out hit, OversightDown + OversightDownDistance, CheckSightDownMask))
				{
					position.y = hit.point.y + OversightDownDistance;
				}
			}
		}
		else
		{
			if (Mode == MovementMode.LERP)
			{
				position = Vector3.Lerp(position, wantedPosition, Mathf.Clamp01(delta * LerpSpeed));
			}
			else if (Mode == MovementMode.VELOCITY)
			{
				Vector3 difference = wantedPosition - position;
				difference = delta * difference.normalized * Force * (Mathf.Min(difference.magnitude, SlowDownDistance) / (SlowDownDistance <= 0 ? 1f : SlowDownDistance));
				currentVelocity = Vector3.ClampMagnitude(currentVelocity + difference, MaximumSpeed);
				currentVelocity -= Vector3.ClampMagnitude(delta * currentVelocity * Dampening, currentVelocity.magnitude);
				position += delta * currentVelocity;
			}
		}

		trans.position = position;

	}

	#endregion
}
