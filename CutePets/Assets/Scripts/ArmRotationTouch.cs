using UnityEngine;
using System.Collections;

public class ArmRotationTouch : MonoBehaviour {
	public int rotationOffset = 90;
	// Use this for initialization
	void Start () {		
	}
	// Update is called once per frame
	void Update () {
		int fingercount = 0;
		foreach (Touch touch in Input.touches) {
			if(touch.phase!=TouchPhase.Ended && touch.phase!=TouchPhase.Canceled){
				fingercount++;
			}
			if(fingercount>0){
				Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
				difference.Normalize ();
				
				float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
				rotZ = Mathf.Clamp (rotZ, -30, 110);
				transform.rotation = Quaternion.Euler (0f, 0f, rotZ + rotationOffset);
			}
		}
	}
}
