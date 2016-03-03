using UnityEngine;
using System.Collections;

public class WeaponTouch : MonoBehaviour {
	public float fireRate = 0;
	public int Damage = 10;
	public LayerMask whatToHit;
	
	public Transform BulletTrailPrefab;
	public Transform HitPrefab;
	public Transform MuzzleFlashPrefab;
	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;
	
	// Handle camera shaking
	public float camShakeAmt = 0.05f;
	public float camShakeLength = 0.1f;
	CameraShake camShake;
	
	float timeToFire = 0;
	Transform firePoint;

	public int weaponDamage;
	private float FireRate, bulletLife;
	
	// Use this for initialization
	void Awake () {
		firePoint = transform.FindChild ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("No firePoint? WHAT?!");
		}
	}
	void Start()
	{
		camShake = GameMaster.gm.GetComponent<CameraShake>();
		if (camShake == null)
			Debug.LogError("No CameraShake script found on GM object.");
		weaponDamage = this.GetComponentInParent<Player> ().playerStats.Damage;
		if (this.GetComponentInParent<Player> ().weapon.name != "Laser") {
			FireRate = this.GetComponentInParent<Player> ().playerStats.FireRate;
			bulletLife = 4.0f;
		} else {
			bulletLife = 0.04f;
		}
	}
	// Update is called once per frame
	void Update () {
		GameObject controlPause = GameObject.Find ("PauseControl");
		if (!controlPause.GetComponent<PauseMenu> ().isPaused) {
			int fingercount = 0;
			if (FireRate == 0) {
				foreach (Touch touch in Input.touches) {
					if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
						fingercount++;
					}
					if (fingercount > 0) {
						Shoot ();
					}
				}
			} else {
				foreach (Touch touch in Input.touches) {
					if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
						fingercount++;
					}
					if (fingercount > 0 && Time.time > timeToFire) {
						timeToFire = Time.time + 1/FireRate;
						Shoot ();
					}
				}
			}
		}
	}
	void Shoot () {
			Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
			Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
			RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
			
			Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
			if (hit.collider != null) {
				Debug.DrawLine (firePointPosition, hit.point, Color.red);
				Enemy enemy = hit.collider.GetComponent<Enemy> ();
				if (enemy != null) {
					enemy.DamageEnemy (weaponDamage, true);
				}
				else{
					Destroy(hit.collider.gameObject);
					if(hit.collider.gameObject.layer == 11){
						hit.collider.gameObject.GetComponent<ItemScripts>().BombAction();
					}
					else if(hit.collider.gameObject.layer == 12){
						hit.collider.gameObject.GetComponent<ItemScripts>().ClockAction();
					}
					else if(hit.collider.gameObject.layer == 13){
						hit.collider.gameObject.GetComponent<ItemScripts>().HealthKitAction();
					}
					else if(hit.collider.gameObject.layer == 14){
						hit.collider.gameObject.GetComponent<ItemScripts>().CandyAction();
					}
				}
			}
			if (Time.time >= timeToSpawnEffect) {
				Vector3 hitPos;
				Vector3 hitNormal;
				if (hit.collider == null) {
					hitPos = (mousePosition - firePointPosition) * 30;
					hitNormal = new Vector3 (9999, 9999, 9999);
				} else {
					hitPos = hit.point;
					hitNormal = hit.normal;
				}
				Effect (hitPos, hitNormal);
				timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
	}
	
	void Effect(Vector3 hitPos, Vector3 hitNormal)	{
		Transform trail = Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
		LineRenderer lr = trail.GetComponent<LineRenderer> ();

		if (lr != null) {
			lr.SetPosition (0, firePoint.position);
			lr.SetPosition (1, hitPos);
		}
		
		Destroy (trail.gameObject, bulletLife);

		if (hitNormal != new Vector3(9999, 9999, 9999))
		{
			Transform hitParticle = Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation (Vector3.right, hitNormal)) as Transform;
			Destroy(hitParticle.gameObject, 0.1f);
		}
		
		Transform clone = Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);
		
		//Shake the camera
		camShake.Shake(camShakeAmt, camShakeLength);
	}
}