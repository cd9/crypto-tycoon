using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartController : MonoBehaviour {

	public float chartWidth = 1000;
	public float chartHeight = 1500;
	public int divisionsX = 20;
	public int divisionsY = 10;
	public int updatesPerMove = 5;
	private int updatesSince = 0;
	public float updatesPerSecond = 10;
	float msSince = 0;
	int time = 0;
	public int offsetX = 3;
	private GameObject outline;
	private MarketData currData;
	private MainController mc;
	//private List<CandleStick> candlesticks = new List<CandleStick> (divisionsX - offsetX);
	//TODO implement this

	// Use this for initialization
	void Start () {
		mc = GameObject.Find ("MainController").GetComponent<MainController> ();
		outline = GameObject.Find ("outline");
		currData = mc.BitcoinData;
		print ("FIRST IS " + currData.priceAt(4));
	}

	void updateChart(int updatesSince){
		GameObject[] disposableGOs = GameObject.FindGameObjectsWithTag ("disposable");

		foreach (GameObject go in disposableGOs){
			GameObject.Destroy (go); //get rid of old candlesticks
		}

		float startX = (chartWidth / 2 * -1) + outline.GetComponent<RectTransform>().position.x;
		float startY = (chartHeight / 2 * -1) + outline.GetComponent<RectTransform>().position.y;
		float min = Mathf.Infinity;
		float max = Mathf.NegativeInfinity;
		float[] nowData = new float[divisionsX*updatesPerMove];
		for (int i = 0; i < divisionsX * updatesPerMove; i++) {
			float p = currData.priceAt (i+time); //get data of i + time
			if (p > max) max = p;
			if (p < min) min = p;
			nowData [i] = p;
		}
		max += 30;
		min -= 30;

		max = (int) max / 10;
		max *= 10;

		min = (int) min / 10;
		min *= 10;

		float mid = (max - min) / 2.0f;
		if (true) {
		
		}
		for (int i = offsetX; i < divisionsX; i++) { //calculate all the candlesticks
			//really bad cause I do this 20 times per update.

			float openc = currData.priceAt(time+updatesPerMove*i);
			float closec = currData.priceAt (updatesPerMove + time + updatesPerMove*i);
			float maxc = Mathf.NegativeInfinity;
			float minc = Mathf.Infinity;

			for (int j = 0; j <= updatesPerMove; j++) {
				float pc = currData.priceAt (time +updatesPerMove*i + j);
				if (pc > maxc)
					maxc = pc;
				if (pc < minc)
					minc = pc;
			}

			if (i==divisionsX-1) { //TODO fix redudancy
				closec = currData.priceAt (updatesSince + time + updatesPerMove*i);
				maxc = Mathf.NegativeInfinity;
				minc = Mathf.Infinity;

				for (int j = 0; j <= updatesSince; j++) {
					float pc = currData.priceAt (time +updatesPerMove*i + j);
					if (pc > maxc)
						maxc = pc;
					if (pc < minc)
						minc = pc;
				}
				mc.CurrPrice = closec;
				GameObject.Find("current_price").GetComponent<Text>().text = "PRICE: " +  System.Math.Round(closec, 2).ToString();
			}

			float factor = (chartHeight / (max - min));
			float yCandle = startY + factor * (openc - min + ((closec-openc)/2.0f));
			float yStick = startY + factor * (minc - min + ((maxc-minc)/2.0f));
			float xPos = startX + (chartWidth / divisionsX) * i;

			GameObject candlestick = GameObject.Instantiate (Resources.Load<GameObject>("candlestick"));
			candlestick.transform.SetParent(GameObject.Find("candlesticks").transform);

			foreach (RectTransform c in candlestick.GetComponentsInChildren<RectTransform>()){
				if (c.gameObject.name == "stick") {
					c.gameObject.transform.SetParent(GameObject.Find("candlesticks").transform);
					c.gameObject.transform.position = new Vector3 (xPos, yStick);
					c.sizeDelta = new Vector2 (c.sizeDelta.x, 1.2f*factor*(maxc-minc));
				}
				if (c.gameObject.name == "candle") {
					c.gameObject.transform.SetParent(GameObject.Find("candlesticks").transform);
					c.gameObject.transform.position = new Vector3 (xPos, yCandle);
					c.sizeDelta = new Vector2 (c.sizeDelta.x, factor*(closec-openc));
					if (closec - openc < 0) {
						c.gameObject.GetComponent<Image>().color = new Color32 (222, 0, 0, 255);
						c.gameObject.transform.position = new Vector3 (xPos, yCandle-(openc-closec));
						c.sizeDelta = new Vector2 (c.sizeDelta.x, factor*(openc-closec));
					}
				}
			}

			GameObject.Destroy (candlestick);
		}



		for (int i = 1; i < divisionsY; i++) {
			float yPos = startY + i*(chartHeight/divisionsY);
			float xPos = startX + chartWidth/2.0f;
			GameObject ydiv = GameObject.Instantiate (Resources.Load<GameObject>("ydiv"));
			Vector3 scale = ydiv.transform.localScale;
			ydiv.transform.SetParent (GameObject.Find ("grid").transform);
			ydiv.transform.localScale = scale;
			ydiv.transform.position = new Vector3 (xPos, yPos);
			ydiv.GetComponentInChildren<Text> ().text = (min + ((max-min)/divisionsY)*i).ToString();

		}


	}
		
	// Update is called once per frame
	void FixedUpdate () {
		currData = mc.BitcoinData;
		msSince = msSince + (Time.fixedDeltaTime*1000);
		if (updatesSince > updatesPerMove) {
			time += updatesPerMove;
			msSince = 0;
			updatesSince = 1;
			updateChart(updatesSince);
		} else if (msSince>=(1000/updatesPerSecond)){
			msSince = 0;
			updatesSince++;
			updateChart(updatesSince);

		}

	}
}
