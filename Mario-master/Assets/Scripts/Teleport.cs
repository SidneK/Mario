using UnityEngine;

public class Teleport : MonoBehaviour
{
	private bool isTeleport = false;
	private float timer_on_direction = 0;
	private float time_on_direction = 1.5f;

	private enum Direction { LEFT, DOWN, RIGHT, UP };
	private Direction direction;

	private void Update()
	{
		if (isTeleport)
			OnDirection();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Secret" && !isTeleport)
		{
			if (InputUI.GetKeyDown(UIKeyCode.DOWN))
			{
				isTeleport = true;
				direction = Direction.DOWN;
				GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
				Logic.Instance.BackgroundMain.Stop();
				Logic.Instance.PipeAndDownLevel.Play();
				Invoke("TeleportToSecret", time_on_direction);
				Invoke("PlayBackgroundSecret", time_on_direction);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Main" && !isTeleport)
		{
			isTeleport = true;
			direction = Direction.RIGHT;
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			Logic.Instance.BackgroundSecret.Stop();
			Logic.Instance.PipeAndDownLevel.Play();
			Invoke("TeleportToMain", time_on_direction);
		}
		else if (collision.gameObject.tag == "UpPlayer")
		{
			isTeleport = true;
			direction = Direction.UP;
			GetComponent<CircleCollider2D>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			Logic.Instance.PipeAndDownLevel.Play();
			if (Logic.Instance.ModePlayer == Mode.LITTLE)
				time_on_direction = 1f;
			Invoke("PlayBackgroundMain", time_on_direction);
		}
	}

	private void OnDirection()
	{
		timer_on_direction += Time.deltaTime;
		float speed_player = 1.5f;
		switch (direction)
		{
			case Direction.RIGHT:
				GetComponent<Animator>().SetInteger("State", (int)State.RUN);
				transform.Translate(new Vector3(speed_player * Time.deltaTime, 0, 0));
				break;
			case Direction.DOWN:
				transform.Translate(new Vector3(0, -speed_player * Time.deltaTime, 0));
				break;
			case Direction.UP:
				transform.Translate(new Vector3(0, speed_player * Time.deltaTime, 0));
				break;
		}
		if (timer_on_direction >= time_on_direction)
		{
			if (direction == Direction.UP)
			{
				GetComponent<CircleCollider2D>().enabled = true;
				GetComponent<BoxCollider2D>().enabled = true;
			}
			GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePosition; // unfreeze x and y are only
			timer_on_direction = 0;
			isTeleport = false;
		}
	}

	private void TeleportToMain()
	{
		TakeTeleport(World.MAIN);
	}

	private void TeleportToSecret()
	{
		TakeTeleport(World.SECRET);
	}

	private void TakeTeleport(World world)
	{
		switch(world)
		{
			case World.MAIN:
				transform.position = TurnCamera.Instance.SpawnMainRoom.position;
				TurnCamera.Instance.Turn(World.MAIN);
				break;
			case World.SECRET:
				transform.position = TurnCamera.Instance.SpawnSecretRoom.position;
				TurnCamera.Instance.Turn(World.SECRET);
				break;
		}
	}

	private void PlayBackgroundSecret()
	{
		Logic.Instance.BackgroundSecret.Play();
	}

	private void PlayBackgroundMain()
	{
		Logic.Instance.BackgroundMain.Play();
	}
}
