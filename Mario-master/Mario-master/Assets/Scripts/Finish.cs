using UnityEngine;

public class Finish : MonoBehaviour
{
	private Transform flag;
	private Transform block;

	private bool isFlagMove = false;
	private bool isFlagDown = false;
	private const float speed_flag = 4f;
	private const float speed_player = 3f;

	private void Start()
	{
		flag = transform.GetChild(0); // it's need to get the reference on the flag
		block = transform.GetChild(1); // it's need for that, to know when we are must stop the flag
	}

	private void Update()
	{
		if (isFlagDown)
			MovePlayer();
		if (isFlagMove)
			MoveDownFlag();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			isFlagMove = true;
			GetComponent<BoxCollider2D>().enabled = false; // need for that, to run by Mario through stick
			Logic.Instance.Flagpole.Play();
			Logic.Instance.BackgroundMain.Stop();
			Logic.Instance.Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			Logic.Instance.SetInActiveUI();
			Logic.Instance.StageClear.Play();
		}
	}

	private void MoveDownFlag()
	{
		flag.Translate(new Vector3(0, -speed_flag * Time.deltaTime, 0));
		if (flag.position.y - block.position.y <= 0.7f) // where 0.7 is interval between block and flag
		{
			isFlagDown = true;
			isFlagMove = false;
			Logic.Instance.Player.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePosition;
			TurnCamera.Instance.SetInActiveCamera();
		}
		else
		{
			if (Logic.Instance.Player.transform.position.y - block.position.y > 0.7f)
				FallDownPlayer();
			else
				Logic.Instance.Player.GetComponent<Animator>().SetInteger("State", (int)State.DRAW_FLAG);
		}
	}

	private void MovePlayer()
	{
		Logic.Instance.Player.GetComponent<Animator>().SetInteger("State", (int)State.RUN);
		Logic.Instance.Player.transform.Translate(new Vector3(speed_player * Time.deltaTime, 0, 0));
	}

	private void FallDownPlayer()
	{
		Logic.Instance.Player.GetComponent<Animator>().SetInteger("State", (int)State.FALL_DOWN);
		Logic.Instance.Player.transform.Translate(new Vector3(0, -speed_flag * Time.deltaTime, 0));
	}
}
