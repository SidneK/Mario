using UnityEngine;

public class InputUI : MonoBehaviour
{
	public static bool GetKey(UIKeyCode UIKey)
	{
		switch (UIKey)
		{
			case UIKeyCode.SPACE:
				if (SpaceButton.IsPressed)
					return true;
				return false;
			case UIKeyCode.FIRE:
				if (FireButton.IsPressed)
					return true;
				return false;
			case UIKeyCode.LEFT:
				if (MoveButton.IsPressedLeft)
					return true;
				return false;
			case UIKeyCode.RIGHT:
				if (MoveButton.IsPressedRight)
					return true;
				return false;
			case UIKeyCode.DOWN:
				if (DownButton.IsPressed)
					return true;
				return false;
		}
		throw new System.Exception("It's impossible");
	}

	public static bool GetKeyDown(UIKeyCode UIKey)
	{
		switch (UIKey)
		{
			case UIKeyCode.SPACE:
				if (SpaceButton.IsPressedDown)
					return true;
				return false;
			case UIKeyCode.FIRE:
				if (FireButton.IsPressedDown)
					return true;
				return false;
			case UIKeyCode.LEFT:
				return false;
			case UIKeyCode.RIGHT:
				return false;
			case UIKeyCode.DOWN:
				if (DownButton.IsPressedDown)
					return true;
				return false;
		}
		throw new System.Exception("It's impossible");
	}

	public static bool GetKeyUp(UIKeyCode UIKey)
	{
		switch (UIKey)
		{
			case UIKeyCode.SPACE:
				if (SpaceButton.IsPressedUp)
					return true;
				return false;
			default:
				return false;
		}
		throw new System.Exception("It's impossible");
	}

	public static int GetAxis(string axisName)
	{
		if (axisName == "Horizontal")
			return MoveButton.Horizontal;
		throw new System.Exception("Such the axisName isn't existence");
	}
}

public enum UIKeyCode
{
	SPACE = 0,
	FIRE = 1,
	LEFT = 2,
	RIGHT = 3,
	DOWN = 4
};
