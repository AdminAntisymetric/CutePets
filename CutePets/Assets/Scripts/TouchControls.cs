using UnityEngine;
using System.Collections;

public class TouchControls : MonoBehaviour 
{
	private PauseMenu thePauseMenu;


	// Use this for initialization
	void Start () 
	{
		AudioButton ();
		thePauseMenu = FindObjectOfType<PauseMenu> ();
	}
	public void Pause()
	{
		AudioButton ();
		thePauseMenu.pauseUnpause ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void AudioButton(){
		audioManager audioManager = GameObject.FindGameObjectWithTag ("DataManager").GetComponent<audioManager> ();
		audioManager.PlaySound ("Boton");
	}
}