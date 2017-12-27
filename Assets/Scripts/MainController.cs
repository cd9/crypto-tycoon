using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainController : MonoBehaviour {
	private MarketData bitcoinData = null;
	public float maxV = 30;
	public float skewUp = 1.1f;
	private Text USD, BTC, ETH, LTC;
	private float usdbalance = 1000;
	private float btcbalance = 0;
	private float increment = 1;
	private float currPrice;
	private bool buyEnabled = true;
	private bool sellEnabled = true;
	public float disabledTimer = 3.0f;
	private Text timer_text;
	public float dayLength = 120;
	private ChartController cc;



	public float CurrPrice{
		get{
			return currPrice;	
		}
		set{
			currPrice = value;
		}
	}


	public MarketData BitcoinData
	{
		get{
			if (bitcoinData == null) {
				bitcoinData = new MarketData ("bitcoin", "BTC", 3000, generatePriceData (3000, 100, maxV, skewUp));
			}
			return bitcoinData;
			
		}
		set{
			bitcoinData = value;
		}
	}

	float[] generatePriceData(int size, float startingprice, float maxVariance, float skew){
		float[] data = new float[size];
		float curr = startingprice;
		//generate random prices not sure how this will look.
		for (int i = 0; i<size; i++){
			data[i] = curr;
			curr = curr + Random.Range (-(maxVariance), maxVariance*skew);
		}
		return data;
	}

	public void IncInc(){
		//could just divide and multiply but might want other increments later
		increment *= 10;
		increment = (float)System.Math.Round (increment, 3);
		if (increment >= 1000)
			increment = 1000;
		updateInc ();
	}

	public void deIncInc(){
		increment = increment / 10.0f;
		if (increment <= 0.01f)
			increment = 0.01f;
		updateInc ();
	}

	void updateInc(){
		string imgname = "";
		if (increment == 0.01f) {
			imgname = "button_0_01";
		} else if (increment == 0.1f) {
			imgname = "button_0_1";
		} else if (increment == 1) {
			imgname = "button_1";
		} else if (increment == 10) {
			imgname = "button_10";
		} else if (increment == 100) {
			imgname = "button_100";
		} else if (increment == 1000) {
			imgname = "button_1000";
		} else {
			print (increment);
		}
		GameObject.Find ("button_inc").GetComponent<Image> ().sprite = Resources.Load<Sprite> (imgname);
	}

	// Use this for initialization
	void Start () {
		USD = GameObject.Find ("b_usd").GetComponent<Text> ();
		BTC = GameObject.Find ("b_btc").GetComponent<Text> ();
		ETH = GameObject.Find ("b_eth").GetComponent<Text> ();
		LTC = GameObject.Find ("b_ltc").GetComponent<Text> ();
		setBalances ();
		timer_text = GameObject.Find ("timer").GetComponent<Text> ();
		cc = GameObject.Find ("ChartController").GetComponent<ChartController> ();

	}

	void setBalances(){
		//TODO get these from save files
		USD.text = System.Math.Round(usdbalance, 2).ToString() + " USD";
		BTC.text = System.Math.Round (btcbalance, 2).ToString () + " BTC";
		ETH.text = "0 ETH";
		LTC.text = "0 LTC";
	}

	public void buy(){
		if (usdbalance - (increment * currPrice) >= 0 && buyEnabled) {
			usdbalance -= increment * currPrice;
			btcbalance += increment;
			setBalances ();
			StartCoroutine (disable ("buying"));
		}
	}

	public void sell(){
		if (btcbalance - increment >= 0 && sellEnabled) {
			usdbalance += increment * currPrice;
			btcbalance -= increment;
			setBalances ();
			StartCoroutine (disable ("selling"));
		}
	}

	private IEnumerator disable(string t){
		buyEnabled = false;
		sellEnabled = false;
		Color dark = new Color32 (156, 156, 156, 255);
		Color white = new Color32 (255, 255, 255, 255);

		GameObject.Find ("button_buy").GetComponent<Image> ().color = dark;
		GameObject.Find ("button_sell").GetComponent<Image>().color = dark;
		float i = 0;
		while (i < disabledTimer) {
			yield return new WaitForSeconds (0.001f);
			i = i + 0.01f;
			timer_text.text = t;
			for (float j = 0; j <= disabledTimer-i; j+=0.5f) {
				timer_text.text = timer_text.text + ".";
			}
		}
		timer_text.text = "";
		GameObject.Find ("button_buy").GetComponent<Image> ().color = white;
		GameObject.Find ("button_sell").GetComponent<Image> ().color = white;
		buyEnabled = true;
		sellEnabled = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		setBalances (); //TODO make this only every "frame"
		cc.getTime();

	}
}
