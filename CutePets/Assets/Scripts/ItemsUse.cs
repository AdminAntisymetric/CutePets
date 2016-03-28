using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemsUse : MonoBehaviour {
	public GameObject bombData;
	public GameObject clockData;
	public GameObject healthkitData;
	public GameObject candyData;
	private GameObject[] enemyList;
	private GameObject playerRef;
	Button bombButton, candyButton, clockButton, healthkitButton;

	int damage, healthUp, speedFactor;
	float timeEffect, invincibleEffect;
	// Use this for initialization
	void Start () {
		bombButton = GameObject.Find ("BombButton").GetComponent<Button>();
		candyButton = GameObject.Find ("CandyButton").GetComponent<Button>();
		clockButton = GameObject.Find ("ClockButton").GetComponent<Button>();
		healthkitButton = GameObject.Find ("HealthkitButton").GetComponent<Button>();

		damage = bombData.GetComponent<ItemScripts> ().damage;
		healthUp = healthkitData.GetComponent<ItemScripts> ().healthUp;
		timeEffect = clockData.GetComponent<ItemScripts> ().timeEffect;
		speedFactor = clockData.GetComponent<ItemScripts> ().speedFactor;
		invincibleEffect = candyData.GetComponent<ItemScripts> ().timeEffect;
	}
	
	// Update is called once per frame
	void Update () {
		enemyList = GameObject.FindGameObjectsWithTag ("Enemy");
		playerRef = GameObject.FindGameObjectWithTag ("Player");
	}

	public void BombAction(){
		//Todos los enemigos presentes en pantalla reciben Damage como daño
		foreach (GameObject enemyData in enemyList) {
			enemyData.GetComponent<Enemy>().DamageEnemy(damage, true);
			Instantiate(enemyData.GetComponent<Enemy>().deathParticles,enemyData.GetComponent<Enemy>().transform.position, enemyData.GetComponent<Enemy>().transform.rotation);
		}
		bombButton.interactable = false;
		GameObject gm = GameObject.FindGameObjectWithTag ("GM");
		gm.GetComponent<GameMaster> ().bombActivated = true;
	}
	public void CandyAction(){
		//Durante TimeEffect, todos los enemigos que aparezcan hacen 0 daño
		foreach (GameObject enemyData in enemyList) {
			enemyData.GetComponent<Enemy> ().isWeakened = true;
			enemyData.GetComponent<Enemy> ().weakCounter = invincibleEffect;
		}
		playerRef.GetComponent<Player> ().isInvincible = true;
		playerRef.GetComponent<Player> ().invincibleCounter = invincibleEffect;
		candyButton.interactable = false;
	}
	public void HealthKitAction(){
		//Recupera HealthUp de vida el jugador
		playerRef.GetComponent<Player> ().playerStats.Health += healthUp;
		healthkitButton.interactable = false;
	}
	public void ClockAction(){
		//Todos los enemigos que aparezcan durante TimeEffect reducen su velocidad
		foreach (GameObject enemyData in enemyList) {
			enemyData.GetComponent<EnemyAI> ().isSlowed = true;
			enemyData.GetComponent<EnemyAI> ().speedFactor = speedFactor;
			enemyData.GetComponent<EnemyAI> ().slowCounter = timeEffect;
		}
		GameObject gm = GameObject.FindGameObjectWithTag ("GM");
		gm.GetComponent<GameMaster> ().clockActivated = true;
		gm.GetComponent<GameMaster> ().slowCounter = timeEffect;
		clockButton.interactable = false;
	}
}