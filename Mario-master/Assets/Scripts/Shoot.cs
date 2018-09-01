using UnityEngine;

public class Shoot : MonoBehaviour
{
	public GameObject Fireball;

	public static SpriteRenderer WhereSpawnFireball; // work with the script MovementFireball
	private Transform spawn_fireball_left;
	private Transform spawn_fireball_right;
	private Animator shoot;

	private const int max_fireball = 2;
	public static int CountFireball; // work with the script MovementFireball

	public static bool IsShoot;
	private float shoot_timer;
	private float shoot_time;

	private void Start()
	{
		WhereSpawnFireball = GetComponent<SpriteRenderer>();
		spawn_fireball_left = transform.Find("SpawnFireballLeft");
		spawn_fireball_right = transform.Find("SpawnFireballRight");
		shoot = GetComponent<Animator>();
		CountFireball = 0;
		IsShoot = false;
		shoot_timer = 0;
		shoot_time = 0.25f;
	}

	private void FixedUpdate()
	{
		if (InputUI.GetKeyDown(UIKeyCode.FIRE) && Logic.Instance.ModePlayer == Mode.FIRE && CountFireball < max_fireball)
			Fire();
		if (IsShoot)
		{
			shoot_timer += Time.deltaTime;
			if (shoot_timer >= shoot_time)
			{
				IsShoot = false;
				shoot_timer = 0;
			}
		}
	}

	private void Fire()
	{
		shoot.SetInteger("State", (int)State.SHOOT);
		IsShoot = true;
		Logic.Instance.MarioShoot.Play();
		if (WhereSpawnFireball.flipX)
			Instantiate(Fireball, spawn_fireball_left.transform.position, Quaternion.identity);
		else
			Instantiate(Fireball, spawn_fireball_right.transform.position, Quaternion.identity);
		++CountFireball;
	}
}
