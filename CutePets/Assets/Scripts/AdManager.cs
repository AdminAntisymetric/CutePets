using UnityEngine;
using System.Collections;
using admob;

public class AdManager : MonoBehaviour {
	GameMaster gmRef;
	bool adShown;
	// Use this for initialization
	void Start () {
		gmRef = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster> ();
		Admob.Instance().setTesting (true);
		Admob.Instance().initAdmob("ca-app-pub-7030539849658179/2969041847", "ca-app-pub-7030539849658179/2969041847");//set your admob id here
	}
	// Update is called once per frame
	void Update () {
		if (gmRef.gameOver && !adShown) {
			adShown = true;
			Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.TOP_CENTER, 0);
			Debug.Log ("Banner se Muestra");
		} else if(!gmRef.gameOver && !adShown){
			adShown = false;
			Admob.Instance().removeBanner();
			Debug.Log ("Banner se Quita");
		}
	}
}