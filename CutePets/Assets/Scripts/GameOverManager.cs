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
		Application.LoadLevel(Application.loadedLevel);
	}
	public void LoadMenu(){
		Application.LoadLevel(0);
	}
}