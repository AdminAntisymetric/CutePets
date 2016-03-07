using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemScripts : MonoBehaviour {
	private GameObject[] enemyList;
	private GameObject playerRef;
	public float timeEffect = 10;
	public int damage = 50;
	public int healthUp = 25;
	public int speedFactor = 2;
	Button bombButton, candyButton, clockButton, healthkitButton;

	// Use this for initialization
	void Start () {
		bombButton = GameObject.Find ("BombButton").GetComponent<Button>();
		candyButton = GameObject.Find ("CandyButton").GetComponent<Button>();
		clockButton = GameObject.Find ("ClockButton").GetComponent<Button>();
		healthkitButton = GameObject.Find ("HealthkitButton").GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
		//int fingercount = 0;
		//foreach (Touch touch in Input.touches) {
		//	if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
		//		fingercount++;
		//	if (fingercount > 0) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if(hit.collider != null)
				{
					if(hit.collider.gameObject.tag == "Bomb"){
						bombButton.interactable = true;
						Destroy (hit.collider.gameObject);
					}
					if(hit.collider.gameObject.tag == "Clock"){
						clockButton.interactable = true;
						Destroy (hit.collider.gameObject);
					}
					if(hit.collider.gameObject.tag == "HealthKit"){
						healthkitButton.interactable = true;
						Destroy (hit.collider.gameObject);
					}
					if(hit.collider.gameObject.tag == "Candy"){
						candyButton.interactable = true;
						Destroy (hit.collider.gameObject);
					}
				}
				Destroy (gameObject, 5);
		//	}
		//}
	}
}