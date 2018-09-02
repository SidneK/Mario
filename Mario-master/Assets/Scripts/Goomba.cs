using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Goomba : MonoBehaviour
{
    public float speed = 2.0F;
    private bool isDead = false;
	private bool isActive = false;
	private const float distanceToActivate = 15f;

	private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector3 direction;
	private GameObject Player;
    
    private GoombaState State
    {
        get { return (GoombaState)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        direction = -transform.right;
    }

    private void Update()
    {
		if (isActive)
		{
			State = GoombaState.Moving;

			if (!isDead)
			{
				rb.velocity = direction * speed;
				State = GoombaState.Moving;
			}
			else
				State = GoombaState.Stomp;
		}
		else
		{
            if (Logic.Instance.Player == null)
                Logic.Instance.Player = GameObject.FindGameObjectWithTag("Player");
            else
                if (transform.position.x - Logic.Instance.Player.transform.position.x <= distanceToActivate)

			if (Logic.Instance.Player == null)
				Logic.Instance.Player = GameObject.FindGameObjectWithTag("Player");
			else
				if (transform.position.x - Logic.Instance.Player.transform.position.x <= distanceToActivate)
					isActive = true;
		}
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isActive)
        {
            if ((col.gameObject.tag == "Block" && col.transform.position.y > gameObject.transform.position.y) || col.gameObject.tag == "Enemy")
                direction *= -1.0F;

            if (col.gameObject.tag == "Player")
            {
                if (Mathf.Abs(col.gameObject.transform.position.x - transform.position.x) < 0.5F)
                {
                    Logic.Instance.KillEnemy.Play();
                    isDead = true;
                    GetComponent<BoxCollider2D>().enabled = false;
                    rb.gravityScale = 0;
                    rb.velocity = Vector3.zero;
                    Destroy(gameObject, 0.4F);
                }
            }
            else if (col.gameObject.tag == "Bullet" || col.gameObject.tag == "Drift")
            {
                sprite.flipY = true;
                GetComponent<BoxCollider2D>().enabled = false;
            }

            else if (col.gameObject.tag == "Bullet")
            {
                sprite.flipY = true;
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Destroy")
            Destroy(gameObject, 0.5F);
    }
}


public enum GoombaState
{
    Moving,
    Stomp
}
