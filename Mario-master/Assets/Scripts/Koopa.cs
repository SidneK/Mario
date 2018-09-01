using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Koopa : MonoBehaviour
{
    public float speed = 1.7F;
    private int lives = 3;
	private bool isActive = false;
	private const float necessary_distance_for_activation = 15f;

	private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector3 direction;
	private GameObject Player;

	private KoopaState State
    {
        get { return (KoopaState)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    private void Start()
    {
        direction = -transform.right;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
		Player = GameObject.FindGameObjectWithTag("Player");
	}

    private void Update()
    {
		if (Player == null)
			Player = GameObject.FindGameObjectWithTag("Player");
		if (isActive)
		{
			Move();

			if (lives == 3)
			{
				State = KoopaState.Moving;
			}
			else
				State = KoopaState.Stomp;
		}
		else
		{
			if (transform.position.x - Player.transform.position.x <= necessary_distance_for_activation)
				isActive = true;
		}
	}

    private void Move()
    {
        sprite.flipX = direction.x > 0.0F;
        /*
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right * direction.x * 0.5F, 0.1F);

        if (colliders.Length > 0 && colliders.All(x => !x.GetComponent<Movement>()))
            direction *= -1.0F;
        */
        if (lives == 2 || lives == 0)
            speed = 0;

        if (lives == 1)
            speed = 6;

        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.gameObject.tag == "Block" && col.transform.position.y > gameObject.transform.position.y) || (col.gameObject.tag == "Enemy" && lives != 1))
            direction *= -1.0F;

        if (col.gameObject.tag == "Player")
        {
            if (Mathf.Abs(col.gameObject.transform.position.x - transform.position.x) < 0.4F)
            {
                lives--;

                if(lives == 1)
                {
                    gameObject.tag = "Bullet";
                    if (col.gameObject.transform.position.x < gameObject.transform.position.x)
                        direction = transform.right;
                }

                if (lives == 0)
                {
                    speed = 0;
                    GetComponent<BoxCollider2D>().enabled = false;
                    rb.gravityScale = 0;
                    Destroy(gameObject, 0.4F);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Destroy")
            Destroy(gameObject, 0.5F);
    }
}


public enum KoopaState
{
    Moving,
    Stomp
}
