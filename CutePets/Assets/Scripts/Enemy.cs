using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	[System.Serializable]
	public class EnemyStats {
		public int maxHealth = 100;
		public int score = 150;
		private int _curHealth;
		public int curHealth
		{
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp (value, 0, maxHealth); }
		}
		
		public int damage = 40;
		
		public void Init()
		{
			curHealth = maxHealth;
		}
	}
	
	public EnemyStats stats = new EnemyStats();
	
	public Transform deathParticles;
	
	public float shakeAmt = 0.1f;
	public float shakeLength = 0.1f;
	private bool wasKilled = false;

	public bool isWeakened = false;
	public int weakCounter = 0, refDamage;

	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;
	
	void Start()
	{
		refDamage = stats.damage;
		stats.Init();
		
		if (statusIndicator != null)
		{
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
		
		if (deathParticles == null)
		{
			Debug.LogError("No death particles referenced on Enemy");
		}
	}

	void Update(){
		if (isWeakened) {
			stats.damage = 0;
			if (weakCounter > 0) {
				weakCounter--;
			} else {
				isWeakened = false;
			}
		} else {
			stats.damage = refDamage;
		}
	}
	
	public void DamageEnemy (int damage, bool wasHit) {
		stats.curHealth -= damage;
		if (stats.curHealth <= 0)
		{
			GameMaster.KillEnemy (this.gameObject, wasHit);
		}
		
		if (statusIndicator != null)
		{
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
	}
	
	void OnCollisionEnter2D(Collision2D _colInfo)
	{
		Player _player = _colInfo.collider.GetComponent<Player>();
		if (_player != null) {
			_player.DamagePlayer (stats.damage);
			DamageEnemy (9999999, false);
		} else if(_colInfo.gameObject.tag == "Floor"){
			DamageEnemy (9999999, false);
		}
	}
}
