using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public enum direction { STOP = 0, LEFT = -1, RIGHT = 1 };
	public direction Direction;
	public static int Horizontal;
	public static bool IsPressedLeft = false;
	public static bool IsPressedRight = false;

	public void OnPointerDown(PointerEventData eventData)
	{
		Horizontal = Direction == direction.LEFT ? -1 : 1;
		if (Horizontal == 1)
			IsPressedRight = true;
		else
			IsPressedLeft = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Horizontal = 0;
		IsPressedLeft = IsPressedRight = false;
	}
}
