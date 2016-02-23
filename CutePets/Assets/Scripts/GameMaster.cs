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
	public Text scoretext;

	void Start()
	{

		if (cameraShake == null)
		{
			Debug.LogError("No camera shake referenced in GameMaster");
		}

		_remainingLives = maxLives;
		scoretext.text = "0";
	}
	
		public void EndGame ()
		{
			Debug.Log("GAME OVER");
		GameOverScreen.SetActive(true);
	}

	public IEnumerator _RespawnPlayer () {
		GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds (spawnDelay);
		
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		GameObject clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		Destroy (clone, 3f);
	}
	
	public static void KillPlayer (Player player) {
		Destroy (player.gameObject);
			_remainingLives -= 1;
			if (_remainingLives <= 0)
			{
				gm.EndGame();
			} else
			{
		gm.StartCoroutine(gm._RespawnPlayer());
			}
			}
	
	public static void KillEnemy (GameObject enemy) {
		gm._KillEnemy(enemy);		//recibimos los datos en nuestro manager
	}
	public void _KillEnemy(GameObject _enemy)
	{
		//primero obtenemos la variable de 'points' para sumarla a nuestro marcador global.
		TotalScore += _enemy.GetComponent<Enemy> ().stats.score;
		Debug.Log ("Destruido");
		//cambiamos el texto de nuestra GUI para que muestre el valor numerico como texto
		scoretext.text = TotalScore.ToString ();
		//hacemos referencia por medio de GetComponent para manipular y obtener
		GameObject _clone = Instantiate (_enemy.GetComponent<Enemy>().deathParticles, _enemy.transform.position, Quaternion.identity) as GameObject;
		/*Destruimos al enemigo; para el efecto de explosion use un script extra que se llama Destroy;
		 Destroy(_clone, 0.1f)*/
		Destroy (_enemy);
	}
}
