using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using admob;

public class SelectMenuScript : MonoBehaviour {
	public GameObject[] characters;
	public GameObject selectedCharacter, currCharacter;
	public Sprite select, selected;
	int index;
	int totalCharacters;
	Transform spriteArea;
	GameObject selectBtn, unlockBtn;

	// Use this for initialization
	void Start () {
		selectBtn = GameObject.Find ("SelectButton");
		unlockBtn = GameObject.Find ("UnlockButton");
		selectBtn.SetActive (false);
		unlockBtn.SetActive (false);
		index = 0;
		totalCharacters = characters.Length - 1;
		spriteArea = GameObject.Find ("CharacterSprite").transform;
		Instantiate (characters [index], spriteArea.position, spriteArea.rotation);
		currCharacter = characters [index];
		Admob.Instance().initAdmob("ca-app-pub-7030539849658179/2969041847","ca-app-pub-7030539849658179/4499160641");//set your admob id here
		Admob.Instance ().setTesting (true);
		Admob.Instance ().loadInterstitial ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		CheckPlayable ();
		if (currCharacter.name != selectedCharacter.name) {
			GameObject.Find ("SelectButton").GetComponent<Button> ().interactable = true;
			GameObject.Find ("SelectButton").GetComponent<Image>().sprite = select;
		} else {
			GameObject.Find ("SelectButton").GetComponent<Button> ().interactable = false;
			GameObject.Find ("SelectButton").GetComponent<Image>().sprite = selected;
		}
	}
	public void IndexUp(){
		if (index < totalCharacters)
			index++;
		else
			index = 0;
		DestroyCreate ();
		currCharacter = characters [index];
		CheckPlayable ();
	}
	public void IndexDown(){
		if (index <= 0)
			index = totalCharacters;
		else
			index--;
		DestroyCreate ();
		currCharacter = characters [index];
		CheckPlayable ();
	}
	void DestroyCreate(){
		currCharacter = GameObject.FindGameObjectWithTag ("Player");
		Destroy (currCharacter);
		Instantiate (characters [index],spriteArea.position, spriteArea.rotation);
	}
	void CheckPlayable(){
		bool isPlayable = characters [index].GetComponent<Player> ().playable;
		if (!isPlayable) {
			unlockBtn.SetActive (true);
			selectBtn.SetActive (false);
		} else {
			unlockBtn.SetActive (false);
			selectBtn.SetActive (true);
		}
	}
	public void ReturnMenu(){
		GameObject.FindWithTag ("DataManager").GetComponent<DataTransfer> ().saveCharacters = true;
		Application.LoadLevel (0);
	}
	public void SelectCharacter(){
		selectedCharacter = characters [index];
	}
	public void InterstitialUnlock(){
		/*if (Admob.Instance ().isInterstitialReady ())
			Admob.Instance ().showInterstitial ();
		else
			Admob.Instance ().loadInterstitial ();*/
		characters [index].GetComponent<Player> ().playable = true;
	}
}