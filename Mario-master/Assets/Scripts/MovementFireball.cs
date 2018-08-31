using UnityEngine;

public class MovementFireball : MonoBehaviour
{
	public float Speed;
	public float JumpForce;

	private Rigidbody2D body_fireball;
	private Animator explosion;
	private bool isExplosion;
	private bool direction; // 1 - left, 0 - right
	private float explosion_timer;
	private float explosion_time;

	private void Awake()
	{
		body_fireball = GetComponent<Rigidbody2D>();
		explosion = GetComponent<Animator>();
		isExplosion = false;
		direction = Shoot.WhereSpawnFireball.flipX;
		explosion_timer = 0;
		explosion_time = 0.3f;
	}

	private void FixedUpdate()
	{
		Move();
		if (isExplosion)
		{
			explosion_timer += Time.deltaTime;
			if (explosion_timer >= explosion_time)
				Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Destroy")
			BlowUp();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
			Kill(collision.gameObject);
		else if (collision.gameObject.tag == "Block" && collision.transform.position.y > transform.position.y)
			BlowUp();
		else if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Floor")
			Jump();
	}

	private void Move()
	{
		if (direction)
			body_fireball.velocity = new Vector2(-Speed, body_fireball.velocity.y);
		else
			body_fireball.velocity = new Vector2(Speed, body_fireball.velocity.y);
	}

	private void Jump()
	{
		body_fireball.velocity = new Vector2(body_fireball.velocity.x, JumpForce);
		transform.Rotate(new Vector3(0, 0, 90));
	}

	private void Kill(GameObject enemy)
	{
		Destroy(enemy);
		BlowUp();
	}

	private void BlowUp()
	{
		isExplosion = true;
		body_fireball.constraints = RigidbodyConstraints2D.FreezeAll;
		explosion.SetInteger("State", 1);
		Logic.Instance.FireballExplosion.Play();
		--Shoot.CountFireball;
	}
}
