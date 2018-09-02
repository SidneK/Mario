using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logic : MonoBehaviour
{
	public AudioSource BackgroundMain;
	public AudioSource BackgroundSecret;
	public AudioSource JumpLittle;
	public AudioSource JumpSupper;
	public AudioSource MarioDie;
	public AudioSource MarioUp;
	public AudioSource MarioShoot;
	public AudioSource FireballExplosion;
	public AudioSource BonusUp;
	public AudioSource UnEnableBlock;
	public AudioSource BreakBlock;
	public AudioSource Coin;
	public AudioSource PipeAndDownLevel;
	public AudioSource Pause;
	public AudioSource StageClear;
	public AudioSource Flagpole;
	public AudioSource KillEnemy;
	public GameObject Mushroom;
	public GameObject Flower;
	public GameObject MarioLittle;
	public GameObject MarioBig;
	public GameObject MarioFire;
	public GameObject CoinFromBlock;
	public Text PrintPause;
	public bool IsImmortalPlayer;
	public bool StayPlayer;
	public bool IsPause
	{
		get
		{
			return isPause;
		}
		set
		{
			isPause = value;
			Time.timeScale = isPause ? 0 : 1; // if pause, then the game must stop
			PrintPause.enabled = isPause;
			if (isPause)
			{
				StageClear.Stop();
				BackgroundMain.Stop();
				BackgroundSecret.Stop();
				Pause.Play();
			}
			else
			{
				if (TurnCamera.Instance.MainWorld.enabled)
					BackgroundMain.Play();
				else
					BackgroundSecret.Play();
			}
		}
	}
	public bool IsGameOver;
	public Mode ModePlayer;
	public Levels CurrentLevel
	{
		set
		{
			SceneManager.LoadScene((int)value);
		}
		get
		{
			return (Levels)SceneManager.GetActiveScene().buildIndex;
		}
	}

	public GameObject Player;
	private bool isPause;

	public static Logic Instance { get; private set; }

	private void Start()
	{
		Instance = this;
		IsImmortalPlayer = false;
		StayPlayer = false;
		IsGameOver = false;
		isPause = false;
		Player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (Player == null)
			Player = GameObject.FindGameObjectWithTag("Player");
	}

	public void TransitToFirstLevel()
	{
		TransitToLevel(Levels.WORLD_1_1);
	}

	public void TransitToCurrentLevel()
	{
		CurrentLevel = CurrentLevel;
	}

	public void TransitToNextLevel()
	{
		int current_level = (int)CurrentLevel;
		++current_level;
		CurrentLevel = (Levels)current_level;
	}

	public void TransitToLevel(Levels level)
	{
		CurrentLevel = level;
	}

	public void SetInActiveUI() // need at the time a animation
	{
		GameObject Left = GameObject.FindGameObjectWithTag("ButtonLeft");
		Left.GetComponent<MoveButton>().enabled = false;
		GameObject Right = GameObject.FindGameObjectWithTag("ButtonRight");
		Right.GetComponent<MoveButton>().enabled = false;
		GameObject Space = GameObject.FindGameObjectWithTag("ButtonJump");
		Space.GetComponent<SpaceButton>().enabled = false;
		GameObject Fire = GameObject.FindGameObjectWithTag("ButtonAttack");
		Fire.GetComponent<FireButton>().enabled = false;
		GameObject Down = GameObject.FindGameObjectWithTag("ButtonDown");
		Down.GetComponent<DownButton>().enabled = false;
	}

	public void SetActiveUI()
	{
		GameObject Left = GameObject.FindGameObjectWithTag("ButtonLeft");
		Left.GetComponent<MoveButton>().enabled = true;
		GameObject Right = GameObject.FindGameObjectWithTag("ButtonRight");
		Right.GetComponent<MoveButton>().enabled = true;
		GameObject Space = GameObject.FindGameObjectWithTag("ButtonJump");
		Space.GetComponent<SpaceButton>().enabled = true;
		GameObject Fire = GameObject.FindGameObjectWithTag("ButtonAttack");
		Fire.GetComponent<FireButton>().enabled = true;
		GameObject Down = GameObject.FindGameObjectWithTag("ButtonDown");
		Down.GetComponent<DownButton>().enabled = true;
	}
}

public enum Mode
{
	LITTLE = 0,
	BIG = 1,
	FIRE = 2
};

public enum Levels
{
	WORLD_1_1 = 0,
	WORLD_1_2 = 1
};
