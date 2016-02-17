using UnityEngine;
using System.Collections;

public class Retry : MonoBehaviour {

	void OnMouseDown(){ // this object was clicked - do something 
		Application.LoadLevel(Application.loadedLevel); }
}
