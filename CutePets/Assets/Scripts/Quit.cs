using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour {
	
	void OnMouseDown(){ // this object was clicked - do something 
		Debug.Log("APPLICATION QUIT!");
		Application.Quit(); }
}
