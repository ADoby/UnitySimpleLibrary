using UnityEngine;
using System.Collections;

public class HouseClick : MonoBehaviour
{
	Renderer renderer;
	public delegate void ClickEvent();
	public ClickEvent OnMouseClick;

	public SimpleCamera cam;
	public SimpleCamera.CameraMode SetMode;

	public GameObject Other;

	void Awake()
	{
		renderer = GetComponent<Renderer>();
		renderer.enabled = false;
	}

	void OnMouseEnter()
	{
		renderer.enabled = true;
	}
	void OnMouseExit()
	{
		renderer.enabled = false;
	}

	void OnMouseUpAsButton()
	{
		if (OnMouseClick != null) OnMouseClick();
		if (cam)
		{
			cam.CurrentCameraMode = SetMode;
			if (SetMode == SimpleCamera.CameraMode.Orbit)
			{
				cam.OrbitCenter = transform.position;
			}
		}
		gameObject.SetActive(false);
		Other.SetActive(true);
	}
}
