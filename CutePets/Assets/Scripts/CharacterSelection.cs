using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using admob;

public class CharacterSelection : MonoBehaviour {
	public GameObject[] characters;
	int index;
	int totalCharacters;
	Transform spriteArea;
	GameObject unlock, select;
	public GameObject selectedCharacter;

	// Use this for initialization
	void Start () {
		index = 0;
		totalCharacters = characters.Length - 1;
		spriteArea = GameObject.Find ("CharacterSprite").transform;
		unlock = GameObject.Find ("UnlockButton");
		select = GameObject.Find ("SelectButton");
		unlock.SetActive(false);
		select.SetActive(false);
		Admob.Instance().initAdmob("ca-app-pub-7030539849658179/2969041847","ca-app-pub-7030539849658179/4499160641");//set your admob id here
		Admob.Instance ().setTesting (true);
		Admob.Instance ().loadInterstitial ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void IndexUp(){
		if (index < totalCharacters)
			index++;
		else
			index = 0;
		Debug.Log (index);
		DestroyCreate ();
		CheckPlayable ();
	}
	public void IndexDown(){
		if (index <= 0)
			index = totalCharacters;
		else
			index--;
		Debug.Log (index);
		DestroyCreate ();
		CheckPlayable ();
	}
	void DestroyCreate(){
		GameObject currCharacter = GameObject.FindGameObjectWithTag ("Player");
		Destroy (currCharacter);
		Instantiate (characters [index], spriteArea.position, spriteArea.rotation);
	}
	void CheckPlayable(){
		//Debug.Log (characters [index].GetComponent<Player> ().playable);
		bool isPlayable = characters [index].GetComponent<Player> ().playable;
		if (!isPlayable) {
			unlock.SetActive (true);
			select.SetActive (false);
		} else {
			unlock.SetActive (false);
			select.SetActive (true);
		}
	}
	public void InterstitialUnlock(){
		if (Admob.Instance ().isInterstitialReady ())
			Admob.Instance ().showInterstitial ();
		else
			Admob.Instance ().loadInterstitial ();

		characters [index].GetComponent<Player> ().playable = true;
	}
	public void SelectCharacter(){
		selectedCharacter = characters [index];
		Debug.Log (selectedCharacter);
	}
}