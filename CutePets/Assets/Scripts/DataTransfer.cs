using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class DataTransfer : MonoBehaviour {
	public GameObject characterRef;
	public int highscore;
	public GameObject[] charactersSave;
	public bool saveScore = false, saveCharacters = false, loadCharacters = false;
	public int totalCharacters;
	SelectMenuScript characterData;

	[System.Serializable]
	class DataStored{
		public int scoreSave;
		public bool[] playableCharacters;
	}
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
	}
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("MainMenu") != null)
			Load ();

		if (GameObject.Find ("MainUI") != null) {
			characterData = GameObject.Find ("MainUI").GetComponent<SelectMenuScript>();
			characterRef = GameObject.Find ("MainUI").GetComponent<SelectMenuScript> ().selectedCharacter;
			totalCharacters = GameObject.Find ("MainUI").GetComponent<SelectMenuScript> ().characters.Length;
			charactersSave = new GameObject[totalCharacters];
			for (int i = 0; i < totalCharacters; i++) {
				charactersSave [i] = GameObject.Find ("MainUI").GetComponent<SelectMenuScript> ().characters [i];
			}
			if (loadCharacters) {
				loadCharacters = false;
				LoadCharacterData();
			}
		}
		if (saveScore) {
			saveScore = false;
			SaveScore ();
		}
		if (saveCharacters) {
			saveCharacters = false;
			SaveCharacterData();
		}
	}

	void SaveScore(){
		BinaryFormatter binary = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/score.data");
		DataStored gameData = new DataStored ();
		gameData.scoreSave = highscore;
		binary.Serialize (file, gameData);
		file.Close ();
	}
	void SaveCharacterData(){
		Debug.Log ("Guardamos info de los personajes actuales");
		BinaryFormatter binary = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/characters.data");
		DataStored gameData = new DataStored ();
		gameData.playableCharacters = new bool[totalCharacters];
		for (int i=0; i<totalCharacters; i++) {
			gameData.playableCharacters[i] = GameObject.Find ("MainUI").GetComponent<SelectMenuScript>().characters[i].GetComponent<Player>().playable;
			Debug.Log (gameData.playableCharacters[i]);
		}
		binary.Serialize (file, gameData);
		file.Close ();
	}
	void Load(){
		if(File.Exists(Application.persistentDataPath + "/score.data")){
			BinaryFormatter binary = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/score.data", FileMode.Open);
			DataStored gameData = (DataStored)binary.Deserialize(file);
			file.Close();
			if(GameObject.Find("MainMenu")!=null)
				GameObject.Find ("Highscore").GetComponent<Text>().text = gameData.scoreSave.ToString();
		}
	}
	void LoadCharacterData(){
		Debug.Log ("Cargamos info de los personajes actuales");
		if (File.Exists (Application.persistentDataPath + "/characters.data")) {
			BinaryFormatter binary = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/characters.data", FileMode.Open);
			DataStored gameData = (DataStored)binary.Deserialize(file);
			file.Close();
			for(int i=0; i<gameData.playableCharacters.Length; i++){
				GameObject.Find ("MainUI").GetComponent<SelectMenuScript>().characters[i].GetComponent<Player>().playable = gameData.playableCharacters[i];
			}
		}
	}
}