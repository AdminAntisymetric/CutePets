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

	[System.Serializable]
	public class ItemDrops{
		public GameObject ItemDrop;
	}
	public GameObject[] ItemAmount;

	public EnemyStats stats = new EnemyStats();
	
	public Transform deathParticles;
	
	public float shakeAmt = 0.1f;
	public float shakeLength = 0.1f;
	private bool wasKilled = false;

	//timer es un timer interno que inicia en 0 para el conteo
	//weakCounter el el valor que le daremos al timer como tiempo limite
	public bool isWeakened = false;
	public float weakCounter = 0;
	public int refDamage;
	private float timer;

	[Header("Optional: ")]
	[SerializeField]
	private StatusIndicator statusIndicator;
	
	void Start()
	{
		timer = 0;
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
			timer += Time.deltaTime;
			if(timer <= weakCounter){
				stats.damage = 0;
			}else{
				isWeakened = false;
			}
		} else {
			stats.damage = refDamage;
			timer = 0;
		}
	}
	
	public void DamageEnemy (int damage, bool wasHit) {
		Instantiate (deathParticles,this.gameObject.transform.position, this.gameObject.transform.rotation);
		stats.curHealth -= damage;
		if (stats.curHealth <= 0)
		{
			int drop = Random.Range (0,10);
			if(drop>7){
				int randomItem = Random.Range(0,10);
				//Instantiate (ItemAmount[0] as Object,this.gameObject.transform.position,this.gameObject.transform.rotation);
				if(randomItem>=0 && randomItem<4)
					Instantiate (ItemAmount[2] as Object,this.gameObject.transform.position,this.gameObject.transform.rotation);
				if(randomItem>=4 && randomItem<6)
					Instantiate (ItemAmount[3] as Object,this.gameObject.transform.position,this.gameObject.transform.rotation);
				if(randomItem>=6 && randomItem<8)
					Instantiate (ItemAmount[0] as Object,this.gameObject.transform.position,this.gameObject.transform.rotation);
				if(randomItem>=8 && randomItem<10)
					Instantiate (ItemAmount[1] as Object,this.gameObject.transform.position,this.gameObject.transform.rotation);
			}
			GameMaster.KillEnemy (this.gameObject, wasHit);
			Instantiate (deathParticles,this.gameObject.transform.position, this.gameObject.transform.rotation);
		}
		
		if (statusIndicator != null)
		{
			statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
		}
	}
	
	/*void OnCollisionEnter2D(Collision2D _colInfo)
	{
		Player _player = _colInfo.collider.GetComponent<Player>();
		if (_player != null) {
			_player.DamagePlayer (stats.damage);
			DamageEnemy (9999999, false);
		} else if(_colInfo.gameObject.tag == "Floor"){
			DamageEnemy (9999999, false);
		}
	}*/
	void OnTriggerEnter2D(Collider2D other){
		Player _player = other.GetComponent<Player> ();
		if (other.name == "Floor" || other.name == "Boundary")
			DamageEnemy (9999999, false);
		if (other.tag == "Player") {
			_player.DamagePlayer (stats.damage);
			DamageEnemy (9999999, false);
		}
		if (other.tag == "Bullet") {
			Destroy (other.gameObject);
			Instantiate(deathParticles, other.transform.position, other.transform.rotation);
			DamageEnemy (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().playerStats.Damage, true);
		}
	}
}