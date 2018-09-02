using UnityEngine;

public class CameraObserver : MonoBehaviour
{
	private Transform position_camera;
	private float max_player_x;

	private void Start()
	{
		position_camera = GetComponent<Transform>();
		max_player_x = position_camera.position.x / 2;
	}

	private void Update()
	{
		if (Logic.Instance.Player == null)
			Logic.Instance.Player = GameObject.FindGameObjectWithTag("Player");
		if (Logic.Instance.Player.transform.position.x > max_player_x)
		{
			if (Logic.Instance.Player.transform.position.x < 0)
				max_player_x = Logic.Instance.Player.transform.position.x + 0.01f;
			else
				max_player_x = Logic.Instance.Player.transform.position.x - 0.01f;
			position_camera.position = new Vector3(Logic.Instance.Player.transform.position.x, position_camera.position.y, -10);
		}
	}
}
