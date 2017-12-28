using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarratorController : MonoBehaviour {

	private Text header, breakdown;
	private Image cover;
	private MainController mc;
	private int day = 0;
	private string breakdownstring = "starting balance: $400\nending balance: $450\nprofit: $50\n\n--------------------------------\nadditional expenses:\n--------------------------------\nfood: $50\nRent: $100\nFinal balance: $300";

	// Use this for initialization
	void Start () {
		header = GameObject.Find ("header").GetComponent<Text> ();
		breakdown = GameObject.Find ("breakdown").GetComponent<Text> ();
		cover = GameObject.Find ("cover").GetComponent<Image> ();
		mc = GameObject.Find ("MainController").GetComponent<MainController> ();
		day = mc.getDay ();
		StartCoroutine (fullAnimation ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator fullAnimation(){
		yield return new WaitForSeconds (2);
		float i = 255;
		while (i > 0) {
			i--;
			cover.color = new Color32 (255, 255, 255, i);
			yield return new WaitForSeconds (0.01f);
		}
		yield return new WaitForSeconds (0.2f);
		animateBox("Day "+day" over", header);
		yield return new WaitForSeconds(1);
		animateBox(breakdownstring, breakdown);

	}

	public IEnumerator animateBox(string s, Text t){ //i==1 for purchase text, 2 for select text, 3 for intro text
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
