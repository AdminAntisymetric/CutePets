using UnityEngine;
using System.Collections;

public class AutoMovement : MonoBehaviour {
	public int smooth = 1;
	public int LifeSpan = 10;
	GameMaster gmRef;
	// Use this for initialization
	void Start () {
		gmRef = GameObject.FindWithTag ("GM").GetComponent<GameMaster> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * Time.deltaTime * smooth;
		if (gmRef.gameOver) {
			smooth = 0;
		} else {
			Destroy (this.gameObject,LifeSpan);
		}
	}
}