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


	public Transform playerPrefab;
	public Transform spawnPoint;
	public float spawnDelay = 2;
	public Transform spawnPrefab;
	
	public CameraShake cameraShake;

	public GameObject GameOverScreen;
	private GameObject waveCounter;
	public Text wavetext, scoretext;

	void Start()
	{

		if (cameraShake == null)
		{
			Debug.LogError("No camera shake referenced in GameMaster");
		}
		waveCounter = GameObject.Find ("SpawnPoints");
		_remainingLives = maxLives;
		wavetext.text = "Wave #1";
		scoretext.text = "0 pts";
	}
	
	public void EndGame ()
	{
		Debug.Log("GAME OVER");
		GameOverScreen.SetActive(true);
	}

	public void Update(){
		/*if (waveCounter.GetComponent<WaveSpawner> ().totalwaves % 5 == 0 && waveCounter.GetComponent<WaveSpawner> ().totalwaves > 10) {
			playerPrefab.gameObject.GetComponent<Player>().playerStats.Health += 20;

		}*/
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
		if (_remainingLives < 1)
		{
			gm.EndGame();
		} else
		{
			gm.Respawn();
		}
	}
	
	public static void KillEnemy (GameObject enemy, bool destroyedByUs) {
		gm._KillEnemy(enemy, destroyedByUs);		//recibimos los datos en nuestro manager
	}
	public void _KillEnemy(GameObject _enemy, bool _destroyedbyus)
	{
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
}
