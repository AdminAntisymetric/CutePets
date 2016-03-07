using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	[System.Serializable]
	public class PlayerStats {
		public int Health = 100;
		public float FireRate = 5.0f;
		public int Damage = 30;
	}
	public int curHealth;
	public bool isInvincible = false;
	public float timer;
	public float invincibleCounter;
	public GameObject weapon;
	public PlayerStats playerStats = new PlayerStats();
	
	public int fallBoundary = -20;

	public float repeatDamagePeriod = 2.0f;

	private Vector3 healthScale;
	private SpriteRenderer healthBar;
	private float lastHitTime;

	public Transform weaponlocation;

	void Start(){
		GameObject weaponinstance = Instantiate(weapon, new Vector3(weaponlocation.position.x, weaponlocation.position.y, weaponlocation.position.z), Quaternion.identity ) as GameObject;
		weaponinstance.transform.parent = gameObject.transform;
		timer = 0;
		invincibleCounter = 0;
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthScale = healthBar.transform.localScale;
		curHealth = playerStats.Health;
	}

	void Update () {
		if (transform.position.y <= fallBoundary)
			DamagePlayer (9999999);

		if (playerStats.Health > 100)
			playerStats.Health = 100;

		curHealth = playerStats.Health;
		UpdateHealthBar();

		if (isInvincible) {
			timer += Time.deltaTime;
			if (timer <= invincibleCounter)
				StartCoroutine(FlashInvincible(1.0f,0.8f,0.0f,1.0f));
			else
				isInvincible = false;
		} else
			timer = 0;
	}
	
	public void DamagePlayer (int damage) {
		/*playerStats.Health -= damage;
		if (playerStats.Health <= 0) {
			GameMaster.KillPlayer(this);
			playerStats.Health = 100;
		}*/
		if (Time.time > lastHitTime + repeatDamagePeriod) {
			playerStats.Health -= damage;
			lastHitTime = Time.time;
			if(playerStats.Health <=0){
				GameMaster.KillPlayer(this);
				playerStats.Health = 100;
			}
		}
	}
	IEnumerator FlashInvincible(float red, float green, float blue, float alpha){
		Color color = new Color (red, green, blue, alpha);
		yield return new WaitForSeconds (.2f);
		GetComponent<SpriteRenderer>().color = color;
		yield return new WaitForSeconds (.2f);
		GetComponent<SpriteRenderer>().color = Color.white;
		yield return new WaitForSeconds (.2f);
	}
	public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - playerStats.Health * 0.01f);	
		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * playerStats.Health * 0.01f, 2.5f, 1);
	}
}