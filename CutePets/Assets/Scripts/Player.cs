using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	[System.Serializable]
	public class PlayerStats {
		public int Health = 100;
		public float FireRate = 5.0f;
		public int Damage = 30;
	}
	public GameObject weapon;
	public PlayerStats playerStats = new PlayerStats();
	
	public int fallBoundary = -20;

	void Start(){
		GameObject weaponinstance = Instantiate(weapon, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity ) as GameObject;
		weaponinstance.transform.parent = gameObject.transform;
	}

	void Update () {
		if (transform.position.y <= fallBoundary)
			DamagePlayer (9999999);
	}
	
	public void DamagePlayer (int damage) {
		playerStats.Health -= damage;
		if (playerStats.Health <= 0) {
			GameMaster.KillPlayer(this);
			playerStats.Health = 100;
		}
	}
	
}

