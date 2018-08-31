using UnityEngine;

public class CameraObserver : MonoBehaviour
{
	public GameObject Player;

	private Transform position_camera;
	private float max_player_x;

	private void Start()
	{
		position_camera = GetComponent<Transform>();
		max_player_x = position_camera.position.x / 2;
	}

	private void Update()
	{
		if (Player == null) // if old mario is destroy, then need find the reference on the new Mario
			Player = GameObject.FindGameObjectWithTag("Player");
		if (Player.transform.position.x > max_player_x)
		{
			if (Player.transform.position.x < 0)
				max_player_x = Player.transform.position.x + 0.01f;
			else
				max_player_x = Player.transform.position.x - 0.01f;
			position_camera.position = new Vector3(Player.transform.position.x, position_camera.position.y, -10);
		}
	}
}
