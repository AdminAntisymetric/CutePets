using UnityEngine;
using System.Collections;

public class AutoMovement : MonoBehaviour {
	public int smooth = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * Time.deltaTime * smooth;
	}
}