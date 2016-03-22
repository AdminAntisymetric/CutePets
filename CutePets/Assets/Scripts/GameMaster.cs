using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
	
	public static GameMaster gm;
	public int TotalScore = 0;
	public int maxLives = 3;
	public static int _remainingLives;
	public static int RemainingLives 
	{
		get {return _remainingLives;}
	}

	void Awake () {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
		TotalScore = 0;
	}

	//public Transform playerPrefab;
	public GameObject playerPrefab;
	public Transform spawnPoint;
	public float spawnDelay = 2;
	public Transform spawnPrefab;
	
	public CameraShake cameraShake;

	public GameObject GameOverScreen;
	private GameObject waveCounter;
	public Text wavetext, scoretext;

	private GameObject[] enemyList;
	private GameObject playerRef;

	GameObject bombFX, clockFX;
	Color bombRef, clockRef;

	public bool bombActivated=false, clockActivated=false;
	public float timer, slowCounter;

	public bool gameOver = false;

	void Start(){
		if(GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataTransfer>().characterRef != null)
			playerPrefab = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataTransfer>().characterRef;
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);

		if (cameraShake == null){
			Debug.LogError("No camera shake referenced in GameMaster");
		}
		waveCounter = GameObject.Find ("SpawnPoints");
		_remainingLives = maxLives;
		wavetext.text = "Wave #1";
		scoretext.text = "0 pts";

		timer = 0;
		slowCounter = 0;

		bombFX = GameObject.Find ("BombEffect");
		clockFX = GameObject.Find ("ClockEffect");
		bombRef = bombFX.GetComponent<SpriteRenderer> ().color;
		clockRef = clockFX.GetComponent<SpriteRenderer> ().color;
		bombFX.GetComponent<SpriteRenderer> ().color = new Color (bombRef.r,bombRef.g,bombRef.b,0.0f);
		clockFX.GetComponent<SpriteRenderer> ().color = new Color (clockRef.r,clockRef.g,clockRef.b,0.0f);
	}
	
	public void EndGame (){
		GameOverScreen.SetActive(true);
		gameOver = true;
		GameObject.Find ("PauseButton").gameObject.GetComponent<Button> ().interactable = false;
		GameObject.Find ("ScoreText").gameObject.GetComponent<Text> ().text = TotalScore.ToString ();
		if (TotalScore > GameObject.FindGameObjectWithTag ("DataManager").GetComponent<DataTransfer> ().highscore) {
			GameObject.FindGameObjectWithTag ("DataManager").GetComponent<DataTransfer> ().highscore = TotalScore;
			GameObject.FindGameObjectWithTag ("DataManager").GetComponent<DataTransfer> ().saveScore = true;
		}
	}

	public void Update(){
		enemyList = GameObject.FindGameObjectsWithTag ("Enemy");
		playerRef = GameObject.FindGameObjectWithTag ("Player");

		if (bombActivated)
			StartCoroutine (BombEffect ());
		else {
			bombFX.GetComponent<SpriteRenderer> ().color = new Color (bombRef.r, bombRef.g, bombRef.b, 0.0f);
		}

		if (clockActivated) {
			timer += Time.deltaTime;
			if (timer <= slowCounter)
				StartCoroutine (ClockEffect (2));
			else
				clockActivated = false;
		} else {
			timer = 0;
			clockFX.GetComponent<SpriteRenderer> ().color = new Color (clockRef.r, clockRef.g, clockRef.b, 0.0f);
		}
}

	public IEnumerator _RespawnPlayer () {
		GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds (spawnDelay);
		Debug.Log (playerPrefab + " " + spawnPoint.position + " " + spawnPoint.rotation);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
	}
	
	public static void KillPlayer (Player player) {
		Destroy (player.gameObject);
		_remainingLives -= 1;
		if (_remainingLives < 1){
			gm.EndGame();
		} else{
			gm.Respawn();
		}
	}
	
	public static void KillEnemy (GameObject enemy, bool destroyedByUs) {
		gm._KillEnemy(enemy, destroyedByUs);		//recibimos los datos en nuestro manager
	}
	public void _KillEnemy(GameObject _enemy, bool _destroyedbyus){
		//primero obtenemos la variable de 'points' para sumarla a nuestro marcador global.
		if (_destroyedbyus) {
			TotalScore += _enemy.GetComponent<Enemy> ().stats.score;
			//cambiamos el texto de nuestra GUI para que muestre el valor numerico como texto
			scoretext.text = TotalScore.ToString () + " pts";
		}
		//hacemos referencia por medio de GetComponent para manipular y obtener
		GameObject _clone = Instantiate (_enemy.GetComponent<Enemy>().deathParticles, _enemy.transform.position, Quaternion.identity) as GameObject;
		/*Destruimos al enemigo; para el efecto de explosion use un script extra que se llama Destroy;
		 Destroy(_clone, 0.1f)*/
		Destroy (_enemy);
	}
	public void Respawn(){
		Debug.Log (playerPrefab + " " + spawnPoint.position + " " + spawnPoint.rotation);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
	}

	IEnumerator BombEffect(){
		bombFX.GetComponent<SpriteRenderer> ().color = new Color (bombRef.r, bombRef.g, bombRef.b, 0.1f);
		yield return new WaitForSeconds (.1f);
		bombFX.GetComponent<SpriteRenderer> ().color = new Color (bombRef.r, bombRef.g, bombRef.b, 0.5f);
		yield return new WaitForSeconds (.1f);
		bombActivated = false;
	}
	IEnumerator ClockEffect(int factor){
		clockFX.GetComponent<SpriteRenderer> ().color = new Color (clockRef.r, clockRef.g, clockRef.b, 0.5f);
		yield return new WaitForSeconds (.1f);
	}
}
