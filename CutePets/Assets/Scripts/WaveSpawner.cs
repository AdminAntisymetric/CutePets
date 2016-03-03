using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {
	private float randomenemy;
	public int totalwaves = 1;
	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public Transform enemy2;
		public Transform enemy3;
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

	void Start()
	{
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("No spawn points referenced.");
		}

		waveCountdown = timeBetweenWaves;
	}

	void Update()
	{
		if (state == SpawnState.WAITING)
		{
			if (!EnemyIsAlive())
			{
				WaveCompleted();
			}
			else
			{
				return;
			}
		}
		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING)
			{
				StartCoroutine( SpawnWave ( waves[nextWave] ) );
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
		GameObject waveText = GameObject.Find("GameMaster");
		waveText.GetComponent<GameMaster> ().wavetext.text = "Wave #" + totalwaves.ToString();
	}

	void WaveCompleted()
	{
		Debug.Log("Wave Completed!");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1)
		{
			nextWave = 0;
			Debug.Log("ALL WAVES COMPLETE! Looping...");
		}
		else
		{
			nextWave++;
		}
		totalwaves++;
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f)
		{
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Enemy") == null)
			{
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		Debug.Log("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;

		/*if (totalwaves % 5 == 0 && totalwaves>10) {
			_wave.enemy.GetComponent<Enemy>().stats.damage += 5;
			_wave.enemy.GetComponent<Enemy>().stats.score += 10;
			_wave.enemy.GetComponent<Enemy>().stats.maxHealth += 15;

			_wave.enemy2.GetComponent<Enemy>().stats.damage += 5;
			_wave.enemy2.GetComponent<Enemy>().stats.score += 10;
			_wave.enemy2.GetComponent<Enemy>().stats.maxHealth += 15;

			_wave.enemy3.GetComponent<Enemy>().stats.damage += 5;
			_wave.enemy3.GetComponent<Enemy>().stats.score += 10;
			_wave.enemy3.GetComponent<Enemy>().stats.maxHealth += 15;
		}*/

		for (int i = 0; i < _wave.count; i++) {
			//SpawnEnemy(_wave.enemy);
			randomenemy = Random.Range (0, 60);
			if (randomenemy <= 36)
				SpawnEnemy (_wave.enemy);
			if (randomenemy >= 37 && randomenemy <= 51)
				SpawnEnemy (_wave.enemy2);
			if (randomenemy >= 52 && randomenemy <= 60)
				SpawnEnemy (_wave.enemy3);
			//yield return new WaitForSeconds( 1f/_wave.rate );
			yield return new WaitForSeconds( 1f/(Random.Range(1,_wave.rate)) );
		}
		state = SpawnState.WAITING;
		yield break;
	}

	void SpawnEnemy(Transform _enemy)
	{
		Debug.Log("Spawning Enemy: " + _enemy.name);

		Transform _sp = spawnPoints[ Random.Range (0, spawnPoints.Length) ];
		Instantiate(_enemy, _sp.position, _sp.rotation);
	}
}
