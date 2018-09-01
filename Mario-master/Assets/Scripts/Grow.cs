using UnityEngine;

public class Grow : MonoBehaviour
{
	private Rigidbody2D body_player;
	private Transform coordinate_player;
	private Animator different;
	private SpriteRenderer sprite;
	private float timer_grow = 0;
	private const float time_grow = 1.4f;
	private float immortal_counter = 0;
	private static float immortal_timer = 0;
	private const float immortal_time = 4;

	private void Start()
	{
		body_player = GetComponent<Rigidbody2D>();
		coordinate_player = GetComponent<Transform>();
		different = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (Logic.Instance.StayPlayer && !Logic.Instance.IsImmortalPlayer)
		{
			timer_grow += Time.deltaTime;
			if (timer_grow >= time_grow)
			{
				Logic.Instance.StayPlayer = false;
				timer_grow = 0;
				if (Logic.Instance.ModePlayer == Mode.LITTLE)
					ChangeMode(Mode.BIG);
			}
		}
		if (Logic.Instance.IsImmortalPlayer)
		{
			Flashing();
			MakeImmortal();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Bonus")
		{
			Destroy(collision.gameObject);
			if (Logic.Instance.ModePlayer == Mode.BIG)
				ChangeMode(Mode.FIRE);
			else if (Logic.Instance.ModePlayer == Mode.LITTLE)
			{
				body_player.constraints = RigidbodyConstraints2D.FreezeAll;
				Logic.Instance.StayPlayer = true;
				different.SetInteger("State", (int)State.DIFFERENT);
			}
			Logic.Instance.MarioUp.Play();
		}
		// * need to add a condition, if Mario's killing a enemy
		else if (collision.gameObject.tag == "Enemy" && Logic.Instance.ModePlayer != Mode.LITTLE)
		{
			different.SetInteger("State", (int)State.IMMORTAL);
			Logic.Instance.StayPlayer = true;
			Logic.Instance.IsImmortalPlayer = true;
			Logic.Instance.PipeAndDownLevel.Play();
		}
	}

	private void ChangeMode(Mode mode_player)
	{
		Logic.Instance.ModePlayer = mode_player;
		Destroy(gameObject); // destroy old player
		switch (mode_player) // and instantiate new player
		{
			case Mode.LITTLE:
				Instantiate(Logic.Instance.MarioLittle, coordinate_player.position, Quaternion.identity);
				break;
			case Mode.BIG:
				coordinate_player.position = new Vector3(coordinate_player.position.x, coordinate_player.position.y + 0.32f, 0);
				Instantiate(Logic.Instance.MarioBig, coordinate_player.position, Quaternion.identity);
				break;
			case Mode.FIRE:
				Instantiate(Logic.Instance.MarioFire, coordinate_player.position, Quaternion.identity);
				break;
		}
	}

	private void Flashing()
	{
		immortal_counter += Time.deltaTime;
		if (immortal_counter >= 0.3f)
			sprite.enabled = false;
		if (immortal_counter >= 0.8f)
		{
			sprite.enabled = true;
			immortal_counter = 0;
		}
	}

	private void MakeImmortal()
	{
		immortal_timer += Time.deltaTime;
		if (immortal_timer >= 2 && Logic.Instance.StayPlayer)
		{
			Logic.Instance.StayPlayer = false;
			ChangeMode(Mode.LITTLE);
		}
		if (immortal_timer >= immortal_time)
		{
			sprite.enabled = true;
			immortal_timer = 0;
			Logic.Instance.IsImmortalPlayer = false;
		}
	}
}
