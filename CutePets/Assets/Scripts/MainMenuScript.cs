using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
	public GameObject DataManager;
	void Awake(){
		if (GameObject.FindWithTag ("DataManager") == null)
			Instantiate (DataManager, DataManager.gameObject.transform.position, DataManager.gameObject.transform.rotation);
		GameObject.Find ("Highscore").GetComponent<Text> ().text = GameObject.FindGameObjectWithTag ("DataManager").GetComponent<DataTransfer> ().highscore.ToString ();
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayGame(){
		AudioButton ();
		Application.LoadLevel (2);
	}

	public void Selection(){
		AudioButton ();
		Application.LoadLevel (1);
		GameObject.FindGameObjectWithTag ("DataManager").GetComponent<DataTransfer> ().loadCharacters = true;
	}

	public void AudioButton(){
		//Debug.Log (GameObject.FindGameObjectWithTag ("DataManager").GetComponent<audioManager> ());
		audioManager audioManager = GameObject.FindGameObjectWithTag ("DataManager").GetComponent<audioManager> ();
		audioManager.PlaySound ("Boton");
	}
}