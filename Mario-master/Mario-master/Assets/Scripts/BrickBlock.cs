using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBlock : MonoBehaviour
{
    public bool hasCoin = false;
    public float pushForce;

    private Transform body_block;
    private SpriteRenderer sprite;
    private float basic_position;
    private bool pushed;
    private bool used;
    private float pushing_timer;
    private float pushing_time;
    
    public Sprite usedBlock;

    private void Start()
    {
        body_block = GetComponent<Transform>();
        basic_position = body_block.position.y;
        pushed = false;
        pushing_timer = 0;
        pushing_time = 0.15f;
    }

    private void FixedUpdate()
    {
        if(pushed)
        {
            Push(pushForce);
            pushing_timer += Time.fixedDeltaTime;
            if(pushing_timer >= pushing_time)
            {
                pushing_timer = 0;
                pushed = false;
                /*
                if (hasCoin)
                    sprite.sprite = usedBlock;
                else
                */
                //Destroy(gameObject);
            }
            if (used && body_block.position.y > basic_position)
                Push(pushForce, -1);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!used && col.collider.transform.position.y < transform.position.y && col.collider.tag == "Player")
        {
            pushed = true;
            Debug.Log("Got a coin");
            //Destroy(gameObject);
        }
    }

    private void Push(float speed, int direction = 1)
    {
        if (direction == 1 || direction == -1)
            body_block.Translate(new Vector3(0, speed * direction * Time.fixedDeltaTime, 0));
    }
}
