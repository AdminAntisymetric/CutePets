using UnityEngine;
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
	/*void OnCollisionEnter2D (Collision2D other){
		if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss") {
			Destroy (this.gameObject);
			other.gameObject.GetComponent<Enemy> ().DamageEnemy (damage, true);
		} else if (other.gameObject.tag == "Player"){
			Debug.Log ("Madres somos nosotros mismos xD");
			other.rigidbody.isKinematic=true;
		}
		other.rigidbody.isKinematic=false;
	}*/
}
