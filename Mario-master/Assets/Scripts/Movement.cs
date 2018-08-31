using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
	[Header("Physics")]
	public LayerMask Ground;
	public Transform CoordinateLegs;
	public float Speed;
	public float JumpForce;

	private Animator state; // 0 - idle, 1 - jump, 2 - run, 3 - drift, 4 - death/sit
	private SpriteRenderer flip;
	private Rigidbody2D body_player;
	private bool IsGround;
	private bool IsDriftAvailable;
	private float agony_timer;
	private float agony_time;
	private float drift_timer;
	private float drift_time;
	private float timer_on_drift;
	private float time_on_drift;
	private float basic_jump_force;
	private const int max_jump_force = 28;
	private float basic_speed;
	private const int max_speed = 26;
	private int last_direction; // this is var. need for Drift

	private void Start()
	{
		state = GetComponent<Animator>();
		flip = GetComponent<SpriteRenderer>();
		body_player = GetComponent<Rigidbody2D>();
		IsGround = false;
		IsDriftAvailable = false;
		agony_timer = 0;
		agony_time = 2.5f;
		drift_timer = 0;
		drift_time = 0.3f;
		timer_on_drift = 0;
		time_on_drift = 0.5f;
		basic_jump_force = JumpForce;
		basic_speed = Speed;
		last_direction = 1;
	}

	private void FixedUpdate()
	{
		if (!Logic.Instance.IsGameOver && !Logic.Instance.StayPlayer)
		{
			IsGround = Physics2D.OverlapCircle(CoordinateLegs.position, 0.1f, Ground);
			Move();
			if (InputUI.GetKeyDown(UIKeyCode.FIRE))
				UpSpeed(2);
			if (IsGround)
				JumpForce = basic_jump_force; // default jump when player on ground
			if (InputUI.GetKey(UIKeyCode.SPACE) && !IsGround)
				UpJumpForceInAir();
			if (InputUI.GetKeyDown(UIKeyCode.SPACE) && IsGround)
				Jump(JumpForce);
			if ((InputUI.GetKey(UIKeyCode.DOWN) || Input.GetKey(KeyCode.S)) && Logic.Instance.ModePlayer != Mode.LITTLE)
				Sit();
		}
	}

	private void Update()
	{
		if (Logic.Instance.IsGameOver) // time to restart lvl, if mario is dead
		{
			agony_timer += Time.deltaTime;
			if (agony_timer >= agony_time)
				SceneManager.LoadScene("World 1-1");
		}
		else
		{
			#region DriftCrutch
			// if direction change, then animation of drift is on
			if (IsDriftAvailable && InputUI.GetAxis("Horizontal") != last_direction && InputUI.GetAxis("Horizontal") != 0)
				Drift();
			else if (IsDriftAvailable && body_player.velocity.x > -0.25f && body_player.velocity.x < 0.25f)
				IsDriftAvailable = false;
			#endregion
			// if player in motion, then the counter is accumulated for possible the drift
			if (InputUI.GetAxis("Horizontal") != 0 && !IsDriftAvailable)
			{
				drift_timer += Time.deltaTime;
				if (drift_timer >= drift_time)
				{
					last_direction = InputUI.GetAxis("Horizontal");
					IsDriftAvailable = true;
					drift_timer = 0;
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Destroy")
			Dead();
		else if (collision.gameObject.tag == "Coin")
		{
			Destroy(collision.gameObject);
			Logic.Instance.Coin.Play();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
        // invisible walls(childs object of the camera) that be under the camera
        if (collision.gameObject.tag == "Enemy" && Logic.Instance.ModePlayer == Mode.LITTLE)
        {
            if (!(gameObject.transform.position.y - collision.gameObject.transform.position.y >= 0.5))
                Dead();
            else
                Jump(JumpForce / 3);
        }
	}

	private void Move()
	{
		if (!IsDriftAvailable && IsGround && !Shoot.IsShoot)
		{
			if (InputUI.GetAxis("Horizontal") != 0)
			{
				Flip(InputUI.GetAxis("Horizontal"));
				state.SetInteger("State", (int)State.RUN);
			}
			else
			{
				Speed = basic_speed;
				state.SetInteger("State", (int)State.IDLE);
			}
			/*if (Input.GetAxis("Horizontal") != 0)
			{
				Flip(Input.GetAxis("Horizontal"));
				state.SetInteger("State", (int)State.RUN);
			}
			else
			{
				Speed = basic_speed;
				state.SetInteger("State", (int)State.IDLE);
			}*/
		}
		body_player.AddForce(new Vector2(InputUI.GetAxis("Horizontal") * Speed, body_player.velocity.y));
		body_player.AddForce(new Vector2(Input.GetAxis("Horizontal") * Speed, body_player.velocity.y));
	}

	private void Flip(float move_x)
	{
		if (move_x == 1)
			flip.flipX = false;
		else if (move_x == -1)
			flip.flipX = true;
	}

	private void UpJumpForceInAir()
	{
		if (JumpForce <= max_jump_force)
		{
			JumpForce += 2;
			body_player.velocity = new Vector2(body_player.velocity.x, JumpForce / 2);
		}
	}

	private void UpSpeed(int speed)
	{
		if (Speed <= max_speed)
			Speed += speed;
	}

	private void Jump(float jumpForce)
	{
		//Debug.Log("Jump");
		state.SetInteger("State", (int)State.JUMP);
		body_player.velocity = new Vector2(body_player.velocity.x, jumpForce);
		if (Logic.Instance.ModePlayer == Mode.LITTLE)
			Logic.Instance.JumpLittle.Play();
		else
			Logic.Instance.JumpSupper.Play();
	}

	private void Dead()
	{
		Logic.Instance.IsGameOver = true;
		GetComponent<BoxCollider2D>().enabled = false; // to go through some walls
		GetComponent<CircleCollider2D>().enabled = false;
		body_player.gravityScale = 1;
		if (Logic.Instance.ModePlayer == Mode.LITTLE)
		{
			body_player.velocity = new Vector2(0, JumpForce - 10); // num 10 need for really jump
			state.SetInteger("State", (int)State.DEATH_OR_SIT);
		}
		else
			body_player.velocity = new Vector2(0, body_player.velocity.y);
		Logic.Instance.BackgroundSound.Stop();
		Logic.Instance.MarioDie.Play();
	}

	private void Drift()
	{
		Flip(InputUI.GetAxis("Horizontal"));
		state.SetInteger("State", (int)State.DRIFT);
		timer_on_drift += Time.deltaTime;
		if (timer_on_drift >= time_on_drift)
		{
			IsDriftAvailable = false;
			timer_on_drift = 0;
		}
	}

	private void Sit()
	{
		FireButton.IsPressedDown = false; // because Mario can't shoot sitting down
		state.SetInteger("State", (int)State.DEATH_OR_SIT);
	}
}

public enum State
{
	// all mode
	IDLE = 0,
	JUMP = 1,
	RUN = 2,
	DRIFT = 3,
	DEATH_OR_SIT = 4,
	DIFFERENT = 5,
	// all besides little
	IMMORTAL = 6,
	// only the fire mode
	SHOOT = 7
};
