using UnityEngine;

public class TurnCamera : MonoBehaviour
{
	public Camera MainWorld;
	public Camera SecretWorld;
	public Transform SpawnSecretRoom;
	public Transform SpawnMainRoom;

	private static Canvas Interface;

	public static TurnCamera Instance { get; private set; }

	public void Start()
	{
		Instance = this;
		Interface = GetComponent<Canvas>();
		SecretWorld.enabled = false;
	}

	public void Turn(World world)
	{
		switch (world)
		{
			case World.MAIN:
				MainWorld.enabled = true;
				SecretWorld.enabled = false;
				Interface.worldCamera = MainWorld;
				break;
			case World.SECRET:
				SecretWorld.enabled = true;
				MainWorld.enabled = false;
				Interface.worldCamera = SecretWorld;
				break;
		}
	}

	public void SetInActiveCamera()
	{
		GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		MainCamera.GetComponent<CameraObserver>().enabled = false;
	}

	public void SetActiveCamera()
	{
		GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		MainCamera.GetComponent<CameraObserver>().enabled = true;
	}
}

public enum World
{
	MAIN = 0,
	SECRET = 1
};
