using UnityEngine;
using System.Collections;

public class TouchControls : MonoBehaviour 
{
	private PauseMenu thePauseMenu;


	// Use this for initialization
	void Start () 
	{
		thePauseMenu = FindObjectOfType<PauseMenu> ();
	}
	public void Pause()
	{
		thePauseMenu.pauseUnpause ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
