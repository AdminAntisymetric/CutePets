﻿using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour {
	public int moveSpeed = 230;
	private int damage;
	private GameObject playerData;
	void Start(){
		playerData = GameObject.FindGameObjectWithTag ("Player");
		damage = playerData.GetComponent<Player> ().playerStats.Damage;
	}
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
	}
	void OnCollisionEnter2D (Collision2D other){
		//Debug.Log (other.gameObject.name);
<<<<<<< HEAD
		Destroy (this.gameObject);
		if(other.gameObject.layer == 8)
			other.gameObject.GetComponent<Enemy> ().DamageEnemy (damage, true);
=======
		if (other.gameObject.tag == "Enemy") {
			Destroy (this.gameObject);
			other.gameObject.GetComponent<Enemy> ().DamageEnemy (damage, true);
		} else {
			Debug.Log ("No es enemigo");
		}
>>>>>>> origin/master
	}
}
