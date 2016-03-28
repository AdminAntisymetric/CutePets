using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour {
	public int WavesToSpawn = 5;
	public enum SpawnState { SPAWNING, WAITING, COUNTING };
	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}
	
	public Wave[] waves;
	private int nextWave = 0;
	public int NextWave
	{
		get { return nextWave + 1; }
	}
	
	public Transform[] spawnPoints;
	
	public float timeBetweenWaves = 5f;
	private float waveCountdown;
	public float WaveCountdown
	{
		get { return waveCountdown; }
	}
	
	private float searchCountdown = 1f;
	
	private SpawnState state = SpawnState.COUNTING;
	public SpawnState State
	{
		get { return state; }
	}
	private GameObject NormalSpawnCount;
	private bool yaAparecio=false;
	/*Si no ha aparecido el jefe, es false; si ya aparecio es true
	El jefe aparece una sola vez por wave y la wave no debe reiniciar.*/
	
	void Start()
	{
		if (spawnPoints.Length == 0){
			Debug.LogError("No spawn points referenced.");
		}
		NormalSpawnCount = GameObject.FindWithTag("EnemySpawn");
		waveCountdown = timeBetweenWaves;
	}
	
	void Update()
	{
		if (state == SpawnState.WAITING){
			if (!EnemyIsAlive()){
				WaveCompleted();
			}
			else{
				return;
			}
		}
		if (waveCountdown <= 0){
			if (state != SpawnState.SPAWNING){
				StartCoroutine( SpawnWave ( waves[nextWave] ) );
			}
		}
		else{
			waveCountdown -= Time.deltaTime;
		}
	}
	void WaveCompleted()
	{
		Debug.Log("Wave Completed!");
		
		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;
		
		if (nextWave + 1 > waves.Length - 1){
			nextWave = 0;
			Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else{
			nextWave++;
		}
	}
	
	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f){
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Boss") == null){
				return false;
			}
		}
		return true;
	}
	IEnumerator SpawnWave(Wave _wave){
		Debug.Log("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;
		if (NormalSpawnCount.GetComponent<WaveSpawner> ().totalwaves % 5 == 0) {
			SpawnEnemy (_wave.enemy);
			yield return new WaitForSeconds( 1f/_wave.rate);
		}
		state = SpawnState.WAITING;
		yield break;
	}
	void SpawnEnemy(Transform _enemy){
		Debug.Log("Spawning Enemy: " + _enemy.name);
		
		Transform _sp = spawnPoints[0];
		Instantiate(_enemy, _sp.position, _sp.rotation);
	}
}