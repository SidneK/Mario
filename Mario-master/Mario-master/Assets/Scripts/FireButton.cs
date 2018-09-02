using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public static bool IsPressed = false;
	public static bool IsPressedDown = false;

	private float timer_pressed_down = 0;
	private float time_pressed_down = 0.025f;

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
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		IsPressed = true;
		IsPressedDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		IsPressed = false;
	}
}
