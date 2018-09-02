using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public static bool IsPressed = false;
	public static bool IsPressedDown = false;
	public static bool IsPressedUp = false;

	private float timer_pressed_down = 0;
	private float time_pressed_down = 0.05f;

	private float timer_pressed_up = 0;
	private float time_pressed_up = 0.4f;

	private void Update()
	{
		if (IsPressedDown)
		{
			timer_pressed_down += Time.deltaTime;
			if (timer_pressed_down >= time_pressed_down)
			{
				timer_pressed_down = 0;
				IsPressedDown = false;
			}
		}
		if (IsPressedUp)
		{
			timer_pressed_up += Time.deltaTime;
			if (timer_pressed_up >= time_pressed_up)
			{
				timer_pressed_up = 0;
				IsPressedUp = false;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		IsPressed = true;
		IsPressedDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		IsPressed = false;
		IsPressedUp = true;
	}
}
