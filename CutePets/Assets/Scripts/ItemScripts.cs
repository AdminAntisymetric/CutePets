using UnityEngine;
using System.Collections;

public class ItemScripts : MonoBehaviour {
	private GameObject[] enemyList;
	private GameObject playerRef;
	private float newSpeed;
	public int timeEffect = 10;
	public int damage = 50;
	public int healthUp = 25;
	public int speedFactor = 2; 

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		enemyList = GameObject.FindGameObjectsWithTag ("Enemy");
		playerRef = GameObject.FindGameObjectWithTag ("Player");
	}

	public void ClockAction(){
		//Todos los enemigos que aparezcan durante TimeEffect reducen su velocidad
		Debug.Log ("Usamos logica del reloj");
		foreach (GameObject enemyData in enemyList) {
			enemyData.GetComponent<EnemyAI> ().isSlowed = true;
			enemyData.GetComponent<EnemyAI> ().speedFactor = speedFactor;
			enemyData.GetComponent<EnemyAI> ().slowCounter = timeEffect;
		}
	}
	public void BombAction(){
		//Todos los enemigos presentes en pantalla reciben Damage como daño
		Debug.Log ("Usamos logica de la bomba");
		foreach (GameObject enemyData in enemyList) {
			enemyData.GetComponent<Enemy>().DamageEnemy(damage, true);
			Instantiate(enemyData.GetComponent<Enemy>().deathParticles,enemyData.GetComponent<Enemy>().transform.position, enemyData.GetComponent<Enemy>().transform.rotation);
		}
	}
	public void CandyAction(){
		//Durante TimeEffect, todos los enemigos que aparezcan hacen 0 daño
		foreach (GameObject enemyData in enemyList) {
			enemyData.GetComponent<Enemy> ().isWeakened = true;
			enemyData.GetComponent<Enemy> ().weakCounter = timeEffect;
		}
	}
	public void HealthKitAction(){
		//Recupera HealthUp de vida el jugador
		playerRef.GetComponent<Player> ().playerStats.Health += healthUp;
	}
}
