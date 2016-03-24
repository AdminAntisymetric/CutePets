using UnityEngine;
using System.Collections;

public class ShotgunScript : MonoBehaviour {
	public float fireRate = 0;
	public int Damage = 10;
	public LayerMask whatToHit;

	public Transform BulletTrailPrefab;
	public Transform HitPrefab;
	public Transform MuzzleFlashPrefab;
	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	public float camShakeAmt = 0.05f;
	public float camShakeLength = 0.1f;
	CameraShake camShake;

	float timeToFire = 0;
	Transform firePoint,firePoint2,firePoint3,firePoint4,firePoint5;

	private int weaponDamage;
	private float FireRate, bulletLife;

	void Awake () {
		firePoint = transform.FindChild ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("No firePoint? WHAT?!");
		}
		firePoint2 = transform.FindChild ("FirePoint2");
		if (firePoint2 == null) {
			Debug.LogError ("No firePoint? WHAT?!");
		}
		firePoint3 = transform.FindChild ("FirePoint3");
		if (firePoint3 == null) {
			Debug.LogError ("No firePoint? WHAT?!");
		}
		firePoint4 = transform.FindChild ("FirePoint4");
		if (firePoint4 == null) {
			Debug.LogError ("No firePoint? WHAT?!");
		}
		firePoint5 = transform.FindChild ("FirePoint5");
		if (firePoint5 == null) {
			Debug.LogError ("No firePoint? WHAT?!");
		}
	}
	// Use this for initialization
	void Start () {
		camShake = GameMaster.gm.GetComponent<CameraShake>();
		if (camShake == null)
			Debug.LogError("No CameraShake script found on GM object.");
		weaponDamage = this.GetComponentInParent<Player> ().playerStats.Damage;
		FireRate = this.GetComponentInParent<Player> ().playerStats.FireRate;
		//FireRate = 0;
		bulletLife = 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject controlPause = GameObject.Find ("PauseControl");
		if (!controlPause.GetComponent<PauseMenu> ().isPaused) {
			int fingercount = 0;
			if (FireRate == 0) {
				//foreach (Touch touch in Input.touches) {
				//	if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
				//		fingercount++;
				//	}
				//	if (fingercount > 0) {
						Shoot ();
				//	}
				//}
			} else {
				//foreach (Touch touch in Input.touches) {
				//	if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
				//		fingercount++;
				//	}
				//	if (fingercount > 0 && Time.time > timeToFire) {
				if(Time.time>timeToFire) {
						timeToFire = Time.time + 1/FireRate;
						Shoot ();
					}
				//}
			}
		}
	}
	void Shoot () {
		if (Time.time >= timeToSpawnEffect) {
			Effect ();
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
	}
	
	void Effect()	{
		Transform trail = Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
		Transform trail2 = Instantiate (BulletTrailPrefab, firePoint2.position, firePoint2.rotation) as Transform;
		Transform trail3 = Instantiate (BulletTrailPrefab, firePoint3.position, firePoint3.rotation) as Transform;
		Transform trail4 = Instantiate (BulletTrailPrefab, firePoint4.position, firePoint4.rotation) as Transform;
		Transform trail5 = Instantiate (BulletTrailPrefab, firePoint5.position, firePoint5.rotation) as Transform;
		
		Destroy (trail.gameObject, bulletLife);
		Destroy (trail2.gameObject, bulletLife);
		Destroy (trail3.gameObject, bulletLife);
		Destroy (trail4.gameObject, bulletLife);
		Destroy (trail5.gameObject, bulletLife);
		
		Transform clone = Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);
		
		//Shake the camera
		camShake.Shake(camShakeAmt, camShakeLength);
	}

	public void ShotgunFX(){
		Debug.Log (GameObject.FindGameObjectWithTag ("DataManager").GetComponent<audioManager> ());
		audioManager audioManager = GameObject.FindGameObjectWithTag ("DataManager").GetComponent<audioManager> ();
		audioManager.PlaySound("Shotgun");
	}
}