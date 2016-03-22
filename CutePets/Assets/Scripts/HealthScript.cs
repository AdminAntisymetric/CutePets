using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	private SpriteRenderer healthBar;
	private Vector3 healthScale;
	public int curHealth;
	Player playerData;

	// Use this for initialization
	void Start () {
		playerData = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		healthScale = healthBar.transform.localScale;
		curHealth = playerData.playerStats.Health;
	}
	
	// Update is called once per frame
	void Update () {
		curHealth = playerData.playerStats.Health;
		UpdateHealthBar();
	}
	public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - playerData.playerStats.Health * 0.01f);	
		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(healthScale.x * playerData.playerStats.Health * 0.01f, 0.5f, 1);
	}
}
