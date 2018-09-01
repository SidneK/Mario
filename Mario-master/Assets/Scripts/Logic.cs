using UnityEngine;

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
	public GameObject Mushroom;
	public GameObject Flower;
	public GameObject MarioLittle;
	public GameObject MarioBig;
	public GameObject MarioFire;
	public GameObject CoinFromBlock;
	public bool IsImmortalPlayer;
	public bool StayPlayer;
	public bool IsPause;
	public bool IsGameOver;
	public Mode ModePlayer;

	public static Logic Instance { get; private set; }

	private void Start()
	{
		Instance = this;
		IsImmortalPlayer = false;
		StayPlayer = false;
		IsGameOver = false;
		IsPause = false;
	}

}

public enum Mode
{
	LITTLE = 0,
	BIG = 1,
	FIRE = 2
};
