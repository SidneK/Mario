using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Goomba : MonoBehaviour
{
    public float speed = 2.0F;
    private bool isDead = false;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector3 direction;
    
    private GoombaState State
    {
        get { return (GoombaState)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        direction = -transform.right;
    }

    private void Update()
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(col.gameObject.name);

        if ((col.gameObject.tag == "Block" && col.transform.position.y > gameObject.transform.position.y) || col.gameObject.tag == "Enemy")
            direction *= -1.0F;

        if (col.gameObject.tag == "Player")
        {
            if (Mathf.Abs(col.gameObject.transform.position.x - transform.position.x) < 0.5F)
            {
                isDead = true;
                GetComponent<BoxCollider2D>().enabled = false;
                rb.gravityScale = 0;
                rb.velocity = Vector3.zero;
                Destroy(gameObject, 0.4F);
            }
        }
        else if (col.gameObject.tag == "Bullet")
            InstantKill();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Destroy")
            Destroy(gameObject, 0.5F);
    }

    private void InstantKill()
    {
        Quaternion rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
        rb.velocity = Vector3.zero;
        gameObject.transform.rotation = rotation;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}


public enum GoombaState
{
    Moving,
    Stomp
}
