using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsController : MonoBehaviour {

	public int timePerNews = 30;
	private float timeSinceNews = 0;

	// Use this for initialization
	void Start () {
		timeSinceNews = timePerNews-5;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timeSinceNews += Time.fixedDeltaTime;
		if (timeSinceNews > timePerNews) {
			StartCoroutine (animateNews (getNews ()));
			timeSinceNews = 0;
		}
	}

	private string getNews(){
		return "New in Crypto: vitalik buterin says he named it ethereum because it \"sounds cool lol\"";

	}

	public IEnumerator animateNews(string s){ //i==1 for purchase text, 2 for select text, 3 for intro text

		GameObject news = GameObject.Find ("news"); //clearing the speechbox
		Text t = news.GetComponent<Text> ();
		t.text = "";
		int i = 0;
		while (i < s.Length) {
			/*if (s [i] != ' ' && playsound&&PlayerPrefs.GetInt("isFX", 1)==1) {
				audio.Play ();
			}
			playsound = !playsound;*/
			t.text += s [i++];
			yield return new WaitForSeconds (0.03f);
		}
	}
}
