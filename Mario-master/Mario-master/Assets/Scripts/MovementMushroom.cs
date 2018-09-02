using UnityEngine;

public class MovementMushroom : MonoBehaviour
{
	public float Speed;

	private Rigidbody2D body_mushroom;
	private SpriteRenderer flip;

	private void Start()
	{
		body_mushroom = GetComponent<Rigidbody2D>();
		flip = GetComponent<SpriteRenderer>();
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Destroy")
			Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Block" && collision.transform.position.y > transform.position.y)
			flip.flipX = !flip.flipX; // inverse a direction
	}

	private void Move()
	{
		int moveX = flip.flipX ? -1 : 1; // if flipX - false, then right, else true - left 
		body_mushroom.velocity = new Vector2(Speed * moveX, body_mushroom.velocity.y);
	}
}
