using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

public void Quit ()
	{
		Debug.Log("APPLICATION QUIT!");
		Application.Quit();
	}

	public void Retry ()
	{
		AudioButton ();
		Application.LoadLevel(Application.loadedLevel);
	}
	public void LoadMenu(){
		AudioButton ();
		Application.LoadLevel(0);
	}
	public void AudioButton(){
		//Debug.Log (GameObject.FindGameObjectWithTag ("DataManager").GetComponent<audioManager> ());
		audioManager audioManager = GameObject.FindGameObjectWithTag ("DataManager").GetComponent<audioManager> ();
		audioManager.PlaySound ("Boton");
	}
}