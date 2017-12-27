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

	// Use this for initialization
	void Start () {
		USD = GameObject.Find ("b_usd").GetComponent<Text> ();
		BTC = GameObject.Find ("b_btc").GetComponent<Text> ();
		ETH = GameObject.Find ("b_eth").GetComponent<Text> ();
		LTC = GameObject.Find ("b_ltc").GetComponent<Text> ();
		setBalances ();

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
			StartCoroutine (disable ());
		}
	}

	public void sell(){
		if (btcbalance - increment >= 0 && sellEnabled) {
			usdbalance += increment * currPrice;
			btcbalance -= increment;
			setBalances ();
			StartCoroutine (disable ());
		}
	}

	private IEnumerator disable(){
		buyEnabled = false;
		sellEnabled = false;
		Color dark = new Color32 (156, 156, 156, 255);
		Color white = new Color32 (255, 255, 255, 255);

		GameObject.Find ("button_buy").GetComponent<Image> ().color = dark;
		GameObject.Find ("button_sell").GetComponent<Image>().color = dark;
		float i = 0;
		while (i < disabledTimer) {
			yield return new WaitForSeconds (0.01f);
			i = i + 0.01f;
		}
		GameObject.Find ("button_buy").GetComponent<Image> ().color = white;
		GameObject.Find ("button_sell").GetComponent<Image> ().color = white;
		buyEnabled = true;
		sellEnabled = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		setBalances (); //TODO make this only every "frame"
	}
}
