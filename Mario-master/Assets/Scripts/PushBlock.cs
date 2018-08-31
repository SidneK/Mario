using UnityEngine;

public class PushBlock : MonoBehaviour
{
	public Sprite UnEnableBlock;
	public float PushForce;

	private GameObject bonus;
	private Transform body_block;
	private float basic_position;
	private bool isUsed;
	private bool isPush;
	private float pushing_timer;
	private float pushing_time;

	private void Start()
	{
		body_block = GetComponent<Transform>();
		basic_position = body_block.position.y;
		isUsed = false;
		isPush = false;
		pushing_timer = 0;
		pushing_time = 0.15f;
	}

	private void FixedUpdate()
	{
		if (isPush)
		{
			Push(body_block, PushForce);
			if (gameObject.tag == "BonusBlock")
				Push(bonus.transform, PushForce - 0.5f);
			pushing_timer += Time.fixedDeltaTime;
			if (pushing_timer >= pushing_time)
			{
				if (gameObject.tag == "BonusBlock")
					bonus.GetComponent<SpriteRenderer>().enabled = true;
				pushing_timer = 0;
				isPush = false;
				isUsed = true;
			}
		}
		if (isUsed && body_block.position.y > basic_position)
		{
			Push(body_block, PushForce, -1);
			if (gameObject.tag == "BonusBlock")
				Push(bonus.transform, PushForce);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!isUsed)
		{
			if (collision.gameObject.tag == "Player" && collision.transform.position.y < body_block.position.y)
			{
				isPush = true;
				InstantiateBonus();
			}
		}
		else
		{
			if (collision.gameObject.tag == "Player" && collision.transform.position.y < body_block.position.y)
				Logic.Instance.UnEnableBlock.Play();
		}
	}

	private void Push(Transform push_object, float speed, int direction = 1) // direction only up or down and must have 1 or -1
	{
		if (direction == 1 || direction == -1)
			push_object.Translate(new Vector3(0, speed * direction * Time.fixedDeltaTime, 0));
	}

	private void InstantiateBonus()
	{
		if (gameObject.tag == "BonusBlock")
		{
			switch (Logic.Instance.ModePlayer)
			{
				case Mode.LITTLE:
					bonus = Instantiate(Logic.Instance.Mushroom, body_block.position, Quaternion.identity);
					FreezeComponents();
					Invoke("UnFreezeComponents", 0.3f);
					break;
				default:
					bonus = Instantiate(Logic.Instance.Flower, body_block.position, Quaternion.identity);
					FreezeComponents();
					Invoke("UnFreezeComponents", 0.3f);
					break;
			}
			Logic.Instance.BonusUp.Play();
			SetUnEnableBlock();
		}
		else if (gameObject.tag == "CoinBlock")
		{
			Logic.Instance.Coin.Play();
			SetUnEnableBlock();
		}
		else if (gameObject.tag == "Block" && Logic.Instance.ModePlayer != Mode.LITTLE)
		{
			Destroy(gameObject);
			Logic.Instance.BreakBlock.Play();
		}
		else if (gameObject.tag == "Block")
		{
			Invoke("UnUse", 0.5f);
			Logic.Instance.UnEnableBlock.Play();
		}
	}

	private void FreezeComponents()
	{
		bonus.tag = "Untagged";
		bonus.transform.position = new Vector3(bonus.transform.position.x, bonus.transform.position.y, 1);
		bonus.GetComponent<SpriteRenderer>().enabled = false;
		if (Logic.Instance.ModePlayer == Mode.LITTLE)
		{
			bonus.GetComponent<MovementMushroom>().enabled = false;
			bonus.GetComponent<Rigidbody2D>().gravityScale = 0;
		}
		bonus.GetComponent<CircleCollider2D>().enabled = false;
	}

	private void UnFreezeComponents()
	{
		bonus.tag = "Bonus";
		bonus.transform.position = new Vector3(bonus.transform.position.x, bonus.transform.position.y, 0);
		if (Logic.Instance.ModePlayer == Mode.LITTLE)
		{
			bonus.GetComponent<MovementMushroom>().enabled = true;
			bonus.GetComponent<Rigidbody2D>().gravityScale = 1;
		}
		bonus.GetComponent<CircleCollider2D>().enabled = true;
	}

	private void UnUse()
	{
		isUsed = false;
	}

	private void SetUnEnableBlock()
	{
		GetComponent<Animator>().enabled = false;               // turn off animation to on sprite 
		GetComponent<SpriteRenderer>().sprite = UnEnableBlock;
	}
}
